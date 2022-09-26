using System.Net;
using IntegrationTests.Helpers;

namespace IntegrationTests;

public class TokenEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public TokenEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    public static IEnumerable<object[]> Credentials => new List<object[]>
    {
        new object[] { "admin", "admin", HttpStatusCode.OK },
        new object[] { "lorem", "ipsum", HttpStatusCode.Unauthorized }
    };

    [Theory]
    [MemberData(nameof(Credentials))]
    public async Task GetTokeEndpointReturnsOkWhenValidCredentials(string username, string password, HttpStatusCode code)
    {
        // Arrange
        var client = _factory.CreateClient();
        
        client.DefaultRequestHeaders.Add("username", username);
        client.DefaultRequestHeaders.Add("password", password);
        
        var response = await client.GetAsync("/token");

        // Assert
        Assert.Equal(code, response.StatusCode);
    }
}
