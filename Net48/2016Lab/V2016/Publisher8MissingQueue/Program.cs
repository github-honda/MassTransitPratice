using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Publisher8MissingQueue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetUpDirectExchange();
        }

        /*
        ref: https://dotnetcodr.com/2016/09/05/messaging-with-rabbitmq-and-net-review-part-11-various-other-topics/

        If a message cannot be forwarded to any queue from an exchange it’s lost by default. 
        In other words the publisher doesn’t know whether a message has been relayed to at least one queue. 
        There is a way around that via an overload of the BasicPublish method. 
        We also need to set up an event handler for the BasicReturn event of the IModel object. 
        The event handler is triggered in case there was no matching queue for the message. 
        
        The following example deliberately sets up an exchange with no queue:         
         */
        private static void SetUpDirectExchange()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.ExchangeDeclare("no.queue.exchange", ExchangeType.Direct, true, false, null);
            IBasicProperties properties = channel.CreateBasicProperties();
            channel.BasicReturn += Channel_BasicReturn;

            channel.BasicPublish("no.queue.exchange", "", true, properties, Encoding.UTF8.GetBytes("This is a message from the RabbitMq .NET driver"));

            channel.Close();
            connection.Close();
        }

        private static void Channel_BasicReturn(object sender, BasicReturnEventArgs e)
        {
            Debug.WriteLine(string.Concat("Queue is missing for the message: ", Encoding.UTF8.GetString(e.Body.ToArray())));
            Debug.WriteLine(string.Concat("Reply code and text: ", e.ReplyCode, " ", e.ReplyText));
        }
    }
}
