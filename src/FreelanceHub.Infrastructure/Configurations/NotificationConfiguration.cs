using FreelanceHub.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceHub.Infrastructure.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notifications");
            builder.HasKey(notification => notification.NotificationId);

            builder.Property(notification => notification.NotificationId).HasColumnName("notification_id");
            builder.Property(notification => notification.RecipientUserId).HasColumnName("recipient_user_id").IsRequired();
            builder.Property(notification => notification.ActorUserId).HasColumnName("actor_user_id");
            builder.Property(notification => notification.NotificationType).HasColumnName("notification_type").HasConversion<int>().IsRequired();
            builder.Property(notification => notification.Title).HasColumnName("title").HasMaxLength(160).IsRequired();
            builder.Property(notification => notification.Message).HasColumnName("message").HasMaxLength(500).IsRequired();
            builder.Property(notification => notification.TargetUrl).HasColumnName("target_url").HasMaxLength(500).IsRequired();
            builder.Property(notification => notification.RelatedEntityId).HasColumnName("related_entity_id");
            builder.Property(notification => notification.GroupKey).HasColumnName("group_key").HasMaxLength(100);
            builder.Property(notification => notification.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
            builder.Property(notification => notification.ReadAt).HasColumnName("read_at");

            builder.HasIndex(notification => new { notification.RecipientUserId, notification.ReadAt, notification.CreatedAt });
            builder.HasIndex(notification => new { notification.RecipientUserId, notification.GroupKey })
                .IsUnique()
                .HasFilter("[group_key] IS NOT NULL");

            builder.HasOne(notification => notification.RecipientUser)
                .WithMany(user => user.ReceivedNotifications)
                .HasForeignKey(notification => notification.RecipientUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(notification => notification.ActorUser)
                .WithMany()
                .HasForeignKey(notification => notification.ActorUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}