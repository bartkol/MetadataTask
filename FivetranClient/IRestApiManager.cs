using FivetranClient.Models;

namespace FivetranClient;

/// <summary>
/// Interface for managing REST API interactions with Fivetran.
/// </summary>
public interface IRestApiManager : IDisposable
{
    /// <summary>
    /// Gets an asynchronous enumerable of groups.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable of Group objects.</returns>
    public IAsyncEnumerable<Group> GetGroupsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets an asynchronous enumerable of connectors for a specified group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>An asynchronous enumerable of Connect objects.</returns>
    public IAsyncEnumerable<Connector> GetConnectorsAsync(string groupId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the schemas for a specified connector asynchronously.
    /// </summary>
    /// <param name="connectorId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>An asynchronous enumerable of DataSchema objects.</returns>
    public Task<DataSchemas?> GetConnectorSchemasAsync(string connectorId, CancellationToken cancellationToken);
}