using FreelanceHub.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreelanceHub.Infrastructure.DataBase
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
	{
		public DbSet<Category> Categories { get; set; } = null!;
		public DbSet<Tag> Tags { get; set; } = null!;

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
			builder.SeedCategoriesAndTags();

			builder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
			builder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
			builder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
			builder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");
			
		}
	}
}
