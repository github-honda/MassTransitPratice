using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using Service1;

namespace Publisher1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AmqpMessagingService messagingService = new AmqpMessagingService();
            IConnection connection = messagingService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();

            // Open comment to test "OneWayMesssageQueue" queue has been set up in the RabbitMQ Console.
            // messagingService.SetUpQueueForOneWayMessageDemo(model);

            RunOneWayMessageDemo(model, messagingService);

            Console.ReadKey();
        }
        private static void RunOneWayMessageDemo(IModel model, AmqpMessagingService messagingService)
        {
            Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");
            while (true)
            {
                string message = Console.ReadLine();
                if (message.ToLower() == "q") break;

                messagingService.SendOneWayMessage(message, model);
            }
        }
    }
}
