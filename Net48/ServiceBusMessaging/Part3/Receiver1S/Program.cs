using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver1S
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sales consumer";
            Console.WriteLine("SALES");
            RunMassTransitReceiverWithRabbit();
        }

        private static void RunMassTransitReceiverWithRabbit()
        {
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
                {
                    settings.Password("accountant");
                    settings.Username("accountant");
                });

                rabbit.ReceiveEndpoint("mycompany.domains.queues.events.sales", conf =>
                {
                    conf.Consumer<CustomerRegisteredConsumerSls>();
                });
            });

            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
