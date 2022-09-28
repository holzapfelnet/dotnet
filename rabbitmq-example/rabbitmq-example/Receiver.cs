#pragma warning disable SA1600

using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMq.Example;

internal sealed class Receiver
{
    private readonly Settings _settings;
    private readonly ILogger _logger;

    public Receiver(Settings settings, ILogger logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public void Consume(CancellationToken token)
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

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation(" [x] Received {message}", message);
        };

        channel.BasicConsume(
            queue: _settings.QueueName,
            autoAck: true,
            consumer: consumer);

        token.WaitHandle.WaitOne();
    }
}