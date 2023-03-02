using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;

namespace RabbitMQClient1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyIsOpenConnection();
            SetUpDirectExchange();
            Console.WriteLine("Main done...");
            Console.ReadKey();
        }

        /// <summary>
        /// Test opening channel.
        /// </summary>
        static void SetUpDirectExchange()
        {
            // ref: https://dotnetcodr.com/2016/08/05/messaging-with-rabbitmq-and-net-review-part-3-the-net-client-and-some-initial-code/
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));

            /*
The queue name
Whether it is durable
Whether it is exclusive, which means whether the queue is exclusively used for the connection
Whether it should be auto-deleted
The same dictionary object with custom options as in ExchangeDeclare             * 
             */
            string sExchange = "my.first.exchange";
            string sQueueName = "my.first.queue";
            bool bDurable = true;
            bool bExclusive = false;
            bool bAutoDeleted = false;
            channel.ExchangeDeclare(sExchange, ExchangeType.Direct, true, false, null);
            channel.QueueDeclare(sQueueName, bDurable, bExclusive, bAutoDeleted, null);
            channel.QueueBind(sQueueName, sExchange, "");

            MyPublishing(channel, sExchange);

            channel.Close();
            connection.Close();
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));

            Console.WriteLine("Main done...");
            Console.ReadKey();
        }

        static void MyPublishing(IModel channel, string sExchangeName)
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "text/plain";
            string sMsg = $"Message from Publisher1 {DateTime.Now:o}";
            PublicationAddress address = new PublicationAddress(ExchangeType.Direct, sExchangeName, "");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes(sMsg));
            Console.WriteLine($"Publishing: {sMsg}");
        }

        /// <summary>
        /// Test opening channel at amqp://accountant:accountant@localhost:5672/accounting
        /// </summary>
        static void MyIsOpenConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("amqp://accountant:accountant@localhost:5672/accounting");
            IConnection connection = connectionFactory.CreateConnection();
            Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));
        }

    }
}
