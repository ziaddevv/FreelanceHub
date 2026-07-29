using FreelanceHub.Application.Services.Abstractions;
using FreelanceHub.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FreelanceHub.Web.Services
{
    public class NotificationRealtimeService : INotificationRealtimeService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationRealtimeService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToUserAsync(
            int recipientUserId,
            object notificationPayload,
            CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients
                .Group($"User_{recipientUserId}")
                .SendAsync("ReceiveNotification", notificationPayload, cancellationToken);
        }
    }
}