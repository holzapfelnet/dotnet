#pragma warning disable SA1600

namespace RabbitMqExample
{
    internal sealed class Settings
    {
        public string Hostname { get; set; } = default!;

        public string Username { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string QueueName { get; set; } = default!;
    }
}