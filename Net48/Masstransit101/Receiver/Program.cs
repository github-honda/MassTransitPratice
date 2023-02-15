using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using MassTransit;
using MassTransit.RabbitMqTransport;
using Domains;
using RepositoryDummy;
using StructureMap;
using System.Security.Cryptography.X509Certificates;

namespace Receiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Receiver";
            Console.WriteLine("Receiver");

            var container = new StructureMap.Container(conf =>
            {
                conf.For<ICustomerRepository>().Use<CustomerRepository>();
            });
            string whatDoIHave = container.WhatDoIHave();

            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                // 20230210, Honda, not support in MassTransit.RabbitMqTransport (8.0.12.0)
                //IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
                //{
                //    settings.Password("accountant");
                //    settings.Username("accountant");
                //});

                rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
                {
                    settings.Password("accountant");
                    settings.Username("accountant");
                });
                

                rabbit.ConnectBusObserver(new BusObserver());

                //rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.domains.queues", conf =>
                rabbit.ReceiveEndpoint("mycompany.domains.queues", conf =>
                {

                    conf.Consumer<RegisterCustomerConsumer>(container);
                    conf.Consumer<RegisterDomainConsumer>();
                    conf.UseRetry(Retry.Immediate(5));

                    //conf.UseRetry(Retry.Filter<Exception>(e => e.Message.IndexOf("We pretend that an exception was thrown") > -1).Immediate(5));
                    //conf.UseRetry(Retry.Exponential(5, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5)));
                    //conf.UseRetry(Retry.Intervals(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(4)));
                });

                //rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.queues.errors.newcustomers", conf =>
                rabbit.ReceiveEndpoint("mycompany.queues.errors.newcustomers", conf =>
                {
                    conf.Consumer<RegisterCustomerFaultConsumer>();
                });
            });

            rabbitBusControl.Start();
            //rabbitBusControl.ConnectReceiveObserver(new MessageReceiveObserver());
            //rabbitBusControl.ConnectConsumeObserver(new MessageConsumeObserver());
            //rabbitBusControl.ConnectConsumeMessageObserver(new RegisterCustomerMessageObserver());
            Console.ReadKey();

            rabbitBusControl.Stop();

            Console.ReadKey();

        }
    }
}
