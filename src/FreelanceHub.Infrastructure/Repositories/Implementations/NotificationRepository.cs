using FreelanceHub.Domain.Models;
using FreelanceHub.Infrastructure.DataBase;
using FreelanceHub.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FreelanceHub.Infrastructure.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<Notification>().AddAsync(notification, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContext.Notifications.AddAsync(notification, cancellationToken);
        }

        public async Task<List<Notification>> GetByUserIdAsync(int userId, int take = 20, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Notification>()
                .Where(n => n.RecipientUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(take)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Notifications
                .AsNoTracking()
                .Where(n => n.RecipientUserId == userId && n.ReadAt == null)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task MarkAsReadAsync(int notificationId, int userId, CancellationToken cancellationToken = default)
        {
            var notification = await _dbContext.Set<Notification>()
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId && n.RecipientUserId == userId, cancellationToken);

            if (notification != null && notification.ReadAt == null)
            {
                notification.ReadAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}