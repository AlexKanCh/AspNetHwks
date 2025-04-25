using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Services;

public class RabbitMqService : IAsyncDisposable
{
    public async Task SendMessageAsync(string queueName, string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest"
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        await Task.Run(() => channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body));
        Console.WriteLine($" [x] Sent '{message}' to queue '{queueName}'");
    }

    public async Task StartConsumingAsync(string queueName, Func<string, Task> onMessageReceived)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest"
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received '{message}' from queue '{queueName}'");

            await onMessageReceived(message);
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }

    public async ValueTask DisposeAsync()
    {
    }
}

