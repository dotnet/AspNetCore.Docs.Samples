using TimedBackgroundTasks;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TimedHostedService>();
    })
    .Build();

host.Run();
