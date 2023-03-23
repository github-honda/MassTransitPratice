using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
// add
using Messaging1;

namespace Publisher1
{
    internal class Program
    {
        public static object RegisterNewOrderConsumer { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("CUSTOMER REGISTRATION COMMAND PUBLISHER");
            Console.Title = "Publisher window";
            RunMassTransitPublisherWithRabbit();
        }

        private static void RunMassTransitPublisherWithRabbit()
        {
            string rabbitMqAddress = "rabbitmq://localhost:5672/accounting";
            string rabbitMqQueue = "mycompany.domains.queues";
            Uri rabbitMqRootUri = new Uri(rabbitMqAddress);

            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(rabbitMqRootUri, settings =>
                {
                    settings.Password("accountant");
                    settings.Username("accountant");
                });
            });

            Task<ISendEndpoint> sendEndpointTask = rabbitBusControl.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            Task sendTask = sendEndpoint.Send<IRegisterCustomer>(new
            {
                Address = "New Street",
                Id = Guid.NewGuid(),
                Preferred = true,
                RegisteredUtc = DateTime.UtcNow,
                Name = "Nice people LTD",
                Type = 1,
                DefaultDiscount = 0
            });
            Console.ReadKey();
        }
    }
}
