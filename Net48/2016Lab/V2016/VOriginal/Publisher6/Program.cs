﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;
using Service1;

namespace Publisher6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AmqpMessagingService messagingService = new AmqpMessagingService();
            IConnection connection = messagingService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();

            //messagingService.SetUpExchangeAndQueuesForTopicsDemo(model);
            RunTopicsDemo(model, messagingService);
        }

        private static void RunTopicsDemo(IModel model, AmqpMessagingService messagingService)
        {
            Console.WriteLine("Enter your message as follows: the routing key, followed by a semicolon, and then the message. Quit with 'q'.");
            while (true)
            {
                string fullEntry = Console.ReadLine();
                string[] parts = fullEntry.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string key = parts[0];
                string message = parts[1];
                if (message.ToLower() == "q") break;
                messagingService.SendTopicsMessage(message, key, model);
            }
        }
    }
}
