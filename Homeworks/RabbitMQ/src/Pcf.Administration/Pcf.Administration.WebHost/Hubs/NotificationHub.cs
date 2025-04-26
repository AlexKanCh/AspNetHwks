using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Hubs;
public class NotificationHub : Hub
{
    //public async Task SendNotification(string message)
    //{
    //    await Clients.Caller.SendAsync("ReceiveNotification", message);
    //}

    public async Task JoinGroup(string partnerId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, partnerId);
        await Clients.Caller.SendAsync("ReceiveMessage", $"You joined group: {partnerId}");
    }

    public async Task SendNotificationToPartner(string partnerId, string message)
    {
        await Clients.Group(partnerId).SendAsync("ReceiveNotification", message);
    }
}
