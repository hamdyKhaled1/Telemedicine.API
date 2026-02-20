using Microsoft.AspNetCore.SignalR;

namespace Telemedicine.API.Hubs
{
    public class WaitingRoomHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            var doctorId = Context.GetHttpContext()?.Request.Query["doctorId"];

            if (!string.IsNullOrEmpty(doctorId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"Doctor_{doctorId}");
            }

            await base.OnConnectedAsync();
        }
    }
}
