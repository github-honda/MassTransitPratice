using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using MassTransit;
using Messaging1;

namespace Publisher1
{
    internal class Program
    {
        public static object RegisterNewOrderConsumer { get; private set; }
        static void Main(string[] args)
        {
            Console.WriteLine("CUSTOMER REGISTRATION COMMAND PUBLISHER");
            Console.Title = "Publisher window"; RunMassTransitPublisherWithRabbit();
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

            string sAddress = string.Concat(rabbitMqAddress, "/", rabbitMqQueue);
            Console.WriteLine($"Connecting {sAddress}");

            Task<ISendEndpoint> sendEndpointTask = rabbitBusControl.GetSendEndpoint(new Uri(sAddress));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            DateTime TempTime = DateTime.UtcNow;
            Task sendTask = sendEndpoint.Send<IRegisterCustomer>(new
            {
                Address = "New Street",
                Id = Guid.NewGuid(),
                Preferred = true,
                //RegisteredUtc = DateTime.UtcNow,
                RegisteredUtc = TempTime,
                Name = "Nice people LTD",
                Type = 1,
                DefaultDiscount = 0
            });
            Console.WriteLine($"Send message on {TempTime.ToString("O")}");
            Console.ReadKey();
        }
    }
}
