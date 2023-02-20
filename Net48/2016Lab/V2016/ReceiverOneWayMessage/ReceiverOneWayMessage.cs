using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using RabbitMQ.Client;

namespace ReceiverOneWayMessage
{
    public class ReceiverOneWayMessage : DefaultBasicConsumer
    {
        readonly IModel _channel;
        public ReceiverOneWayMessage(IModel model) 
            : base(model)
        {
            _channel = model;
        }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            //base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);

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
            Console.WriteLine("Message received by the consumer. Check the debug window for details.");
            Debug.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Debug.WriteLine(string.Concat("Content type: ", properties.ContentType)); // MIME type
            Debug.WriteLine(string.Concat("Consumer tag: ", consumerTag)); // e.g., amq.ctag-qCDfYIYQEpGqvAY7t-bhCQ
            Debug.WriteLine(string.Concat("Delivery tag: ", deliveryTag)); // indicates the position of the message in the queue: 1 is the first message, 2 is the second message etc. according to FIFO. 
            //Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body)));
            Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body.ToArray())));
            Debug.WriteLine(string.Concat("redelivered: ", redelivered)); // Should be true if the message has been viewed in the RabbmitMQ management GUI before.

            // When RabbitMq has received the acknowledgement, i.e. deliverTag then the message is deleted from the queue. 
            bool bMultiple = false;
            _channel.BasicAck(deliveryTag, bMultiple);  
        }
    }
}
