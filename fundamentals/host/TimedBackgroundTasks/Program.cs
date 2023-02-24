using TimedBackgroundTasks;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<TimedHostedService>();

IHost host = builder.Build();
host.Run();
