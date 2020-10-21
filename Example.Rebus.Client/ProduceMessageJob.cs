using Example.Rebus.Contracts;
using FluentScheduler;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Rebus.Client
{
    public class ProduceMessageJob : IJob
    {
        private readonly ILogger<ProduceMessageJob> _logger;
        private readonly IBus _bus;

        public ProduceMessageJob(ILogger<ProduceMessageJob> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }
        public void Execute()
        {
            _logger.LogInformation("Executing ProduceMessageJob");
            _bus.Send(new ImportantMessage()).Wait();
        }
    }
}
