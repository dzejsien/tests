using System;
using System.Threading.Tasks;
using MassTransit;
using Serilog;

namespace MassTransitSagas
{
    public class CalculateCommandConsumer : IConsumer<CalculateCommand>
    {
        private static readonly ILogger _log = Log.Logger.ForContext<CalculateCommandConsumer>();

        public async Task Consume(ConsumeContext<CalculateCommand> context)
        {
            _log.Information($"Consume CalculateCommandConsumer: message = {context.Message}");

            var response = new DoneEvent
            {
                Id = context.Message.Id,
                CorrelationId = context.CorrelationId
            };

            await context.Send(new Uri(new Uri("//guest:guest@localhost/"), "ForTesting.SagaQueue"), response).ConfigureAwait(false);
        }
    }
}