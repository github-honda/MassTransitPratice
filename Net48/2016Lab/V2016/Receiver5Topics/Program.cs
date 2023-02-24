using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver5Topics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReceiveMessagesWithEvents();
        }
        private static void ReceiveMessagesWithEvents()
        {
            // ref: https://dotnetcodr.com/2016/08/25/messaging-with-rabbitmq-and-net-review-part-8-routing-and-topics/

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
                IBasicProperties basicProperties = basicDeliveryEventArgs.BasicProperties;
                Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
                Debug.WriteLine(string.Concat("Message received from the exchange ", basicDeliveryEventArgs.Exchange));
                Debug.WriteLine(string.Concat("Routing key: ", basicDeliveryEventArgs.RoutingKey));
                Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray())));
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);
            };

            channel.BasicConsume("company.queue.topic", false, eventingBasicConsumer);
            /*
Output:
The consumer received the following 2 messages:
Message received from the exchange company.exchange.topic
Routing key: world.news.and.more
Message: It’s Friday night
Message received from the exchange company.exchange.topic
Routing key: beautiful.world
Message: The world is beautiful

The other 4 were discarded as their routing key patterns didn’t match any of the queues.

             */
        }
    }
}
