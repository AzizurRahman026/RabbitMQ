using RabbitMQ.Client;
using System;
using System.Text;

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
using var model = await connection.CreateChannelAsync(); // Await the channel to get IModel instance


string exchangeName = "demoExchange";
string exchangeType = ExchangeType.Direct;
model.ExchangeDeclareAsync("demoExchange", exchangeType);

Console.WriteLine($"Creating Exchange: {exchangeName} & type {exchangeType}");


// Create Queue
await model.QueueDeclareAsync(queue: "demoqueue", durable: true, exclusive: false, autoDelete: false,arguments: null);
Console.WriteLine("Creating Queue");


// Bind Queue to Exchange
model.QueueBindAsync("demoqueue", "demoExchange", "directexchange_key");
Console.WriteLine("Creating Binding");


// byte[] messagebuffer = Encoding.Default.GetBytes("Direct Message");

model.BasicPublishAsync(
    exchange: exchangeName,
    routingKey: "directexchange_key",
    basicProperties: null,
    body: Encoding.Default.GetBytes("hello")
);

Console.WriteLine("Message Sent");

