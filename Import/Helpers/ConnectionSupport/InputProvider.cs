using Microsoft.Extensions.Logging;

namespace Import.Helpers.ConnectionSupport;

/// <inheritdoc/>
public class InputProvider : IInputProvider
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the InputProvider class with the specified logger
    /// </summary>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
    public InputProvider(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
    }

    /// <inheritdoc />
    public string GetApiKey()
    {
        _logger.LogInformation("Provide your Fivetran API Key: ");
        return GetString();
    }

    /// <inheritdoc />
    public string GetApiSecret()
    {
        _logger.LogInformation("Provide your Fivetran API Secret: ");
        return GetString();
    }

    /// <inheritdoc />
    public int GetGroupIndex()
    {
        _logger.LogInformation("Please select a group to import from (by number):");
        return GetNumber();
    }

    private string GetString()
    {
        var input = Console.ReadLine();
        return input ?? string.Empty;
    }

    private int GetNumber()
    {
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) ||
            !int.TryParse(input, out var number))
        {
            throw new ArgumentException("Invalid input. Please enter a valid number.");
        }

        return number;
    }
}