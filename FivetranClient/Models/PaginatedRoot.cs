namespace FivetranClient.Models;

/// <summary>
/// Represents a paginated response containing a collection of items.
/// </summary>
/// <typeparam name="T">Type of object in response.</typeparam>
/// <param name="Data">Collection of objects in response.</param>
public record PaginatedRoot<T>(Data<T> Data);