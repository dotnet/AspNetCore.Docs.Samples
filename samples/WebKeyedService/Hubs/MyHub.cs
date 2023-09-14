using Microsoft.AspNetCore.SignalR;

namespace WebKeyedService.Hubs;


public class MyHub : Hub
{
    public void Method([FromKeyedServices("small")] ICache cache)
    {
        Console.WriteLine(cache.Get("signalr"));
    }
}