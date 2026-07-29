using FreelanceHub.Domain.Enums;

namespace FreelanceHub.Domain.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int RecipientUserId { get; set; }

        public int? ActorUserId { get; set; }

        public NotificationType NotificationType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string TargetUrl { get; set; } = string.Empty;

        public int? RelatedEntityId { get; set; }

        public string? GroupKey { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ReadAt { get; set; }

        public ApplicationUser RecipientUser { get; set; } = null!;

        public ApplicationUser? ActorUser { get; set; }
    }
}