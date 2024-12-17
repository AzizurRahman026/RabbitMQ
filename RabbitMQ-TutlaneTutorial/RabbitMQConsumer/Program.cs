using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQConsumer;
using System.Threading.Channels;

const string UName = "guest";
const string Pwd = "guest";
const string HName = "localhost";

ConnectionFactory connectionFactory = new ConnectionFactory
{
    HostName = HName,
    UserName = UName,
    Password = Pwd,
};
var connection = await connectionFactory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();
// accept only one unack-ed message at a time
// uint prefetchSize, ushort prefetchCount, bool global
await channel.BasicQosAsync(0, 1, false);
MessageReceiver messageReceiver = new MessageReceiver(channel);

// var messageReceiver = new AsyncEventingBasicConsumer(channel);

/*if (messageReceiver != null)
{
    await channel.BasicConsumeAsync("request.queue", false, consumer: messageReceiver);
    Console.ReadLine();
}*/


await messageReceiver.StartConsumingAsync("request.queue");