using Amazon.SQS;
using Example.Rebus.Contracts;
using Example.Rebus.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

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
    .RunConsoleAsync();
