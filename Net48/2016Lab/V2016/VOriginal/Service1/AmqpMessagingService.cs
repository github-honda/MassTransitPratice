using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
