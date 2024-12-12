using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null); // queue will not survive a RabbitMQ node restart
// await channel.QueueDeclareAsync(queue: "hello", durable: true, exclusive: false, autoDelete: false, arguments: null); // will servive...

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");

    int dots = message.Split('.').Length - 1;
    await Task.Delay(dots * 1000);

    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
    Console.WriteLine(" [x] Done");

    // here channel could also be accessed as ((AsyncEventingBasicConsumer)sender).Channel
};

await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);


Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();