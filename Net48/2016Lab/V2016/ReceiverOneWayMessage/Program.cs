using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;

namespace ReceiverOneWayMessage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyReceiveSingleOneWayMessage();

            Console.WriteLine("Main done...");
            Console.ReadKey();

        }
        static void MyReceiveSingleOneWayMessage()
        {
            // ref: https://dotnetcodr.com/2016/08/08/messaging-with-rabbitmq-and-net-review-part-4-one-way-messaging-with-a-basic-consumer/

            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.Port = 5672;
            connectionFactory.HostName = "localhost";
            connectionFactory.UserName = "accountant";
            connectionFactory.Password = "accountant";
            connectionFactory.VirtualHost = "accounting";

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();


            // prefetch limits parameters: PrefetchSize, PrefetchCount, Global.
            uint PprefetchSize = 0;  // 最大訊息筆數. The maximum size of for the messages fetched from the queue. 0=no upper limit.
            ushort PprefetchCount = 1; // 每次接收訊息筆數. The number of messages to be fetched from queue at a time.
            bool Pglobal = false; // for the current channel only, not for the entire connection.
            channel.BasicQos(PprefetchSize, PprefetchCount, Pglobal);
            
            DefaultBasicConsumer Consumer1 = new MyDefaultBasicConsumer(channel);
            string sQueueName = "my.first.queue";
            bool AutoAct = false; // No acknowledgement.
            channel.BasicConsume(sQueueName, AutoAct, Consumer1);
        }
    }
}
