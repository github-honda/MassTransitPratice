using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receiver7ScatterGather
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ref: https://dotnetcodr.com/2016/09/01/messaging-with-rabbitmq-and-net-review-part-10-scattergather/

            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            channel.BasicQos(0, 1, false);
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);

            // 接收訊息並回應訊息後就關閉 Connetion, 即可空出 queue 給其他人使用.
            // 因此測試方式為: 
            //   將本程式複製為3個, 分別改寫為在接收訊息後, 回應三個不同的訊息到 ReplyTo queue. 
            //   同時執行3個程式測試.
            string consumerId = "A";
            Console.WriteLine(string.Concat("Consumer ", consumerId, " up and running, waiting for the publisher to start the bidding process."));
            eventingBasicConsumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                string message = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);
                Console.WriteLine("Message: {0} {1}", message, " Enter your response: ");
                string response = string.Concat("Consumer ID: ", consumerId, ", bid: ", Console.ReadLine());
                IBasicProperties replyBasicProperties = channel.CreateBasicProperties();
                replyBasicProperties.CorrelationId = basicDeliveryEventArgs.BasicProperties.CorrelationId;
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                channel.BasicPublish("", basicDeliveryEventArgs.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);
                channel.Close();
                connection.Close();
            };
            channel.BasicConsume("mycompany.queues.scattergather.a", false, eventingBasicConsumer);
        }
    }
}
