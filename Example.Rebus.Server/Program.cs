using Amazon.SQS;
using Example.Rebus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Handlers;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;
using System;
using System.Threading;
using System.Threading.Tasks;

var SqsConfig = new AmazonSQSConfig { UseHttp = true, ServiceURL = "http://localhost:4566", }; //for localstack

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<ConsoleHostedService>();

        // Automatically register all handlers from the assembly of a given type...
        services.AutoRegisterHandlersFromAssemblyOf<HandleMessage>();

        //Configure Rebus
        services.AddRebus(configure => configure
            .Logging(l => l.ColoredConsole())
            .Transport(t => t.UseAmazonSQS("ServerMessages", SqsConfig))
            );
    })
    .RunConsoleAsync()
    ;

file sealed class ConsoleHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IServiceProvider serviceProvider,
        IBus bus)
    {
        _logger = logger;
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Starting service");

        //Activate Rebus
        _serviceProvider.UseRebus();

        //Send message to self...just to see how this works
        _bus.SendLocal(new ImportantMessage());

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

file sealed class HandleMessage : IHandleMessages<ImportantMessage>
{
    private readonly ILogger<HandleMessage> _log;

    public HandleMessage(ILogger<HandleMessage> log)
    {
        _log = log;
    }

    public Task Handle(ImportantMessage message)
    {
        _log.LogInformation("Handled Important Message");

        return Task.CompletedTask;
    }
}
