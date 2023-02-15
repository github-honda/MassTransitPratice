// By template:
//namespace Company.Consumers
//{
//    using System.Threading.Tasks;
//    using MassTransit;
//    using Contracts;

//    public class GettingStartedConsumer :
//        IConsumer<GettingStarted>
//    {
//        public Task Consume(ConsumeContext<GettingStarted> context)
//        {
//            return Task.CompletedTask;
//        }
//    }
//}


// By https://masstransit.io/quick-starts/in-memory:
namespace GettingStarted.Consumers;

using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

public class GettingStartedConsumer :
    IConsumer<GettingStarted>
{
    readonly ILogger<GettingStartedConsumer> _logger;

    public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<GettingStarted> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message.Value);
        return Task.CompletedTask;
    }
}
