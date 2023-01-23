using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;

namespace GettingStarted
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            // can't compiled !
                            // https://masstransit-project.com/quick-starts/rabbitmq.html
                            //cfg.Host("localhost", "/"), h =>
                            //{
                            //    h.Username("guest");
                            //    h.Password("guest");
                            //});
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    // can't compiled !
                    // https://github.com/MassTransit/Sample-GettingStarted
                    //services.AddMassTransitHostedService(true);
                    services.AddHostedService<Worker>();
                });
    }
}
