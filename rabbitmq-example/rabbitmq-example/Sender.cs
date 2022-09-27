#pragma warning disable SA1600

using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMqExample;

internal sealed class Sender
{
    private readonly Settings _settings;
    private readonly ILogger _logger;
    private readonly Random _random = new(42);

    public Sender(Settings settings, ILogger logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public void Produce(CancellationToken token)
    {
        var factory = new ConnectionFactory() { HostName = _settings.Hostname, UserName = _settings.Username, Password = _settings.Password };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: _settings.QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        while (!token.IsCancellationRequested)
        {
            string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _settings.QueueName,
                basicProperties: null,
                body: body);

            _logger.LogInformation(" [x] Sent {message}", message);

            Thread.Sleep(_random.Next(0, 500));
        }
    }
}