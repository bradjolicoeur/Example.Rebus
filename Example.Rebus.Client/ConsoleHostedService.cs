using FluentScheduler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Rebus.Client
{
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
