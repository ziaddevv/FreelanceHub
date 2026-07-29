using FreelanceHub.Application.Services.Abstractions;
using FreelanceHub.Application.Services.Implementations;
using FreelanceHub.Domain.Models;
using FreelanceHub.Infrastructure.DataBase;
using FreelanceHub.Infrastructure.Repositories.Abstractions;
using FreelanceHub.Infrastructure.Repositories.Implementations;
using FreelanceHub.Infrastructure.Storage;
using FreelanceHub.Web.Hubs;
using FreelanceHub.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FreelanceHub.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services
				.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
				{
					options.User.RequireUniqueEmail = true;
					options.Password.RequiredLength = 6;
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = true;
					options.Password.RequireUppercase = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
					options.Lockout.MaxFailedAccessAttempts = 5;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/AccessDenied";
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromDays(7);
			});

			builder.Services.AddControllersWithViews();
			builder.Services.AddSignalR();
			builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

			var webRootPath = builder.Environment.WebRootPath ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
			builder.Services.AddSingleton(new FileStorageOptions
			{
				RootPath = webRootPath,
				PublicBasePath = "uploads"
			});

			builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
			builder.Services.AddScoped<IFileUploadService, FileUploadService>();
			builder.Services.AddScoped<IJobBrowseService, JobBrowseService>();
			builder.Services.AddScoped<IProfileService, ProfileService>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
			builder.Services.AddScoped<IAdminRepository, AdminRepository>();
			builder.Services.AddScoped<IJobRepository, JobRepository>();
			builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
			builder.Services.AddScoped<IClientProfileRepository, ClientProfileRepository>();
			builder.Services.AddScoped<IFreelancerProfileRepository, FreelancerProfileRepository>();
			builder.Services.AddScoped<IFileStorageRepository, FileStorageRepository>();
			builder.Services.AddScoped<IJobService, JobService>();
			builder.Services.AddScoped<IContractService, ContractService>();
			builder.Services.AddScoped<IContractRepository, ContractRepository>();
			builder.Services.AddScoped<IChatService, ChatService>();
			builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
			builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

            builder.Services.AddScoped<IApplicationManagementService, ApplicationManagementService>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            
			builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationRealtimeService, NotificationRealtimeService>();

            builder.Services.AddSignalR();

            var app = builder.Build();

			SeedAdminAsync(app.Services, builder.Configuration).GetAwaiter().GetResult();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapHub<ChatHub>("/hubs/chat");
            app.MapHub<NotificationHub>("/hubs/notifications");

            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}

		private static async Task SeedAdminAsync(IServiceProvider services, IConfiguration configuration)
		{
			const string adminRole = "Admin";
			var email = configuration["AdminSeed:Email"] ?? "admin@freelancehub.local";
			var password = configuration["AdminSeed:Password"] ?? "Admin123!";

			using var scope = services.CreateScope();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			if (!await roleManager.RoleExistsAsync(adminRole))
			{
				var roleResult = await roleManager.CreateAsync(new IdentityRole<int>(adminRole));
				if (!roleResult.Succeeded)
				{
					throw new InvalidOperationException($"Unable to create the Admin role: {string.Join("; ", roleResult.Errors.Select(error => error.Description))}");
				}
			}

			var admin = await userManager.FindByEmailAsync(email);
			if (admin is null)
			{
				admin = new ApplicationUser
				{
					UserName = email,
					Email = email,
					EmailConfirmed = true,
					FirstName = "System",
					LastName = "Administrator"
				};

				var createResult = await userManager.CreateAsync(admin, password);
				if (!createResult.Succeeded)
				{
					throw new InvalidOperationException($"Unable to create the admin account: {string.Join("; ", createResult.Errors.Select(error => error.Description))}");
				}
			}

			if (!await userManager.IsInRoleAsync(admin, adminRole))
			{
				var addRoleResult = await userManager.AddToRoleAsync(admin, adminRole);
				if (!addRoleResult.Succeeded)
				{
					throw new InvalidOperationException($"Unable to assign the Admin role: {string.Join("; ", addRoleResult.Errors.Select(error => error.Description))}");
				}
			}
		}
	}
}
