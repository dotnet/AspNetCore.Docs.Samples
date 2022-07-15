using Microsoft.AspNetCore.SignalR;

namespace SignalRHubsSample.Snippets.Hubs;

public class ChatHub : Hub
{
    // <snippet_OnConnectedAsync>
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
        await base.OnConnectedAsync();
    }
    // </snippet_OnConnectedAsync>

    // <snippet_OnDisconnectedAsync>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
    // </snippet_OnDisconnectedAsync>

    // <snippet_Clients>
    public async Task SendMessage(string user, string message)
        => await Clients.All.SendAsync("ReceiveMessage", user, message);

    public async Task SendMessageToCaller(string user, string message)
        => await Clients.Caller.SendAsync("ReceiveMessage", user, message);

    public async Task SendMessageToGroup(string user, string message)
        => await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);
    // </snippet_Clients>

    // <snippet_HubMethodName>
    [HubMethodName("SendMessageToUser")]
    public async Task DirectMessage(string user, string message)
        => await Clients.User(user).SendAsync("ReceiveMessage", user, message);
    // </snippet_HubMethodName>

    // <snippet_ThrowException>
    public Task ThrowException()
        => throw new HubException("This error will be sent to the client!");
    // </snippet_ThrowException>
}
