using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using MassTransit;
using Messaging;


namespace Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Publisher");
            Console.Title = "Publisher";

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



            //rabbitBusControl.ConnectSendObserver(new SendObjectObserver());

            Task<ISendEndpoint> sendEndpointTask = rabbitBusControl.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            Task sendTask = sendEndpoint.Send<ICustomer>(new
            {
                Address = "New Street",
                Id = Guid.NewGuid(),
                Preferred = true,
                RegisteredUtc = DateTime.UtcNow,
                Name = "Nice people LTD",
                Type = 1,
                DefaultDiscount = 0,
                Target = "Customers",
                Importance = 1
            }, c =>
            {
                c.FaultAddress = new Uri("rabbitmq://localhost:5672/accounting/mycompany.queues.errors.newcustomers");
            });

            Console.ReadKey();

        }
    }
}
