#pragma warning disable SA1600
#pragma warning disable S1118

using Microsoft.Extensions.Logging;

namespace RabbitMq.Example;

public class Program
{
    public static void Main()
    {
        using ILoggerFactory loggerFactory =
            LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                }));

        var logger = loggerFactory.CreateLogger<Program>();
        var settings = new Settings() { Hostname = "localhost", Username = "user1", Password = "test", QueueName = "myTestQueue" };

        var sender = new Sender(settings, logger);
        var receiver = new Receiver(settings, logger);
        var cancelToken = CancellationToken.None;

        logger.LogInformation("Start");

        Task.Run(() => sender.Produce(cancelToken), cancelToken);

        Thread.Sleep(5000);

        Task.Run(() => receiver.Consume(cancelToken), cancelToken);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}