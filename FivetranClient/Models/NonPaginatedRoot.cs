namespace FivetranClient.Models;

/// <summary>
/// Represents a non-paginated response containing a single item.
/// </summary>
/// <typeparam name="T">Type of object in response.</typeparam>
/// <param name="Data">Collection of objects in response.</param>
public record NonPaginatedRoot<T>(T? Data);