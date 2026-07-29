using FreelanceHub.Application.Services.Abstractions;
using FreelanceHub.Domain.Enums;
using FreelanceHub.Domain.Models;
using FreelanceHub.Infrastructure.Repositories.Abstractions;

namespace FreelanceHub.Application.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationRealtimeService _realtimeService;

        public NotificationService(
            INotificationRepository notificationRepository,
            INotificationRealtimeService realtimeService)
        {
            _notificationRepository = notificationRepository;
            _realtimeService = realtimeService;
        }

        public async Task SendApplicationStatusNotificationAsync(
            int freelancerUserId,
            int clientUserId,
            int applicationId,
            string jobTitle,
            ApplicationStatus newStatus,
            CancellationToken cancellationToken = default)
        {
            var isAccepted = newStatus == ApplicationStatus.Accepted;

            var notification = new Notification
            {
                RecipientUserId = freelancerUserId,
                ActorUserId = clientUserId,
                NotificationType = NotificationType.ApplicationStatusChanged,
                Title = isAccepted ? "Application Accepted 🎉" : "Application Status Update",
                Message = isAccepted
                    ? $"Congratulations! Your proposal for '{jobTitle}' was accepted."
                    : $"Your proposal for '{jobTitle}' was not selected.",
                TargetUrl = $"/Applications/Details/{applicationId}",
                RelatedEntityId = applicationId,
                CreatedAt = DateTime.UtcNow
            };

            // 1. Save to Database
            await _notificationRepository.AddAsync(notification, cancellationToken);

            // 2. Broadcast via Real-time Service Abstraction
            await _realtimeService.SendNotificationToUserAsync(
                freelancerUserId,
                new
                {
                    notificationId = notification.NotificationId,
                    title = notification.Title,
                    message = notification.Message,
                    targetUrl = notification.TargetUrl,
                    createdAt = notification.CreatedAt.ToString("o")
                },
                cancellationToken);
        }
    }
}