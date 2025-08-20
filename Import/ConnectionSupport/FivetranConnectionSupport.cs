using FivetranClient;
using Import.Helpers.ConnectionSupport;
using Import.Helpers.Fivetran;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Group = FivetranClient.Models.Group;

namespace Import.ConnectionSupport;

/// <summary>
/// Provides support for Fivetran connections.
/// Equivalent of database is group in Fivetran terminology
/// </summary>
public partial class FivetranConnectionSupport : IConnectionSupport
{
    public const string ConnectorTypeCode = "FIVETRAN";
    private readonly ILogger _logger;
    private readonly IInputProvider _inputProvider;
    private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(40);

    #region Constructors
    /// <summary>
    /// Default constructor for FivetranConnectionSupport.
    /// Initializes the logger and input provider with default implementations.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when logger or input provider are null.</exception>
    public FivetranConnectionSupport()
    {
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<FivetranConnectionSupport>();
        _inputProvider = new InputProvider(_logger);
    }

    /// <summary>
    /// Constructor for FivetranConnectionSupport with custom logger and input provider.
    /// This allows for dependency injection of the logger and input provider.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="inputProvider"></param>
    /// <exception cref="ArgumentNullException">Thrown when logger or inputProvider are null.</exception>
    public FivetranConnectionSupport(ILogger logger, IInputProvider inputProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        _inputProvider = inputProvider ?? throw new ArgumentNullException(nameof(inputProvider), "InputProvider cannot be null.");
    }
    #endregion

    #region Connection methods
    /// <summary>
    /// Provides connection details for Fivetran selection.
    /// </summary>
    /// <returns>Object type of FivetranConnectionDetailsForSelection which holds apiKey and apiSecret</returns>
    /// <exception cref="ArgumentException">Thrown when API Key or Secret are null or empty.</exception>
    public object? GetConnectionDetailsForSelection()
    {
        var apiKey = _inputProvider.GetApiKey();
        var apiSecret = _inputProvider.GetApiSecret();
        return new FivetranConnectionDetailsForSelection(apiKey, apiSecret, _defaultTimeout);
    }

    /// <summary>
    /// Gets a connection to Fivetran using the provided connection details and selected group to import.
    /// This method creates a RestApiManager instance with the provided API key and secret, and wraps it in a RestApiManagerWrapper.
    /// The selectedToImport parameter specifies which group to import from.
    /// </summary>
    /// <param name="connectionDetails">Must be an object of type FivetranConnectionDetailsForSelection.</param>
    /// <param name="selectedToImport">Fivetran Group to import from.</param>
    /// <returns>Instance of RestApiManagerWrapper</returns>
    /// <exception cref="ArgumentException">Thrown when provided connectionDetails is null or wrong type.</exception>
    /// <exception cref="ArgumentNullException">Thrown when selectedToImport is null or empty.</exception>
    public object GetConnection(object? connectionDetails, string? selectedToImport)
    {
        var details = ValidateConnectionDetails(connectionDetails);

        if (string.IsNullOrWhiteSpace(selectedToImport))
        {
            throw new ArgumentException("Input parameter selectedToImport cannot be null or empty.", nameof(selectedToImport));
        }

        var restApiManager = new RestApiManager(details.ApiKey, details.ApiSecret, details.Timeout);

        return new RestApiManagerWrapper(restApiManager, selectedToImport);
    }

    /// <summary>
    /// Closes the connection to Fivetran by disposing of the RestApiManagerWrapper.
    /// GetConnection must be called before this method to ensure a valid connection object is provided.
    /// </summary>
    /// <param name="connection">Must be an object of type RestApiManagerWrapper.</param>
    /// <exception cref="ArgumentException">Thrown when provided connection is null or wrong type.</exception>
    public void CloseConnection(object? connection)
    {
        var restApiManagerWrapper = ValidateConnection(connection);
        restApiManagerWrapper.Dispose();
    }
    #endregion

