namespace FivetranClient.Models;

/// <summary>
/// Represents a Fivetran connector.
/// </summary>
/// <param name="Id">Id of a connector.</param>
/// <param name="Service"></param>
/// <param name="Schema"></param>
/// <param name="Paused"></param>
public record Connector(
    string Id,
    string Service,
    string Schema,
    bool? Paused
);