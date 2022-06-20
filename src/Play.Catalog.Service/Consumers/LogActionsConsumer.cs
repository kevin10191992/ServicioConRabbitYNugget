using MassTransit;
using System.Text.Json;
using static Play.Catalog.Contract.Contracts;

namespace Play.Catalog.Service.Consumers
{
    public class LogActionsConsumer : IConsumer<LogActions>
    {

        public async Task Consume(ConsumeContext<LogActions> context)
        {
            LogActions message = context.Message;

            Console.WriteLine($"MensajeEntrante: {JsonSerializer.Serialize(message)}");

        }
    }
}
