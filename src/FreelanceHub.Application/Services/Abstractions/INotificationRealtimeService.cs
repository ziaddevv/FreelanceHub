namespace FreelanceHub.Application.Services.Abstractions
{
    public interface INotificationRealtimeService
    {
        Task SendNotificationToUserAsync(
            int recipientUserId,
            object notificationPayload,
            CancellationToken cancellationToken = default);
    }
}