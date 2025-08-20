using Import.ConnectionSupport;

namespace ImportTests.ConnectionSupport.Builders;

public class FivetranConnectionDetailsForSelectionBuilder
{
    private string _apiKey = string.Empty;
    private string _apiSecret = string.Empty;
    private TimeSpan _timeout = default;

    public FivetranConnectionDetailsForSelectionBuilder WithApiKey(string apiKey) { _apiKey = apiKey; return this;}

    public FivetranConnectionDetailsForSelectionBuilder WithApiSecret(string apiSecret) { _apiSecret = apiSecret; return this; }

    public FivetranConnectionDetailsForSelectionBuilder WithTimeout(TimeSpan timeout) { _timeout = timeout; return this; }

    public static FivetranConnectionDetailsForSelectionBuilder Given() => new FivetranConnectionDetailsForSelectionBuilder();

    public FivetranConnectionDetailsForSelection Build()
    {
        return new FivetranConnectionDetailsForSelection(_apiKey, _apiSecret, _timeout);
    }
}