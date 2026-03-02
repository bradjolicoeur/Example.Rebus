using Amazon.SQS;
using Example.Rebus.Client;
using Example.Rebus.Contracts;
using FluentScheduler;
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
        services.AutoRegisterHandlersFromAssemblyOf<Program>();

        //Configure Rebus
        services.AddRebus(configure => configure
            .Logging(l => l.ColoredConsole())
            .Transport(t => t.UseAmazonSQSAsOneWayClient(SqsConfig)) //this is a send only endpoint
            .Routing(r => r.TypeBased().MapAssemblyOf<ImportantMessage>("ServerMessages")));

        //Jobs
        services.AddSingleton<IJob, ProduceMessageJob>();
    })
    .RunConsoleAsync();
