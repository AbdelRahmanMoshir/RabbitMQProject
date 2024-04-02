// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello Moshir to your consumer");

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "user",
    Password = "pass",
    VirtualHost = "/",

};

var conn = factory.CreateConnection();

using var channel = conn.CreateModel();

channel.QueueDeclare("TestQueue", durable: false, exclusive: false);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{

    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"New Message :- {message}");
};

channel.BasicConsume("TestQueue", true, consumer);

Console.ReadKey();