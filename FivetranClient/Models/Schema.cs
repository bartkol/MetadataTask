namespace FivetranClient.Models;

/// <summary>
/// Represents a schema for a Fivetran connector.
/// </summary>
/// <param name="NameInDestination">Schema name in destination database.</param>
/// <param name="Enabled"></param>
/// <param name="Tables">Dictionary of tables within this schema.</param>
public record Schema(
    string NameInDestination,
    bool? Enabled,
    Dictionary<string, Table> Tables
);