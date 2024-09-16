using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRHubsSample.Hubs;
// <snippet_HubClientResultsStronglyTyped>
public interface IClient
{
    Task<string> GetMessage();
}

public class ChatHubClientResultsStronglyTyped : Hub
{
    public class ChatHub : Hub<IClient>
    {
        public async Task<string> WaitForMessage(string connectionId)
        {
            string message = await Clients.Client(connectionId).GetMessage();
            return message;
        }
    }
}
// </snippet_HubClientResultsStronglyTyped>