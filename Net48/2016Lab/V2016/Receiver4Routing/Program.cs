using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver4Routing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReceiveMessagesWithEvents();
        }
        private static void ReceiveMessagesWithEvents()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            channel.BasicQos(0, 1, false);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);

            eventingBasicConsumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                // The same queue can be bound to an exchange with multiple routing keys. 
                // i.e., consumer may receive messages from a queue with different binds (exchanges and routing keys).
                IBasicProperties basicProperties = basicDeliveryEventArgs.BasicProperties;
                Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
                Debug.WriteLine(string.Concat("Message received from the exchange ", basicDeliveryEventArgs.Exchange));
                Debug.WriteLine(string.Concat("Routing key: ", basicDeliveryEventArgs.RoutingKey));
                Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray())));
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);
            };

            channel.BasicConsume("company.exchange.queue", false, eventingBasicConsumer);
            /*
                Output:
                Message received from the exchange company.exchange.routing
                Routing key: asia
                Message: The latest news from Asia!
                Message received from the exchange company.exchange.routing
                Routing key: europe
                Message: The latest news from Europe!
                Message received from the exchange company.exchange.routing
                Routing key: americas
                Message: The latest news from the Americas!             
             */
        }
    }
}
