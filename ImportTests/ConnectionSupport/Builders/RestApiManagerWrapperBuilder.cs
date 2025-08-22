using FivetranClient;
using FivetranClient.Models;
using Import.Helpers.Fivetran;
using NSubstitute;

namespace ImportTests.ConnectionSupport.Builders;

public class RestApiManagerWrapperBuilder
{
    private IRestApiManager _restApiManager = Substitute.For<IRestApiManager>();
    private string _groupId = "test_group_id";

    public RestApiManagerWrapperBuilder WithTimeout(TimeSpan timeout)
    {
        _restApiManager = Substitute.For<RestApiManager>(Arg.Any<string>(), Arg.Any<string>(), timeout);
        return this;
    }

    public RestApiManagerWrapperBuilder WithConnectors()
    {
        var connectors = new List<Connector>() {
            new("connector1", "test", "testSchema", null),
            new("connector2", "test", "testSchema", null)
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
        var schemas = new DataSchemas(
            new Dictionary<string, Schema?>
            {
                { "connector1", new Schema("Schema1", null, []) },
                { "connector2", new Schema("Schema2", null, []) }
            }
        );
        _restApiManager.GetConnectorSchemasAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(schemas);
        return this;
    }

    public static RestApiManagerWrapperBuilder Given() => new();

    public RestApiManagerWrapper Build()
    {
        return new RestApiManagerWrapper(_restApiManager, _groupId);
    }
}