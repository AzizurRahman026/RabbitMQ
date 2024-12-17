using RabbitMQ.Client;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Channels;

// Connection parameters
string UserName = "guest";
string Password = "guest";
string HostName = "localhost";

var connectionFactory = new ConnectionFactory
{
    UserName = UserName,
    Password = Password,
    HostName = HostName
};

// Create an asynchronous connection
var connection = await connectionFactory.CreateConnectionAsync(); // Await the connection

// Create an asynchronous channel
var channel = await connection.CreateChannelAsync(); // Await the channel to get IModel instance


string exchangeName = "demoExchange";
string exchangeType = ExchangeType.Direct;
await channel.ExchangeDeclareAsync("demoExchange", exchangeType);

Console.WriteLine($"Creating Exchange: {exchangeName} & type {exchangeType}");

string queueName = "demoqueue";
// Create Queue
await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false,arguments: null);
Console.WriteLine("Creating Queue");


// Bind Queue to Exchange
await channel.QueueBindAsync("demoqueue", "demoExchange", "directexchange_key");
Console.WriteLine("Creating Binding");

byte[] messagebuffer = Encoding.Default.GetBytes("Publisher Direct Message...2");
var properties = new BasicProperties { Persistent = true };

try
{
    await channel.BasicPublishAsync(
        exchange: exchangeName,
        routingKey: "directexchange_key",
        mandatory: false,
        basicProperties: properties,
        body: messagebuffer
    );
}
catch (Exception ex)
{ 
    Console.Error.WriteLine($"{DateTime.Now} [ERROR], ex: {ex}");
}

Console.WriteLine("Message Sent");

