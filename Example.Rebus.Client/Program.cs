using Amazon.SQS;
using Example.Rebus.Contracts;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Rebus.Client
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
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
        }
    }


    internal sealed class ConsoleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IBus _bus;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IJob> _jobs;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IServiceProvider serviceProvider,
            IBus bus,
            IEnumerable<IJob> jobs)
        {
            _logger = logger;
            _bus = bus;
            _serviceProvider = serviceProvider;
            _jobs = jobs;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting Service");

            _serviceProvider.UseRebus();

            //schedule job to send request message
            //Note, if this endpoint is scaled out, each instance will execute this job
            ConfigureJobLogger();

            foreach(var job in _jobs)
            {
                JobManager.AddJob(
                (IJob)job,
                schedule =>
                {
                    schedule
                        .ToRunNow()
                        .AndEvery(1).Seconds();
                });
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            JobManager.Stop();

            return Task.CompletedTask;
        }

        private void ConfigureJobLogger()
        {
            JobManager.JobException += info =>
            {
                _logger.LogError($"Error occurred in job: {info.Name}", info.Exception);
            };
            JobManager.JobStart += info =>
            {
                _logger.LogDebug($"Start job: {info.Name}. Duration: {info.StartTime}");
            };
            JobManager.JobEnd += info =>
            {
                _logger.LogDebug($"End job: {info.Name}. Duration: {info.Duration}. NextRun: {info.NextRun}.");
            };
        }
    }
}
