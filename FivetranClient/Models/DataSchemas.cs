namespace FivetranClient.Models;

/// <summary>
/// Represents a collection of schemas for a Fivetran connector.
/// </summary>
/// <param name="Schemas"></param>
public record DataSchemas(Dictionary<string, Schema?> Schemas);