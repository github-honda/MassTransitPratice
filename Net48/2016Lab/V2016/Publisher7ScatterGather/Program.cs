using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
//using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace Publisher7ScatterGather
{
    internal class Program
    {
        static string _sExchange = "mycompany.exchanges.scattergather";
        static void Main(string[] args)
        {
            RunScatterGatherQueue();
            Console.ReadKey();
        }
        private static void RunScatterGatherQueue()
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

            // 1. The sender will set up a temporary response queue where the receivers can send their responses.
            // i.e., Bind these different temporary response queues into a same exchange.
            // i.e., The publisher can receive maximum 3 queue's responses at a same time.
            // 可同時連線接收最多3個 queue 回應訊息.
            string sQueuea = "mycompany.queues.scattergather.a";
            string sQueueb = "mycompany.queues.scattergather.b";
            string sQueuec = "mycompany.queues.scattergather.c";
            channel.QueueDeclare(sQueuea, true, false, false, null);
            channel.QueueDeclare(sQueueb, true, false, false, null);
            channel.QueueDeclare(sQueuec, true, false, false, null);

            // 本 ScatterGather 範例以 ExchangeType.Fanout 示範同時發出訊息給所有已綁定的 queue.
            channel.ExchangeDeclare(_sExchange, ExchangeType.Fanout, true, false, null);
            
            channel.QueueBind(sQueuea, _sExchange, "");
            channel.QueueBind(sQueueb, _sExchange, "");
            channel.QueueBind(sQueuec, _sExchange, "");
            int iMinimumResponses = 3;
            SendScatterGatherMessages(connection, channel, iMinimumResponses);
        }

        private static void SendScatterGatherMessages(IConnection connection, IModel channel, int minResponses)
        {
            List<string> responses = new List<string>();
            string rpcResponseQueue = channel.QueueDeclare().QueueName;
            string correlationId = Guid.NewGuid().ToString();

            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.ReplyTo = rpcResponseQueue;
            basicProperties.CorrelationId = correlationId;
            Console.WriteLine("Enter your message and press Enter.");

            // 2. Publish message to all 3 queues: sQueuea, sQueueb, sQueuec.
            string message = Console.ReadLine();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(_sExchange, "", basicProperties, messageBytes);

            // 3. Receive response from each queue of 3 queues: sQueuea, sQueueb, sQueuec.
            EventingBasicConsumer scatterGatherEventingBasicConsumer = new EventingBasicConsumer(channel);
            scatterGatherEventingBasicConsumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                IBasicProperties props = basicDeliveryEventArgs.BasicProperties;
                channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);

                // 4. Check CorrelationId for different consumer from response. 
                if (props != null
                    && props.CorrelationId == correlationId)
                {
                    string response = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                    Console.WriteLine("Response: {0}", response);

                    // 重點: 測試結果發現 responses List 會記住 3 個 queue 回應的(共 3筆訊息)!
                    // 這代表本函數始終保持執行在記憶體中, 持續等待(Event接收到3個訊息)
                    responses.Add(response);

                    // 5. Ending publisher service while match some conditions. 
                    if (responses.Count >= minResponses)
                    {
                        Console.WriteLine(string.Concat("Responses received from consumers: ", string.Join(Environment.NewLine, responses)));
                        channel.Close();
                        connection.Close();
                    }
                }
            };
            channel.BasicConsume(rpcResponseQueue, false, scatterGatherEventingBasicConsumer);
        }
    }
}
