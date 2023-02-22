using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace Receiver3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ref: https://dotnetcodr.com/2016/08/18/messaging-with-rabbitmq-and-net-review-part-7-two-way-messaging/
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
                string message = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                bool bMultiple = false;
                // acknowledge the message.
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, bMultiple);
                
                // Console.WriteLine("Message: {0} {1}", message, " Enter your response: ");
                //string response = Console.ReadLine();
                string response = $"Message from Receiver3 {DateTime.Now:o}";
                Console.WriteLine($"Message: {message}");
                Console.WriteLine($"Response: {response}");
                IBasicProperties replyBasicProperties = channel.CreateBasicProperties();

                // reply the same correlationId from this delivery.
                replyBasicProperties.CorrelationId = basicDeliveryEventArgs.BasicProperties.CorrelationId;
                
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                string sDefaultExchange = ""; // The nameless default AMQP exchange.
                
                // publish a response.
                channel.BasicPublish(sDefaultExchange, 
                    basicDeliveryEventArgs.BasicProperties.ReplyTo, 
                    replyBasicProperties, 
                    responseBytes);
            };

            channel.BasicConsume("mycompany.queues.rpc", false, eventingBasicConsumer);
        }
    }
}
