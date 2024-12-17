using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
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
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Consuming Message...");
                Console.WriteLine($"Message received from the exchange: {ea.Exchange}");
                Console.WriteLine($"Routing key: {ea.RoutingKey}");
                Console.WriteLine($"Message: {message}");

                // Acknowledge the message
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"Waiting for messages from queue: {queueName}");
        }
    }
}