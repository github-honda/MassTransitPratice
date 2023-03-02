using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Publisher8ConfirmSelect
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetUpDirectExchange();
        }

        /*
        ref: https://dotnetcodr.com/2016/09/05/messaging-with-rabbitmq-and-net-review-part-11-various-other-topics/

        The publisher can receive a confirmation from RabbitMq whether a message successfully reached the exchange. 
        Note the following three components in the example code:
        1. ConfirmSelects: it activates feedback mechanism for the publisher
        2. The BasicAcks event handler which is called in case the message broker has acknowledged the message from the publisher
        3. The BasicNacks event handler which is triggered in case RabbitMq for some reason could not acknowledge a message. 
           In this case you can re-send a message if it’s of critical importance.

        Unacknowledged messages
        A recurring line of code in our demos was that the message registered by the receiver had to be acknowledged:
          channel.BasicAck(deliveryTag, false);
        
        The reason we did that was that the “noAck” flag in the channel.BasicConsume function was set to false. 
        That configuration ensures that a message is not deleted from the queue until it has been acknowledged by the consumer:
          channel.BasicConsume("my.first.queue", false, basicConsumer);

        As soon as that publisher has acknowledged the message it is deleted from the queue. 
        However, what if there is no acknowledgement? 
        It can happen in at least two cases:
        1. If there’s an exception during the message processing then the receiver might want to force resending the message. 
           The message will then be requeued and redelivered
        2. If the consumer crashes after receiving the message but before sending the acknowledgement then either another consumer instance can receive it or the same instance after rebooting
        
        The “redelivered” flag will be true in both cases so you’ll have to consider it in your code. 
        If a message is delivered more than once then there might be something wrong with it.

        To actively “unacknowledge” a message use the BasicNack function:
          channel.BasicNack(deliveryTag, false, true);
        The first two parameters are the same as for BasicAck, the last one means whether the message should be requeued.

        If “noAck” in BasicConsume is set to true then the message will be deleted from the queue as soon as it’s been delivered.
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

            channel.ExchangeDeclare("my.first.exchange", ExchangeType.Direct, true, false, null);
            channel.QueueDeclare("my.first.queue", true, false, false, null);
            channel.QueueBind("my.first.queue", "my.first.exchange", "");
            channel.ConfirmSelect();
            channel.BasicAcks += Channel_BasicAcks;
            channel.BasicNacks += Channel_BasicNacks;

            IBasicProperties properties = channel.CreateBasicProperties();
            PublicationAddress address = new PublicationAddress(ExchangeType.Direct, "my.first.exchange", "");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("This is a message from the RabbitMq .NET driver"));

            channel.Close();
            connection.Close();
        }

        private static void Channel_BasicNacks(object sender, BasicNackEventArgs e)
        {
            Console.WriteLine(string.Concat("Message broker could not acknowledge message with tag: ", e.DeliveryTag));
        }

        private static void Channel_BasicAcks(object sender, BasicAckEventArgs e)
        {
            Console.WriteLine(string.Concat("Message broker has acknowledged message with tag: ", e.DeliveryTag));
        }
    }
}
