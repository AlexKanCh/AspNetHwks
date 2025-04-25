using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pcf.Administration.Core.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Services;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IEmployeeService _employeeService;
    private IConnection _connection;
    private IChannel _channel;
    private const string _queueName = "promocode-events";

    public RabbitMqConsumerService(ILogger<RabbitMqConsumerService> logger, IEmployeeService employeeService)
    {
        _logger = logger;
        _employeeService = employeeService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        _logger.LogInformation("RabbitMQ Administration consumer started. Listening to queue 'task_queue'...");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"Administration: Received message: {message}");

            try
            {
                await ProcessMessageAsync(message);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Administration: Error processing message: {message}");
            }
        };

        // Начало прослушивания очереди
        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);

        // Ожидание завершения работы сервиса
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(string message)
    {
        // Здесь можно вызвать бизнес-логику, например, через сервис из Core
        _logger.LogInformation($"Administration: Processing  message: {message}");
        if (Guid.TryParse(message, out var id))
        {
            await _employeeService.UpdateAppliedPromocodesAsync(id);
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