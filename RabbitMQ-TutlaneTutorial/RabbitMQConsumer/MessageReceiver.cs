
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer
{
    public class MessageReceiver
    {
        private readonly IChannel _channel;

        public MessageReceiver(IChannel channel)
        {
            _channel = channel;
        }

        public async Task StartConsumingAsync(string queueName)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                Console.WriteLine($"Consuming Message");
                Console.WriteLine(string.Concat("Message received from the exchange ", ea.Exchange));
                Console.WriteLine(string.Concat("Consumer tag: ", ea.ConsumerTag));
                Console.WriteLine(string.Concat("Delivery tag: ", ea.DeliveryTag));
                Console.WriteLine(string.Concat("Routing tag: ", ea.RoutingKey));
                Console.WriteLine(string.Concat("Message: ", ea.Body));

                // Acknowledge the message
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"Waiting for messages from queue: {queueName}");
        }
    }
}
