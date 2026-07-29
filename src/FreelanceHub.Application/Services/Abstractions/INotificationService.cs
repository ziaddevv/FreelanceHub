using FreelanceHub.Domain.Enums;

namespace FreelanceHub.Application.Services.Abstractions
{
    public interface INotificationService
    {
        Task SendApplicationStatusNotificationAsync(
            int freelancerUserId,
            int clientUserId,
            int applicationId,
            string jobTitle,
            ApplicationStatus newStatus,
            CancellationToken cancellationToken = default);
    }
}