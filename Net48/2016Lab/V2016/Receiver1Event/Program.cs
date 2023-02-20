using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver1Event
{
    internal class Program
    {
        static IModel channelForEventing;
        static void Main(string[] args)
        {
            ReceiveMessagesWithEvents();

            Console.WriteLine("Main done...");
            Console.ReadKey();

        }
        private static void ReceiveMessagesWithEvents()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            channelForEventing = connection.CreateModel();
            channelForEventing.BasicQos(0, 1, false);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channelForEventing);

            eventingBasicConsumer.Received += EventingBasicConsumer_Received;
            // lambda expression based on a anonymous event handler same as above line:
            //eventingBasicConsumer.Received += (sender, e) =>
            //{
            //    IBasicProperties basicProperties = e.BasicProperties;
            //    Debug.WriteLine("Message received by the event based consumer. Check the debug window for details.");
            //    Debug.WriteLine(string.Concat("Message received from the exchange ", e.Exchange));
            //    Debug.WriteLine(string.Concat("Content type: ", basicProperties.ContentType));
            //    Debug.WriteLine(string.Concat("Consumer tag: ", e.ConsumerTag));
            //    Debug.WriteLine(string.Concat("Delivery tag: ", e.DeliveryTag));
            //    Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(e.Body.ToArray())));
            //    Debug.WriteLine(string.Concat("redelivered: ", e.Redelivered)); // Should be true if the message has been viewed in the RabbmitMQ management GUI before.
            //    channelForEventing.BasicAck(e.DeliveryTag, false);
            //};
            /*
sample output:             
Message received from the exchange my.first.exchange
Content type: text/plain
Consumer tag: amq.ctag-mVsEca6C9pBTjPci-kuj-A
Delivery tag: 1
The thread 0xdde8 has exited with code 0 (0x0).
Message: Message from RabbitMQClient1 2023-02-20T13:47:39.8159446+08:00
redelivered: False
             */

            channelForEventing.BasicConsume("my.first.queue", false, eventingBasicConsumer);
        }

        private static void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            /*
sample output:             
Message received from the exchange my.first.exchange
Content type: text/plain
Consumer tag: amq.ctag-mVsEca6C9pBTjPci-kuj-A
Delivery tag: 1
The thread 0xdde8 has exited with code 0 (0x0).
Message: Message from RabbitMQClient1 2023-02-20T13:47:39.8159446+08:00
redelivered: False
             */

            IBasicProperties basicProperties = e.BasicProperties; ;
            Debug.WriteLine("Message received by the event based consumer. Check the debug window for details.");
            Debug.WriteLine(string.Concat("Message received from the exchange ", e.Exchange));
            Debug.WriteLine(string.Concat("Content type: ", basicProperties.ContentType));
            Debug.WriteLine(string.Concat("Consumer tag: ", e.ConsumerTag));
            Debug.WriteLine(string.Concat("Delivery tag: ", e.DeliveryTag));
            Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(e.Body.ToArray())));
            Debug.WriteLine(string.Concat("redelivered: ", e.Redelivered)); // Should be true if the message has been viewed in the RabbmitMQ management GUI before.
            channelForEventing.BasicAck(e.DeliveryTag, false);
        }
    }
}
