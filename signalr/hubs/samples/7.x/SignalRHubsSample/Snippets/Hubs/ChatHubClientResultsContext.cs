using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRHubsSample.Hubs;
// <snippet_HubClientReturnContext>
public class ChatHubClientResultsContext : Hub
{
    async Task SomeMethod(IHubContext<ChatHubClientResultsContext> context)
    {
        string result = await context.Clients.Client(connectionID).InvokeAsync<string>(
            "GetMessage");
    }
    // </snippet_HubClientReturnContext>
}