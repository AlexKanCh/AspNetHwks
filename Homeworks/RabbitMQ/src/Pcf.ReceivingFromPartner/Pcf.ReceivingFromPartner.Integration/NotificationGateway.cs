using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Integration.Configuration;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class NotificationGateway: INotificationGateway
    {
        private readonly string _signalRUrl;

        public NotificationGateway(IOptions<SignalRSettings> options)
        {
            _signalRUrl = options.Value.Url;
        }

        public async Task SendNotificationToPartnerAsync(Guid partnerId, string message)
        {
            try
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl(_signalRUrl)
                    .Build();
                await connection.StartAsync();
                await connection.InvokeAsync("SendNotificationToPartner", partnerId, message);
                await connection.DisposeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification to partner {partnerId}: {ex.Message}");
            }
        }
    }
}