using  FreelanceHub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceHub.Infrastructure
{
	public static class DbSeeder
	{
		public static void SeedCategoriesAndTags(this ModelBuilder builder)
		{
			builder.Entity<Category>(entity =>
			{
				entity.ToTable("categories");
				entity.HasKey(category => category.Id);
				entity.Property(category => category.Id).HasColumnName("category_id");
				entity.Property(category => category.Name).HasColumnName("name").HasMaxLength(100).IsRequired();

				entity.HasData(
					new Category { Id = 1, Name = "Web Development" },
					new Category { Id = 2, Name = "Mobile Development" },
					new Category { Id = 3, Name = "UI / UX Design" },
					new Category { Id = 4, Name = "Writing" },
					new Category { Id = 5, Name = "Marketing" });
			});

			builder.Entity<Tag>(entity =>
			{
				entity.ToTable("tags");
				entity.HasKey(tag => tag.Id);
				entity.Property(tag => tag.Id).HasColumnName("tag_id");
				entity.Property(tag => tag.Name).HasColumnName("name").HasMaxLength(100).IsRequired();

				entity.HasData(
					new Tag { Id = 1, Name = "C#" },
					new Tag { Id = 2, Name = ".NET" },
					new Tag { Id = 3, Name = "React" },
					new Tag { Id = 4, Name = "SQL" },
					new Tag { Id = 5, Name = "Figma" },
					new Tag { Id = 6, Name = "SEO" },
					new Tag { Id = 7, Name = "API" },
					new Tag { Id = 8, Name = "Content Writing" });
			});
		}
	}
}