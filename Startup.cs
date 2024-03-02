namespace Comment_Post
{
    using Comment_Post.Data;
    using Comment_Post.IRepository;
    using Comment_Post.Repository;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Microsoft.AspNetCore.Identity;
    using Comment_Post.Models;
    using Comment_Post.middleware;

    using Comment_Post.Seeder;
    using Comment_Post.Repository.Comment_Post.Repositories;
    using Comment_Post.Service;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

            services.AddScoped<UserRepository>();
            
            services.AddSignalR();

            services.AddHostedService<SignalRNotificationBackgroundService>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                IdentityDataSeeder.InitializeAsync(userManager, roleManager).Wait();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
         
            app.UseRouting();
         
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
       
            app.UseSweetAlertNotificationMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
              
            });

        }
    }

}
