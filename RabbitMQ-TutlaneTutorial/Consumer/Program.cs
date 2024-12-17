using System;
using RabbitMQ.Client;
using Consumer;

const string UserName = "guest";
const string Password = "guest";
const string HostName = "localhost";

var connectionFactory = new ConnectionFactory
{
    UserName = UserName,
    Password = Password,
    HostName = HostName
};

// Create asynchronous connection and channel
var connection = await connectionFactory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

// Set Quality of Service to process one message at a time
await channel.BasicQosAsync(0, 1, false);

// Create and start the message receiver
var messageReceiver = new MessageReceiver(channel);
await messageReceiver.StartConsumingAsync("demoqueue");

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();