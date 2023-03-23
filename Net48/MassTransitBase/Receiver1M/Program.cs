using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using MassTransit;


namespace Receiver1M
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Management consumer";
            Console.WriteLine("MANAGEMENT");
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

            //    rabbit.ReceiveEndpoint(       , "mycompany.domains.queues.events.mgmt", conf =>
            //    {
            //        conf.Consumer<CustomerRegisteredConsumerMgmt>();
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

                rabbit.ReceiveEndpoint("mycompany.domains.queues.events.mgmt", conf =>
                {
                    conf.Consumer<CustomerRegisteredConsumerMgmt>();
                });
            });
            Console.WriteLine($"Connecting {Uri1}");
            rabbitBusControl.Start();
            Console.ReadKey();
            rabbitBusControl.Stop();
        }
    }
}
