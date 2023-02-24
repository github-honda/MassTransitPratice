using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
//using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace Publisher4Routing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetUpDirectExchangeWithRoutingKey();
        }
        private static void SetUpDirectExchangeWithRoutingKey()
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

            // The same queue can be bound to an exchange with multiple routing keys. 
            // i.e., consumer may receive messages from a queue with different binds (exchanges and routing keys).
            string sQueue = "company.exchange.queue";
            string sExchange = "company.exchange.routing";
            channel.ExchangeDeclare(sExchange, ExchangeType.Direct, true, false, null);
            channel.QueueDeclare(sQueue, true, false, false, null);
            channel.QueueBind(sQueue, sExchange, "asia");
            channel.QueueBind(sQueue, sExchange, "americas");
            channel.QueueBind(sQueue, sExchange, "europe");

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "text/plain";

            // Messages with routing Keys "asia", "europe" or "americas"
            // will be send to the exchange "company.exchange.routing"
            // and forwarded to queue "company.exchange.queue": 
            PublicationAddress address = new PublicationAddress(ExchangeType.Direct, sExchange, "asia");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from Asia!"));
            address = new PublicationAddress(ExchangeType.Direct, sExchange, "europe");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from Europe!"));
            address = new PublicationAddress(ExchangeType.Direct, sExchange, "americas");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from the Americas!"));

            // Messages will be discarded as there's no matching queue to which the could be forwarded:
            address = new PublicationAddress(ExchangeType.Direct, sExchange, "africa");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from Africa!"));
            address = new PublicationAddress(ExchangeType.Direct, sExchange, "australia");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from Australia!"));

            channel.Close();
            connection.Close();
            /*
                Output:
The consumer received the following 2 messages:
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
