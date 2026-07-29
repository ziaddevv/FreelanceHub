using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FreelanceHub.Web.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Extract the User ID from the authenticated HttpContext/User claim
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                // Add this connection to a group specific to this user ID
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}