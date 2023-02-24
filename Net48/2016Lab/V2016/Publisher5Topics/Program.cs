using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
//using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace Publisher5Topics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetUpTopicsExchange();
        }
        private static void SetUpTopicsExchange()
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

            string sExchange = "company.exchange.topic";
            string sQueue = "company.queue.topic";
            channel.ExchangeDeclare(sExchange, ExchangeType.Topic, true, false, null);
            channel.QueueDeclare(sQueue, true, false, false, null);
            channel.QueueBind(sQueue, sExchange, "*.world");
            channel.QueueBind(sQueue, sExchange, "world.#");

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "text/plain";
            PublicationAddress address = new PublicationAddress(ExchangeType.Topic, sExchange, "news of the world");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("This is some random news from the world"));

            address = new PublicationAddress(ExchangeType.Topic, sExchange, "news.of.the.world");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("trololo"));

            address = new PublicationAddress(ExchangeType.Topic, sExchange, "the world is crumbling");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("whatever"));

            address = new PublicationAddress(ExchangeType.Topic, sExchange, "the.world.is.crumbling");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello"));

            address = new PublicationAddress(ExchangeType.Topic, sExchange, "world.news.and.more");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("It's Friday night"));

            address = new PublicationAddress(ExchangeType.Topic, sExchange, "world news and more");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("No more tears"));

            address = new PublicationAddress(ExchangeType.Topic, sExchange, "beautiful.world");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The world is beautiful"));

            channel.Close();
            connection.Close();
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
