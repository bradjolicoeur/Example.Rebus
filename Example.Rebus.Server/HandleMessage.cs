using Example.Rebus.Contracts;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System.Threading.Tasks;

namespace Example.Rebus.Server
{
    public class HandleMessage : IHandleMessages<ImportantMessage>
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
}
