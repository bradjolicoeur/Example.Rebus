using Example.Rebus.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.ServiceProvider;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Rebus.Server
{
    internal sealed class ConsoleHostedService : IHostedService
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

        //Send message to self...just to see how this works
        _bus.SendLocal(new ImportantMessage());

        return Task.CompletedTask;
    }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
