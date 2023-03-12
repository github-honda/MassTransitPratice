using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Service1
{
    public class AmqpMessagingService
    {
        private string _hostName = "localhost";
        private string _userName = "guest";
        private string _password = "guest";
        private string _exchangeName = "";
        private string _oneWayMessageQueueName = "OneWayMessageQueue";
        private bool _durable = true;

        // worker demo
        private string _workerQueueDemoQueueName = "WorkerQueueDemoQueue";

        // publish/Subscribe demo
        private string _publishSubscribeExchangeName = "PublishSubscribeExchange";
        private string _publishSubscribeQueueOne = "PublishSubscribeQueueOne";
        private string _publishSubscribeQueueTwo = "PublishSubscribeQueueTwo";

        // rpc
        private string _rpcQueueName = "RpcQueue";
        //private QueueingBasicConsumer _rpcConsumer;
        private EventingBasicConsumer _rpcConsumer;
        private string _responseQueue;

        public IConnection GetRabbitMqConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = _hostName;
            connectionFactory.UserName = _userName;
            connectionFactory.Password = _password;

            return connectionFactory.CreateConnection();
        }
        public void SetUpQueueForOneWayMessageDemo(IModel model)
        {
            model.QueueDeclare(_oneWayMessageQueueName, _durable, false, false, null);
        }
        public void SetUpQueueForWorkerQueueDemo(IModel model)
        {
            model.QueueDeclare(_workerQueueDemoQueueName, _durable, false, false, null);
        }
        public void SetUpExchangeAndQueuesForDemo(IModel model)
        {
            model.ExchangeDeclare(_publishSubscribeExchangeName, ExchangeType.Fanout, true);
            model.QueueDeclare(_publishSubscribeQueueOne, true, false, false, null);
            model.QueueDeclare(_publishSubscribeQueueTwo, true, false, false, null);
            model.QueueBind(_publishSubscribeQueueOne, _publishSubscribeExchangeName, "");
            model.QueueBind(_publishSubscribeQueueTwo, _publishSubscribeExchangeName, "");
        }
        public void SetUpQueueForRpcDemo(IModel model)
        {
            model.QueueDeclare(_rpcQueueName, _durable, false, false, null);
        }

        public void SendOneWayMessage(string message, IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            //basicProperties.SetPersistent(_durable);
            basicProperties.Persistent = _durable;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_exchangeName, _oneWayMessageQueueName, basicProperties, messageBytes);
        }
        public void SendMessageToWorkerQueue(string message, IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            //basicProperties.SetPersistent(_durable);
            basicProperties.Persistent = _durable;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_exchangeName, _workerQueueDemoQueueName, basicProperties, messageBytes);
        }
        public void SendMessageToPublishSubscribeQueues(string message, IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            //basicProperties.SetPersistent(_durable);
            basicProperties.Persistent = _durable;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_publishSubscribeExchangeName, "", basicProperties, messageBytes);
        }

        // 20230312, 新版原始碼, 大多將 IModel model 命名為 channel.
        // public string SendRpcMessageToQueue(string message, IModel model, TimeSpan timeout)
        public string SendRpcMessageToQueue(string message, IModel channel, TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(_responseQueue))
            {
                _responseQueue = channel.QueueDeclare().QueueName;
            }

            if (_rpcConsumer == null)
            {
                //_rpcConsumer = new QueueingBasicConsumer(model);
                _rpcConsumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(_responseQueue, true, _rpcConsumer);
            }

            string correlationId = Guid.NewGuid().ToString();
            string responseFromConsumer = null;
            bool bResponse = false;

            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.ReplyTo = _responseQueue;
            basicProperties.CorrelationId = correlationId;

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", _rpcQueueName, basicProperties, messageBytes);


            //// 20230308, change to EventingBasicConsumer
            //DateTime timeoutDate = DateTime.UtcNow + timeout;
            //while (DateTime.UtcNow <= timeoutDate)
            //{
            //    BasicDeliverEventArgs deliveryArguments = (BasicDeliverEventArgs)_rpcConsumer.Queue.Dequeue();
            //    if (deliveryArguments.BasicProperties != null
            //    && deliveryArguments.BasicProperties.CorrelationId == correlationId)
            //    {
            //        string response = Encoding.UTF8.GetString(deliveryArguments.Body.ToArray());
            //        return response;
            //    }
            //}
            //throw new TimeoutException("No response before the timeout period.");

            _rpcConsumer.Received += (sender, basicDeliveryEventArgs) => 
            {
                IBasicProperties props = basicDeliveryEventArgs.BasicProperties;
                if (basicDeliveryEventArgs.BasicProperties != null
                && basicDeliveryEventArgs.BasicProperties.CorrelationId == correlationId)
                {
                    string response = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                    responseFromConsumer = response;
                    bResponse = true;
                }
                //bool bMultiple = false;
                //channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, bMultiple);
                ////Console.WriteLine("Consumer response: {0}", responseFromConsumer);
            };
            Thread.Sleep(timeout);
            if (bResponse)
                return responseFromConsumer;
            else
            {
                //bool bAutoAct = false;
                //channel.BasicConsume(_responseQueue, bAutoAct, _rpcConsumer);
                throw new TimeoutException("No response before the timeout period.");
            }
        }

        public void ReceiveOneWayMessages(IModel model)
        {
            // 20230303, Honda, Change QueueingBasicConsumer
            //model.BasicQos(0, 1, false); //basic quality of service
            //QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            //model.BasicConsume(_oneWayMessageQueueName, false, consumer);
            //while (true)
            //{
            //    BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
            //    String message = Encoding.UTF8.GetString(deliveryArguments.Body);
            //    Console.WriteLine("Message received: {0}", message);
            //    model.BasicAck(deliveryArguments.DeliveryTag, false);
            //}
            model.BasicQos(0, 1, false); //basic quality of service
            MyDefaultBasicConsumer consumer = new MyDefaultBasicConsumer(model);
            model.BasicConsume(_oneWayMessageQueueName, false, consumer);
        }
        public void ReceiveWorkerQueueMessages(IModel model)
        {
            // 20230303, Honda, Change QueueingBasicConsumer
            //model.BasicQos(0, 1, false); //basic quality of service
            //QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            //model.BasicConsume(_workerQueueDemoQueueName, false, consumer);
            //while (true)
            //{
            //    BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
            //    String message = Encoding.UTF8.GetString(deliveryArguments.Body);
            //    Console.WriteLine("Message received: {0}", message);
            //    model.BasicAck(deliveryArguments.DeliveryTag, false);
            //}
            model.BasicQos(0, 1, false); //basic quality of service
            MyDefaultBasicConsumer consumer = new MyDefaultBasicConsumer(model);
            model.BasicConsume(_workerQueueDemoQueueName, false, consumer);
        }
        public void ReceivePublishSubscribeMessageReceiverOne(IModel model)
        {
            // 20230304, Honda, Change Subscription
            //model.BasicQos(0, 1, false);
            //Subscription subscription = new Subscription(model, _publishSubscribeQueueOne, false);
            //while (true)
            //{
            //    BasicDeliverEventArgs deliveryArguments = subscription.Next();
            //    String message = Encoding.UTF8.GetString(deliveryArguments.Body);
            //    Console.WriteLine("Message from queue: {0}", message);
            //    subscription.Ack(deliveryArguments);
            //}
            model.BasicQos(0, 1, false);
            MyDefaultBasicConsumer consumer = new MyDefaultBasicConsumer(model);
            model.BasicConsume(_publishSubscribeQueueOne, false, consumer);
        }

        public void ReceivePublishSubscribeMessageReceiverTwo(IModel model)
        {
            // 20230304, Honda, Change Subscription
            //model.BasicQos(0, 1, false);
            //Subscription subscription = new Subscription(model, _publishSubscribeQueueTwo, false);
            //while (true)
            //{
            //    BasicDeliverEventArgs deliveryArguments = subscription.Next();
            //    String message = Encoding.UTF8.GetString(deliveryArguments.Body);
            //    Console.WriteLine("Message from queue: {0}", message);
            //    subscription.Ack(deliveryArguments);
            //}
            model.BasicQos(0, 1, false);
            MyDefaultBasicConsumer consumer = new MyDefaultBasicConsumer(model);
            model.BasicConsume(_publishSubscribeQueueTwo, false, consumer);
        }
        public void ReceiveRpcMessage(IModel model)
        {
            // 20230312, 舊寫法為 loop, 新寫法改用 EventingBasicConsumer
            //model.BasicQos(0, 1, false);
            //QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            //model.BasicConsume(_rpcQueueName, false, consumer);

            //while (true)
            //{
            //    BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
            //    string message = Encoding.UTF8.GetString(deliveryArguments.Body);
            //    Console.WriteLine("Message: {0} ; {1}", message, " Enter your response: ");
            //    string response = Console.ReadLine();
            //    IBasicProperties replyBasicProperties = model.CreateBasicProperties();
            //    replyBasicProperties.CorrelationId = deliveryArguments.BasicProperties.CorrelationId;
            //    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            //    model.BasicPublish("", deliveryArguments.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);
            //    model.BasicAck(deliveryArguments.DeliveryTag, false);
            //}
            model.BasicQos(0, 1, false);
            EventingBasicConsumer consumer = new EventingBasicConsumer(model);
            model.BasicConsume(_rpcQueueName, false, consumer);
            consumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                IBasicProperties props = basicDeliveryEventArgs.BasicProperties;
                string message = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());
                Console.WriteLine("Message: {0} ; {1}", message, " Enter your response: ");
                string response = Console.ReadLine();

                IBasicProperties replyBasicProperties = model.CreateBasicProperties();
                replyBasicProperties.CorrelationId = props.CorrelationId;
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                model.BasicPublish("", props.ReplyTo, replyBasicProperties, responseBytes);
                model.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);
            };
        }
    }
}
