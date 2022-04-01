namespace SignalRHubsSample.Snippets.Hubs;

// <snippet_Interface>
public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}
// </snippet_Interface>
