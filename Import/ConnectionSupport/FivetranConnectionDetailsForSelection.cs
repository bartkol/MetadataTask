namespace Import.ConnectionSupport;

/// <summary>
/// Stores connection details (ApiKey, ApiSecret and Timeout) for Fivetran selection.
/// </summary>
public class FivetranConnectionDetailsForSelection
{
    /// <summary>
    /// API Key for Fivetran connection.
    /// This is used to authenticate API requests.
    /// It cannot be null or empty.
    /// </summary>
    public string ApiKey { get; }

    /// <summary>
    /// API Secret for Fivetran connection.
    /// This is used to authenticate API requests along with the API Key.
    /// It cannot be null or empty.
    /// </summary>
    public string ApiSecret { get; }

    /// <summary>
    /// Timeout for Fivetran API requests.
    /// This defines how long the system will wait for a response before timing out.
    /// It must be greater than zero.
    /// </summary>
    public TimeSpan Timeout { get; }

    /// <summary>
    /// Initializes a new instance of the FivetranConnectionDetailsForSelection class.
    /// Validates the API Key, API Secret, and Timeout parameters.
    /// Throws exceptions if any of the parameters are invalid.
    /// </summary>
    /// <param name="apiKey">The API Key for Fivetran connection.</param>
    /// <param name="apiSecret">The API Secret for Fivetran connection.</param>
    /// <param name="timeout">The timeout duration for Fivetran API requests.</param>
    /// <exception cref="ArgumentNullException">Thrown when API Key or Secret is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when Timeout is less than or equal to zero.</exception>
    /// </summary>
    public FivetranConnectionDetailsForSelection(string apiKey, string apiSecret, TimeSpan timeout)
    {
        ApiKey = ValidateApiKey(apiKey);
        ApiSecret = ValidateApiSecret(apiSecret);
        Timeout = ValidateTimeout(timeout);
    }

    private string ValidateApiKey(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException(nameof(apiKey), "API Key cannot be null or empty.");
        }
        return apiKey;
    }

    private string ValidateApiSecret(string apiSecret)
    {
        if (string.IsNullOrWhiteSpace(apiSecret))
        {
            throw new ArgumentException(nameof(apiSecret), "API Secret cannot be null or empty.");
        }
        return apiSecret;
    }

    private TimeSpan ValidateTimeout(TimeSpan timeout)
    {
        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentException("Timeout must be greater than zero.", nameof(timeout));
        }
        return timeout;
    }
}