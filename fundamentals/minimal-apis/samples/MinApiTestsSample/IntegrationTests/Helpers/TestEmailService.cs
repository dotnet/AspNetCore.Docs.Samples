using WebMinRouteGroup.Services;

namespace IntegrationTests.Helpers;

public class TestEmailService : IEmailService
{
    public Task Send(string emailAddress, string body)
    {
        // You don't want to send real email when running integration tests
        return Task.CompletedTask;
    }
}
