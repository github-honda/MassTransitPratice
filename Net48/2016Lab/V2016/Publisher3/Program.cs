using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
//using RabbitMQ.Client.MessagePatterns; does not exist !
using RabbitMQ.Client.Events;

namespace Publisher3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunRpcQueue();
        }
        private static void RunRpcQueue()
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

            string sQueueName = "mycompany.queues.rpc";
            channel.QueueDeclare(sQueueName, true, false, false, null);
            SendRpcMessagesBackAndForth(channel);

            channel.Close();
            connection.Close();
        }

        private static void SendRpcMessagesBackAndForth(IModel channel)
        {
            // 1. Build (ReplyTo queue) and (correlationId) for consumer to response.
            // The name of the temporary queue will be randomly generated, e.g. “amq.gen-3tj4jtzMauwolYqc7CUj9g”.
            // The temporary queue will be available as long as the sender is running. After that it will be removed automatically.
            string rpcResponseQueue = channel.QueueDeclare().QueueName;
            string correlationId = Guid.NewGuid().ToString();
            string responseFromConsumer = null;
            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.ReplyTo = rpcResponseQueue;
            basicProperties.CorrelationId = correlationId;

            // 2. Publish message with (ReplyTo queue) and (correlationId).
            //Console.WriteLine("Enter your message and press Enter.");
            //string message = Console.ReadLine();
            string message = $"Message 1 from Publisher3 {DateTime.Now:o}";
            Console.WriteLine(message);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            string sRoutingKey = "mycompany.queues.rpc";
            string sDefaultExchange = ""; // The nameless default AMQP exchange.
            channel.BasicPublish(sDefaultExchange, sRoutingKey, basicProperties, messageBytes);

            // 3. Waiting response from (ReplyTo queue).
            EventingBasicConsumer rpcEventingBasicConsumer = new EventingBasicConsumer(channel);
            rpcEventingBasicConsumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                IBasicProperties props = basicDeliveryEventArgs.BasicProperties;

                // correlation ID ensures that we’re communicating with the right consumer.
                if (props != null && props.CorrelationId == correlationId)
                {
                    string response = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                    responseFromConsumer = response;
                }
                bool bMultiple = false;
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, bMultiple);
                Console.WriteLine("Consumer response: {0}", responseFromConsumer);

                // 4. Re-publish message with (ReplyTo queue) and (correlationId).
                //Console.WriteLine("Enter your message and press Enter.");
                //message = Console.ReadLine();
                message = $"Re-publishing message 2 from Publisher3 {DateTime.Now:o}";
                messageBytes = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(sDefaultExchange, sRoutingKey, basicProperties, messageBytes);

                // 5. Repeat 3 and 4.
            };

            bool bAutoAct = false;
            channel.BasicConsume(rpcResponseQueue, bAutoAct, rpcEventingBasicConsumer);
        }

    }
}
