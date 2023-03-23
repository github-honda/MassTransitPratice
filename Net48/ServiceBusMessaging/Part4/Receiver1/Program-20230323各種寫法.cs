//using System;

//// add
//using Microsoft.Extensions.DependencyInjection;
//using MassTransit;
//using MassTransit.StructureMapIntegration;
//using StructureMap;
//using Domain1;
//using Dummy1;
//using MassTransit.Context;

using System;

// add
//using MassTransit;
//using MassTransit.StructureMapIntegration;
//using StructureMap;
using Domain1;
using Dummy1;

//using System;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
//using MassTransit.StructureMap;
using StructureMap;

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


        // 20230322, not support above v 8.0.0.
        //private static void RunMassTransitReceiverWithRabbit()
        //{
        //var container = new Container(conf =>
        //{
        //    conf.For<ICustomerRepository>().Use<CustomerRepository>();
        //});
        //string whatDoIHave = container.WhatDoIHave();

        //IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
        //{
        //    rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
        //    {
        //        settings.Password("accountant");
        //        settings.Username("accountant");
        //    });

        //    rabbit.ReceiveEndpoint("mycompany.domains.queues", conf =>
        //    {
        //        conf.Consumer<RegisterCustomerConsumer>(container);
        //    });
        //});

        //rabbitBusControl.Start();
        //Console.ReadKey();

        //rabbitBusControl.Stop();
        //}

        // 20230322, BuildServiceProvider() 是 IServiceCollection 的方法，而不是 StructureMap 的方法。
        //private static void RunMassTransitReceiverWithRabbit()
        //{
        //    var services = new ServiceCollection();
        //    services.AddScoped<ICustomerRepository, CustomerRepository>();

        //    IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
        //    {
        //        rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
        //        {
        //            settings.Password("accountant");
        //            settings.Username("accountant");
        //        });

        //        rabbit.ReceiveEndpoint("mycompany.domains.queues", conf =>
        //        {
        //            conf.ConfigureConsumer<RegisterCustomerConsumer>(services.BuildServiceProvider());
        //        });
        //    });

        //    rabbitBusControl.Start();
        //    Console.ReadKey();

        //    rabbitBusControl.Stop();
        //}

        //private static void RunMassTransitReceiverWithRabbit()
        //{
        //    var container = new Container(conf =>
        //    {
        //        conf.For<ICustomerRepository>().Use<CustomerRepository>();
        //    });
        //    string whatDoIHave = container.WhatDoIHave();



        //    IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
        //    {
        //        rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
        //        {
        //            settings.Password("accountant");
        //            settings.Username("accountant");
        //        });

        //        rabbit.ReceiveEndpoint("mycompany.domains.queues", conf =>
        //        {
        //            conf.ConfigureConsumer<RegisterCustomerConsumer>(container);
        //        });
        //    });

        //    rabbitBusControl.Start();
        //    Console.ReadKey();

        //    rabbitBusControl.Stop();
        //}

        // 20230322, 只能用在 .NET Core
        //private static void RunMassTransitReceiverWithRabbit()
        //{
        //    var container = new Container(config =>
        //    {
        //        config.For<ICustomerRepository>().Use<CustomerRepository>();
        //    });

        //    var registry = new ConfigurationExpression();
        //    registry.For<ICustomerRepository>().Use<CustomerRepository>();

        //    var containerWithRegistry = new Container(registry);
        //    var provider = new StructureMapServiceProvider(containerWithRegistry);

        //    IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
        //    {
        //        rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
        //        {
        //            settings.Password("accountant");
        //            settings.Username("accountant");
        //        });

        //        rabbit.ReceiveEndpoint("mycompany.domains.queues", conf =>
        //        {
        //            conf.ConfigureConsumer<RegisterCustomerConsumer>(provider);
        //        });
        //    });

        //    rabbitBusControl.Start();
        //    Console.ReadKey();

        //    rabbitBusControl.Stop();
        //}


        private static void RunMassTransitReceiverWithRabbit()
        {
            Container container = new Container();
            container.Configure(conf =>
            {
                conf.For<ICustomerRepository>().Use<CustomerRepository>();
            });
            string whatDoIHave = container.WhatDoIHave();

            var serviceProvider = new StructureMapServiceProvider(container);

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost:5672/accounting"), h =>
                {
                    h.Username("accountant");
                    h.Password("accountant");
                });

                cfg.ReceiveEndpoint("mycompany.domains.queues", conf1 =>
                {
                    //conf1.ConfigureConsumer<RegisterCustomerConsumer>(serviceProvider.);
                    //conf1.ConfigureConsumer<RegisterCustomerConsumer>((IRegistrationContext)container);
                    //conf1.Consumer<RegisterCustomerConsumer>((IRegistrationContext)container);
                    //conf1.ConfigureConsumer<RegisterCustomerConsumer>(container);
                    conf1.Consumer<RegisterCustomerConsumer>(container);
                });
            });

            busControl.Start();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            busControl.Stop();
        }


    }
}
