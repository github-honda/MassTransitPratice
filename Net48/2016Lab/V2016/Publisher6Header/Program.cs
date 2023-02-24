using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
//using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace Publisher6Header
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetUpHeadersExchange();
        }
        private static void SetUpHeadersExchange()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.ExchangeDeclare("company.exchange.headers", ExchangeType.Headers, true, false, null);
            channel.QueueDeclare("company.queue.headers", true, false, false, null);
            Dictionary<string, object> headerOptionsWithAll = new Dictionary<string, object>();
            headerOptionsWithAll.Add("x-match", "all");
            headerOptionsWithAll.Add("category", "animal");
            headerOptionsWithAll.Add("type", "mammal");

            channel.QueueBind("company.queue.headers", "company.exchange.headers", "", headerOptionsWithAll);

            Dictionary<string, object> headerOptionsWithAny = new Dictionary<string, object>();
            headerOptionsWithAny.Add("x-match", "any");
            headerOptionsWithAny.Add("category", "plant");
            headerOptionsWithAny.Add("type", "tree");

            channel.QueueBind("company.queue.headers", "company.exchange.headers", "", headerOptionsWithAny);

            IBasicProperties properties = channel.CreateBasicProperties();
            Dictionary<string, object> messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            messageHeaders.Add("type", "insect");
            properties.Headers = messageHeaders;
            PublicationAddress address = new PublicationAddress(ExchangeType.Headers, "company.exchange.headers", "");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of insects"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            messageHeaders.Add("type", "mammal");
            messageHeaders.Add("mood", "awesome");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of awesome mammals"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            messageHeaders.Add("type", "mammal");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of mammals"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of animals"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "fungi");
            messageHeaders.Add("type", "champignon");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of fungi"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "plant");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of plants"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "plant");
            messageHeaders.Add("type", "tree");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of trees"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("mood", "sad");
            messageHeaders.Add("type", "tree");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of sad trees"));

            channel.Close();
            connection.Close();
        }
    }
}
