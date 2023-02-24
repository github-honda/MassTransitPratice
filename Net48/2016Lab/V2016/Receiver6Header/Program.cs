using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver6Header
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
                IBasicProperties basicProperties = basicDeliveryEventArgs.BasicProperties;
                Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
                Debug.WriteLine(string.Concat("Message received from the exchange ", basicDeliveryEventArgs.Exchange));
                Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray())));
                StringBuilder headersBuilder = new StringBuilder();
                headersBuilder.Append("Headers: ").Append(Environment.NewLine);
                foreach (var kvp in basicProperties.Headers)
                {
                    headersBuilder.Append(kvp.Key).Append(": ").Append(Encoding.UTF8.GetString(kvp.Value as byte[])).Append(Environment.NewLine);
                }
                Debug.WriteLine(headersBuilder.ToString());
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);
            };

            channel.BasicConsume("company.queue.headers", false, eventingBasicConsumer);
        }
    }
}
