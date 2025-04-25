using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.Services;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IChannel _channel;
    private const string _queueName = "promocode-to-customer-with-preference-events";

    public RabbitMqConsumerService(IServiceProvider serviceProvider, ILogger<RabbitMqConsumerService> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        _logger.LogInformation("RabbitMQ GivingToCustomer consumer started. Listening to queue 'task_queue'...");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"GivingToCustomer: Received message: {message}");

            try
            {
                await ProcessMessageAsync(message);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GivingToCustomer: Error processing message: {message}");
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(string message)
    {
        _logger.LogInformation($"GivingToCustomer: Processing  message: {message}");
        if (!string.IsNullOrEmpty(message))
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var promoCodeService = scopedServices.GetRequiredService<IPromoCodeService>();

                var promoCode= JsonSerializer.Deserialize<PromoCode>(message);

                await promoCodeService.GivePromoCodeToCustomersWithPreferenceAsync(promoCode);
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ Administration consumer is stopping.");

        await _channel?.CloseAsync();
        await _connection?.CloseAsync();

        await base.StopAsync(stoppingToken);
    }
}
