using Import.ConnectionSupport;
using Import.Helpers.ConnectionSupport;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ImportTests.ConnectionSupport.Builders;

public class FivetranConnectionSupportBuilder
{
    private ILogger _logger = Substitute.For<ILogger<FivetranConnectionSupport>>();
    private IInputProvider _inputProvider = Substitute.For<IInputProvider>();

    public FivetranConnectionSupportBuilder()
    {
        _inputProvider.GetApiKey().Returns("default_test_key");
        _inputProvider.GetApiSecret().Returns("default_test_secret");
    }
    
    public FivetranConnectionSupportBuilder WithLogger(ILogger logger) { _logger = logger; return this; }
    public FivetranConnectionSupportBuilder WithInputProvider(IInputProvider inputProvider) { _inputProvider = inputProvider; return this; }
    public FivetranConnectionSupportBuilder WithApiKey(string? apiKey)
    {
        _inputProvider.GetApiKey().Returns(apiKey);
        return this;
    }
    public FivetranConnectionSupportBuilder WithApiSecret(string? apiSecret)
    {
        _inputProvider.GetApiSecret().Returns(apiSecret);
        return this;
    }

    public static FivetranConnectionSupportBuilder Given() => new FivetranConnectionSupportBuilder();

    public FivetranConnectionSupport Build()
    {
        return new FivetranConnectionSupport(_logger, _inputProvider);
    }
}