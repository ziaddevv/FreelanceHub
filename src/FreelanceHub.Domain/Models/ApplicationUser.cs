using FreelanceHub.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FreelanceHub.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public UserStatus UserStatus { get; set; } = UserStatus.Active;

        public int? ProfileImageAttachmentId { get; set; }

        public Attachment? ProfileImageAttachment { get; set; }

        public ClientProfile? ClientProfile { get; set; }

        public FreelancerProfile? FreelancerProfile { get; set; }

        public ICollection<Attachment> UploadedAttachments { get; set; } = new List<Attachment>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        public ICollection<ChatMessage> SentChatMessages { get; set; } = new List<ChatMessage>();

        public ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();


    }
}