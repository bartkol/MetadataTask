namespace FivetranClient.Models;

/// <summary>
/// Represents a Fivetran table.
/// </summary>
/// <param name="NameInDestination">Name of a table in destination database.</param>
public record Table(string NameInDestination);