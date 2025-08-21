using FluentAssertions;
using FluentAssertions.Execution;
using Import.ConnectionSupport;
using Import.Helpers.Fivetran;
using ImportTests.ConnectionSupport.Builders;
using NSubstitute;

namespace ImportTests.ConnectionSupport;

public class ConnectionSupportTests
{
    [Trait("Method", "GetConnectionDetailsForSelection")]
    [Theory(DisplayName = "GetConnectionDetailsForSelection should throw ArgumentNullException when API Key or Secret is null or empty")]
    [InlineData(null, "test_api_secret")]
    [InlineData("", "test_api_secret")]
    [InlineData("test_api_key", null)]
    [InlineData("test_api_key", "")]
    public void GetConnectionDetailsForSelection_ShouldThrowArgumentNullException(string? apiKey, string? apiSecret)
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given()
            .WithApiKey(apiKey)
            .WithApiSecret(apiSecret)
            .Build();

        // Act
        Action act = () => connectionSupport.GetConnectionDetailsForSelection();

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Trait("Method", "GetConnectionDetailsForSelection")]
    [Fact(DisplayName = "GetConnectionDetailsForSelection should not throw when valid API Key and Secret are provided")]
    public void GetConnectionDetailsForSelection_ShouldNotThrowAnyException()
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given()
            .WithApiKey("test_api_key")
            .WithApiSecret("test_api_secret")
            .Build();

        // Act
        Action act = () => connectionSupport.GetConnectionDetailsForSelection();

        // Assert
        act.Should().NotThrow();
    }

    [Trait("Method", "GetConnectionDetailsForSelection")]
    [Fact(DisplayName = "GetConnectionDetailsForSelection should return a non-null object of type FivetranConnectionDetailsForSelection")]
    public void GetConnectionDetailsForSelection_ShouldReturnObject()
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given().Build();

        // Act
        var result = connectionSupport.GetConnectionDetailsForSelection();

        // Assert
        result.Should().NotBeNull()
            .And.BeOfType<FivetranConnectionDetailsForSelection>();
    }

    [Trait("Method", "GetConnection")]
    [Theory(DisplayName = "GetConnection should throw ArgumentException if provided connectionDetails are invalid")]
    [InlineData(null)]
    [InlineData("")]
    public void GetConnection_ShouldThrowArgumentException_WhenInputedInvalidData(object? connectionDetails)
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given().Build();

        // Act
        Action act = () => connectionSupport.GetConnection(connectionDetails, null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid connection details provided.*")
            .WithParameterName("connectionDetails");
    }

    [Trait("Method", "GetConnection")]
    [Theory(DisplayName = "GetConnection should throw ArgumentNullException if provided selectedToImport are invalid")]
    [InlineData(null)]
    [InlineData("")]
    public void GetConnection_ShouldThrowArgumentNullException_WhenInputedInvalidData(string? selectedToImport)
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given().Build();
        var connectionDetails = FivetranConnectionDetailsForSelectionBuilder.Given()
            .WithApiKey("test_api_key2")
            .WithApiSecret("test_api_secret2")
            .WithTimeout(TimeSpan.FromSeconds(30))
            .Build();

        // Act
        Action act = () => connectionSupport.GetConnection(connectionDetails, selectedToImport);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Input parameter selectedToImport cannot be null or empty.*")
            .WithParameterName("selectedToImport");
    }

    [Trait("Method", "GetConnection")]
    [Fact(DisplayName = "GetConnection should not throw any exception if input data is valid")]
    public void GetConnection_ShouldNotThrowAnyException_WhenInputDataIsValid()
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given().Build();
        var connectionDetails = FivetranConnectionDetailsForSelectionBuilder.Given()
            .WithApiKey("test_api_key")
            .WithApiSecret("test_api_secret")
            .WithTimeout(TimeSpan.FromSeconds(30))
            .Build();

        // Act
        Action act = () => connectionSupport.GetConnection(connectionDetails, "FivetranGroup1");

        // Assert
        act.Should().NotThrow();
    }

    [Trait("Method", "GetConnection")]
    [Fact(DisplayName = "GetConnection should not throw any exception if input data is valid")]
    public void GetConnection_ShouldReturnObject()
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given().Build();
        var connectionDetails = FivetranConnectionDetailsForSelectionBuilder.Given()
            .WithApiKey("test_api_key")
            .WithApiSecret("test_api_secret")
            .WithTimeout(TimeSpan.FromSeconds(30))
            .Build();

        // Act
        var result = connectionSupport.GetConnection(connectionDetails, "FivetranGroup1");

        // Assert
        result.Should().NotBeNull()
            .And.BeOfType<RestApiManagerWrapper>();
    }

    [Trait("Method", "CloseConnection")]
    [Fact(DisplayName = "CloseConnection should throw ArgumentException when invalid connection type is provided")]
    public void CloseConnection_ShouldThrowException_WhenInvalidInputDataIsProvided()
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given()
            .Build();

        // Act
        Action act = () => connectionSupport.CloseConnection(null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid connection type provided.*")
            .WithParameterName("connection");
    }

    [Trait("Method", "CloseConnection")]
    [Fact(DisplayName = "CloseConnection should not throw any exception and dispose connection when valid connection is provided")]
    public void CloseConnection_ShouldProperlyDisposeObject()
    {
        // Arrange
        var connectionSupport = FivetranConnectionSupportBuilder.Given().Build();
        var connection = RestApiManagerWrapperBuilder.Given().Build();

        // Act
        Action act = () => connectionSupport.CloseConnection(connection);

        // Assert
        using (new AssertionScope())
        {
            act.Should().NotThrow();
            connection.RestApiManager.Received(1).Dispose();
        }
    }
}