    #region Import methods
    /// <summary>
    /// Selects a group to import from Fivetran based on the provided connection details.
    /// </summary>
    /// <param name="connectionDetails">Must be an object of type FivetranConnectionDetailsForSelection.</param>
    /// <returns>ID of group provided in connectionDetails</returns>
    /// <exception cref="ArgumentException">Thrown when provided connectionDetails is null or wrong type.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no group is found with the provided connectionDetails.</exception>
    public string SelectToImport(object? connectionDetails)
    {
        var details = ValidateConnectionDetails(connectionDetails);

        using var restApiManager = new RestApiManager(details.ApiKey, details.ApiSecret, _defaultTimeout);

        var groups = GetGroupsAsync(restApiManager).GetAwaiter().GetResult();
        var selectedGroupIndex = _inputProvider.GetGroupIndex();

        if (selectedGroupIndex < 1 ||
            selectedGroupIndex > groups.Count())
        {
            throw new ArgumentOutOfRangeException(nameof(selectedGroupIndex), "Invalid group selection.");
        }

        var selectedGroup = groups.ElementAt(selectedGroupIndex - 1);
        return selectedGroup.Id;
    }

    private async Task<List<Group>> GetGroupsAsync(RestApiManager restApiManager)
    {
        var groups = new List<Group>();
        var hasHeaderPrinted = false;
        var elementIndex = 1;

        await foreach (var group in restApiManager.GetGroupsAsync(CancellationToken.None))
        {
            groups.Add(group);

            if (!hasHeaderPrinted)
            {
                _logger.LogInformation("Available groups in Fivetran account:");
                hasHeaderPrinted = true;
            }

            _logger.LogInformation("{Index}. {Name} (ID: {Id})", elementIndex++, group.Name, group.Id);
        }

        if (!groups.Any())
        {
            throw new InvalidOperationException("No groups found in Fivetran account.");
        }

        return groups;
    }

    /// <summary>
    /// Runs the import process for Fivetran by fetching and displaying lineage mappings based on the provided connection.
    /// </summary>
    /// <param name="connection">Must be an object of type RestApiManagerWrapper.</param>
    /// <exception cref="ArgumentException">Thrown when provided connection is null or wrong type.</exception>
    public void RunImport(object? connection)
    {
        var restApiManagerWrapper = ValidateConnection(connection);
        var mappings = GetMappings(restApiManagerWrapper).GetAwaiter().GetResult();

        foreach (var line in mappings)
        {
            _logger.LogInformation(line);
        }
    }

    private async Task<ConcurrentBag<string>> GetMappings(RestApiManagerWrapper restApiManagerWrapper)
    {
        var restApiManager = restApiManagerWrapper.RestApiManager;
        var groupId = restApiManagerWrapper.GroupId;
        var allMappingsBuffer = new ConcurrentBag<string>() { "Lineage mappings:\n" };
        var foundAnyConnector = false;

        await foreach (var connector in restApiManager.GetConnectorsAsync(groupId, CancellationToken.None))
        {
            foundAnyConnector = true;
            var connectorSchemas = restApiManager
                .GetConnectorSchemasAsync(connector.Id, CancellationToken.None)
                .Result;

            foreach (var schema in connectorSchemas?.Schemas ?? [])
            {
                foreach (var table in schema.Value?.Tables ?? [])
                {
                    allMappingsBuffer.Add($"  {connector.Id}: {schema.Key}.{table.Key} -> {schema.Value?.NameInDestination}.{table.Value.NameInDestination}\n");
                }
            }
        }

        if (!foundAnyConnector)
        {
            throw new InvalidOperationException($"No connectors found in the selected group ({groupId}).");
        }

        if (allMappingsBuffer.IsEmpty)
        {
            allMappingsBuffer.Add($"No mappings found in the selected group ({groupId}).");
        }

        return allMappingsBuffer;
    }
    #endregion

    #region Validation methods
    private RestApiManagerWrapper ValidateConnection(object? connection)
    {
        if (connection is null ||
            connection is not RestApiManagerWrapper restApiManagerWrapper)
        {
            throw new ArgumentException("Invalid connection type provided.", nameof(connection));
        }
        return restApiManagerWrapper;
    }

    private FivetranConnectionDetailsForSelection ValidateConnectionDetails(object? connectionDetails)
    {
        if (connectionDetails is null ||
            connectionDetails is not FivetranConnectionDetailsForSelection details)
        {
            throw new ArgumentException("Invalid connection details provided.", nameof(connectionDetails));
        }
        return details;
    }
    #endregion
}