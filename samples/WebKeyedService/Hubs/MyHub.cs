using Microsoft.AspNetCore.SignalR;

namespace WebKeyedService.Hubs;

// <snippet_1>
public class MyHub : Hub
{
    public void Method([FromKeyedServices("small")] ICache cache)
    {
        Console.WriteLine(cache.Get("signalr"));
    }
}
// </snippet_1>
