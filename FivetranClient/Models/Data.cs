namespace FivetranClient.Models;

/// <summary>
/// Represents a collection of items with pagination support.
/// </summary>
/// <typeparam name="T">Type of objects in this collection.</typeparam>
/// <param name="Items">Collection of objects T</param>
/// <param name="NextCursor"></param>
public record Data<T>(List<T> Items, string? NextCursor);