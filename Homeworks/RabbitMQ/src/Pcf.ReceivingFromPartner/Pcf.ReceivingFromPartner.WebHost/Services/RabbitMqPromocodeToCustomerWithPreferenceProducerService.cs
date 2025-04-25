using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.WebHost.Services;


public class RabbitMqPromocodeToCustomerWithPreferenceProducerService : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string _queueName = "promocode-to-customer-with-preference-events";

    public async Task SendMessageAsync(string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        await Task.Run(() => channel.BasicPublishAsync(exchange: string.Empty, routingKey: _queueName, body: body));
        Console.WriteLine($" [x] Sent '{message}' to queue '{_queueName}'");
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

}