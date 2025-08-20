using FivetranClient;

namespace Import.Helpers.Fivetran;

/// <summary>
/// Wrapper for RestApiManager to manage Fivetran API interactions.
/// This class encapsulates the RestApiManager and the group ID, providing a way to manage API calls.
/// It implements IDisposable to ensure proper resource management.
/// </summary>
public class RestApiManagerWrapper : IDisposable
{
    /// <summary>
    /// API manager for Fivetran interactions.
    /// This is used to perform API operations such as fetching data, managing connections, etc.
    /// It cannot be null and must be initialized before use.
    /// </summary>
    public RestApiManager RestApiManager { get; }

    /// <summary>
    /// Group ID for the Fivetran connection.
    /// This is used to identify the specific group for API operations.
    /// It cannot be null or empty.
    /// </summary>
    public string GroupId { get; }
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the RestApiManagerWrapper class with the specified RestApiManager and group ID.
    /// </summary>
    /// <param name="restApiManager"></param>
    /// <param name="groupId"></param>
    /// <exception cref="ArgumentNullException">Thrown when restApiManager is null.</exception>
    /// <exception cref="ArgumentException">Thrown when groupId is null or empty.</exception>
    public RestApiManagerWrapper(RestApiManager restApiManager, string groupId)
    {
        RestApiManager = restApiManager ?? throw new ArgumentNullException(nameof(restApiManager), "RestApiManager cannot be null.");

        if (string.IsNullOrWhiteSpace(groupId))
        {
            throw new ArgumentException("Group ID cannot be null or empty.", nameof(groupId));
        }

        GroupId = groupId;
    }

    /// <summary>
    /// Disposes the RestApiManagerWrapper, releasing the resources held by the RestApiManager.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        RestApiManager.Dispose();
        _disposed = true;
    }
}