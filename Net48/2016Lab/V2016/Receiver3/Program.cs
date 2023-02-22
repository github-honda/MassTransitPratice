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

            // 1. Receive message including (ReplyTo queue) and (correlationId).
            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            uint PprefetchSize = 0;  // 最大訊息筆數. The maximum size of for the messages fetched from the queue. 0=no upper limit.
            ushort PprefetchCount = 1; // 每次接收訊息筆數. The number of messages to be fetched from queue at a time.
            bool Pglobal = false; // for the current channel only, not for the entire connection.
            channel.BasicQos(PprefetchSize, PprefetchCount, Pglobal);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                // 2. Acknowledge the message.
                string message = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                Console.WriteLine($"Message: {message}");
                bool bMultiple = false;
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, bMultiple);

                // 3. Publish response message with (correlationId) to (ReplyTo queue).
                // Console.WriteLine("Message: {0} {1}", message, " Enter your response: ");
                //string response = Console.ReadLine();
                string response = $"Response from Receiver3 {DateTime.Now:o}";
                Console.WriteLine(response);
                IBasicProperties replyBasicProperties = channel.CreateBasicProperties();
                replyBasicProperties.CorrelationId = basicDeliveryEventArgs.BasicProperties.CorrelationId;
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                string sDefaultExchange = ""; // The nameless default AMQP exchange.
                channel.BasicPublish(sDefaultExchange, 
                    basicDeliveryEventArgs.BasicProperties.ReplyTo, 
                    replyBasicProperties, 
                    responseBytes);
            };

            channel.BasicConsume("mycompany.queues.rpc", false, eventingBasicConsumer);
        }
    }
}
