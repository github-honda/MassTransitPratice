using System;

// add
using Domain1;
using Dummy1;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using StructureMap;
using MassTransit.RabbitMqTransport;

namespace Receiver1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "This is the customer registration command receiver.";
            Console.WriteLine("CUSTOMER REGISTRATION COMMAND RECEIVER.");
            RunMassTransitReceiverWithRabbit();
        }
        private static void RunMassTransitReceiverWithRabbit()
        {
            var container = new Container(conf =>
            {
                conf.For<ICustomerRepository>().Use<CustomerRepository>();
            });
            string whatDoIHave = container.WhatDoIHave();

            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
                {
                    settings.Password("accountant");
                    settings.Username("accountant");
                });

                rabbit.ReceiveEndpoint("mycompany.domains.queues", conf =>
                {
                    // How to fix ?
                    // Error	CS7069	
                    // Reference to type 'IReceiveEndpointConfigurator' claims it is defined in 'MassTransit', but it could not be found	
                    // ref: https://dotnetcodr.com/2016/09/22/messaging-through-a-service-bus-in-net-using-masstransit-part-4-dependency-injection-with-structuremap/
                    conf.Consumer<RegisterCustomerConsumer>(container);
                });
            });

            rabbitBusControl.Start();
            Console.ReadKey();

            rabbitBusControl.Stop();
        }


    }
}
