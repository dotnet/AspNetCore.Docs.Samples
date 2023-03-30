using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRHubsSample.Hubs;
// <snippet_HubClientReturn>
public class ChatHubClientReturn : Hub
{
    public async Task<string> WaitForMessage(string connectionId)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        try
        {
            var message = await Clients.Client(connectionId).InvokeAsync<string>(
                "GetMessage", cts.Token);
            return message;
        }
        catch (OperationCanceledException ex)
        {
            // timeout occurred
            return "Timed out waiting for client input";
        }
        catch (Exception ex)
        {
            return "Error waiting for client result";
        }
    }
    // </snippet_HubClientReturn>
}