using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using Service1;

namespace Consumer5B
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AmqpMessagingService messagingService = new AmqpMessagingService();
            IConnection connection = messagingService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();
            messagingService.ReceiveRoutingMessageReceiverTwo(model);
        }
    }
}
