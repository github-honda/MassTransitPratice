using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;

namespace Publisher2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyOpenChannelFanoutExchange();

            Console.WriteLine("Main done...");
            Console.ReadKey();
        }
        static void MyOpenChannelFanoutExchange()
        {
            // ref: https://dotnetcodr.com/2016/08/15/messaging-with-rabbitmq-and-net-review-part-6-the-fanout-exchange-type/

            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();

            // ExchangeType.Direct: Bind only one queue to exchange. 
            // ExchangeType.Fanout: Bind different queues to the same exchange.
            string sExchangeName = "mycompany.fanout.exchange";
            string sQueueAccounting = "mycompany.queues.accounting";
            string sQueueManagement = "mycompany.queues.management";
            channel.ExchangeDeclare(sExchangeName, ExchangeType.Fanout, true, false, null);
            channel.QueueDeclare(sQueueAccounting, true, false, false, null);
            channel.QueueDeclare(sQueueManagement, true, false, false, null);
            channel.QueueBind(sQueueAccounting, sExchangeName, "");
            channel.QueueBind(sQueueManagement, sExchangeName, "");

            //IBasicProperties properties = channel.CreateBasicProperties();
            //properties.Persistent = true;
            //properties.ContentType = "text/plain";
            //PublicationAddress address = new PublicationAddress(ExchangeType.Fanout, sExchangeName, "");
            //channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("A new huge order has just come in worth $1M!!!!!"));
            MyPublishingFanout(channel, sExchangeName);

            channel.Close();
            connection.Close();
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));

        }

        static void MyPublishingFanout(IModel channel, string sExchangeName)
        {
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "text/plain";
            string sMsg = $"Message from Publisher2  {DateTime.Now:o}";
            PublicationAddress address = new PublicationAddress(ExchangeType.Fanout, sExchangeName, "");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes(sMsg));
            Console.WriteLine($"Publishing: {sMsg}");
        }

    }
}
