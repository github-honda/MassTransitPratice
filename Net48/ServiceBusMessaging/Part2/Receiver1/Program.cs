using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receiver1
{
    public class Program
    {
        static void Main(string[] args)
        {
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

            //    rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.domains.queues", conf =>
            //    {
            //        conf.Consumer<RegisterCustomerConsumer>();
            //    });
            //});
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
                {
                    settings.Password("accountant");
                    settings.Username("accountant");
                });

                rabbit.ReceiveEndpoint("mycompany.domains.queues", conf =>
                {
                    conf.Consumer<RegisterCustomerConsumer>();
                });
            });

            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
