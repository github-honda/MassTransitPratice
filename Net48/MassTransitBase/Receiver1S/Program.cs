using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using MassTransit;

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
            //IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            //{
            //    IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
            //    {
            //        settings.Password("accountant");
            //        settings.Username("accountant");
            //    });

            //    rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.domains.queues.events.sales", conf =>
            //    {
            //        conf.Consumer<CustomerRegisteredConsumerSls>();
            //    });
            //});
            Uri Uri1 = new Uri("rabbitmq://localhost:5672/accounting");
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(Uri1, settings =>
                {
                    settings.Password("accountant");
                    settings.Username("accountant");
                });

                rabbit.ReceiveEndpoint("mycompany.domains.queues.events.sales", conf =>
                {
                    conf.Consumer<CustomerRegisteredConsumerSls>();
                });
            });
            Console.WriteLine($"Connecting {Uri1}");
            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
