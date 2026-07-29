using FreelanceHub.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreelanceHub.Infrastructure.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Skill> Skills => Set<Skill>();

        public DbSet<Attachment> Attachments => Set<Attachment>();

        public DbSet<FreelancerSkill> FreelancerSkills => Set<FreelancerSkill>();

        public DbSet<JobSkill> JobSkills => Set<JobSkill>();

        public DbSet<FreelancerProfileAttachment> FreelancerProfileAttachments => Set<FreelancerProfileAttachment>();

        public DbSet<ClientProfileAttachment> ClientProfileAttachments => Set<ClientProfileAttachment>();

        public DbSet<JobAttachment> JobAttachments => Set<JobAttachment>();

        public DbSet<ApplicationAttachment> ApplicationAttachments => Set<ApplicationAttachment>();

        public DbSet<ContractAttachment> ContractAttachments => Set<ContractAttachment>();

        public DbSet<FreelancerProfile> FreelancerProfiles => Set<FreelancerProfile>();
        public DbSet<ClientProfile> ClientProfiles => Set<ClientProfile>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<Review> Reviews => Set<Review>();

        public DbSet<Application> Applications => Set<Application>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<Notification> Notifications => Set<Notification>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Tag> Tags => Set<Tag>();

        public DbSet<JobCategory> JobCategories => Set<JobCategory>();

        public DbSet<JobTag> JobTags => Set<JobTag>();

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            builder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
            builder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");
        }

        private void UpdateTimestamps()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<ApplicationUser>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(user => user.CreatedAt).IsModified = false;
                }

                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ClientProfile>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(profile => profile.CreatedAt).IsModified = false;
                }

                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries<FreelancerProfile>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(profile => profile.CreatedAt).IsModified = false;
                }

                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries<Job>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(job => job.CreatedAt).IsModified = false;
                }

                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                    entry.Entity.DeletedAt = entry.Entity.IsDeleted
                        ? entry.Entity.DeletedAt ?? utcNow
                        : null;
                }
            }

            foreach (var entry in ChangeTracker.Entries<Application>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(application => application.CreatedAt).IsModified = false;
                }

                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries<Contract>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(contract => contract.CreatedAt).IsModified = false;
                }

                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                }
            }

            foreach (var entry in ChangeTracker.Entries<Attachment>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.UploadedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(attachment => attachment.UploadedAt).IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ApplicationUserRole>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.AssignedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(userRole => userRole.AssignedAt).IsModified = false;
                }
            }
        }
    }
}