using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQProducer.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IOptions<RabbitMQOptions> _options;
        public MessageProducer(IOptions<RabbitMQOptions> options)
        {
            _options = options;
        }
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.Value.HostName,
                UserName = _options.Value.UserName,
                Password = _options.Value.Password

            };

            var conn = factory.CreateConnection();

            using var channel = conn.CreateModel();

            var _queueName = _options.Value.QueueName;


            channel.QueueDeclare(_queueName, durable: false, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish(exchange: "",
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);
        }

        public class RabbitMQOptions
        {
            public string HostName { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string QueueName { get; set; } = string.Empty;
        }
    }
}
