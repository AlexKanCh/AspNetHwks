using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Hubs;
public class NotificationHub : Hub
{
    public async Task SendNotification(string message)
    {
        await Clients.Caller.SendAsync("ReceiveNotification", message);
    }

    public async Task SendNotificationToPartner(string partnerId, string message)
    {
        await Clients.Group(partnerId).SendAsync("ReceiveNotification", message);
    }
}
