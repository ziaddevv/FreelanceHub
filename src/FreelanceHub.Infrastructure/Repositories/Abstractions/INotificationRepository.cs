using FreelanceHub.Domain.Models;

namespace FreelanceHub.Infrastructure.Repositories.Abstractions
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
       
        Task<List<Notification>> GetByUserIdAsync(int userId, int take = 20, CancellationToken cancellationToken = default);

        Task MarkAsReadAsync(int notificationId, int userId, CancellationToken cancellationToken = default);

        Task<List<Notification>> GetUnreadByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}