using FivetranClient;
using FivetranClient.Models;
using Import.Helpers.Fivetran;
using NSubstitute;

namespace ImportTests.ConnectionSupport.Builders;

public class RestApiManagerWrapperBuilder
{
    private RestApiManager _restApiManager = Substitute.For<RestApiManager>(Arg.Any<string>(), Arg.Any<string>(), TimeSpan.FromSeconds(40));
    private string _groupId = "test_group_id";

    public RestApiManagerWrapperBuilder WithTimeout(TimeSpan timeout)
    {
        _restApiManager = Substitute.For<RestApiManager>(Arg.Any<string>(), Arg.Any<string>(), timeout);
        return this;
    }

    public RestApiManagerWrapperBuilder WithConnectors()
    {
        var connectors = new List<Connector>() {
            new() { Id = "connector1" },
            new() { Id = "connector2" }
        };
        
        _restApiManager.GetConnectorsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => GetAsyncConnectors(connectors));
        return this;
    }

    private async IAsyncEnumerable<Connector> GetAsyncConnectors(IEnumerable<Connector> connectors)
    {
        foreach (var connector in connectors)
        {
            yield return connector;
            await Task.Yield();
        }
    }

    public RestApiManagerWrapperBuilder WithConnectorsSchemas()
    {
        var schemas = new DataSchemas
        {
            Schemas = new Dictionary<string, Schema?>
            {
                { "connector1", new Schema { NameInDestination = "Schema1" } },
                { "connector2", new Schema { NameInDestination = "Schema2" } }
            }
        };
        _restApiManager.GetConnectorSchemasAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(schemas);
        return this;
    }

    public static RestApiManagerWrapperBuilder Given() => new RestApiManagerWrapperBuilder();

    public RestApiManagerWrapper Build()
    {
        return new RestApiManagerWrapper(_restApiManager, _groupId);
    }
}