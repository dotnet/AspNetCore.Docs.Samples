using Microsoft.AspNetCore.SignalR;

namespace SignalRHubsSample.Hubs;

// <snippet_Class>
public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
        => await Clients.All.SendAsync("ReceiveMessage", user, message);
}
// </snippet_Class>
