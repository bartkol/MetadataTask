namespace FivetranClient.Models;

/// <summary>
/// Represents a Fivetran group.
/// </summary>
/// <param name="Id">Id of a group.</param>
/// <param name="Name">Name of a group.</param>
public record Group(string Id, string Name);