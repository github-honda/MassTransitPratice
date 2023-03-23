using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using MassTransit;
//using MassTransit.RabbitMqTransport;
using StructureMap;
using Domains1;
using Dummy1;

namespace Receiver1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "This is the customer registration command receiver.";
            Console.WriteLine("CUSTOMER REGISTRATION COMMAND RECEIVER.");
            RunMassTransitReceiverWithRabbit();
        }
        private static void RunMassTransitReceiverWithRabbit()
        {
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                //IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
                //{
                //    settings.Password("accountant");
                //    settings.Username("accountant");
                //});
                //rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.domains.queues", conf =>
                //{
                //    conf.Consumer<RegisterCustomerConsumer>();
                //});


                /*
                Note the URI format in the Host method. 
                It’s different from what we had when we worked directly with RabbitMq: amqp://accountant:accountant@localhost:5672/accounting. 
                The MassTransit URI doesn’t allow setting the username and password directly in the connection string. 
                We can only set the domain name and the virtual host. 
                The virtual host is “accounting” in the above example which we created in the previous series on RabbitMq.

                We can provide the queue name in the ReceiveEndpoint function. 
                In this case it is “mycompany.domains.queues”. 

                Note how the RegisterCustomerConsumer is registered using the Consumer function.
                Finally we start the service bus and don’t let it stop until the user presses a button.
                 */

                var container = new Container(conf =>
                {
                    conf.For<ICustomerRepository>().Use<CustomerRepository>();
                });
                string whatDoIHave = container.WhatDoIHave();

                // different from RabbitMq: amqp://accountant:accountant@localhost:5672/accounting.
                string sAddress = "rabbitmq://localhost:5672/accounting"; // virtual host = “accounting”
                Console.WriteLine($"Connecting {sAddress}");
                rabbit.Host(new Uri(sAddress), settings =>
                {
                    settings.Password("accountant");
                    settings.Username("accountant");
                });

                // queue name = "mycompany.domains.queues"
                rabbit.ReceiveEndpoint("mycompany.domains.queues", conf => 
                {
                    //conf.Consumer<RegisterCustomerConsumer>();
                    //conf.Consumer<RegisterCustomerConsumer>(container);
                    conf.Consumer<RegisterCustomerConsumer>();
                });
            });

            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }
    }
}
