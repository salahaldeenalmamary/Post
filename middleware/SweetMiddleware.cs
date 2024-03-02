using Comment_Post.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Comment_Post.middleware
{
    public class SweetAlertNotificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SweetAlertNotificationMiddleware(RequestDelegate next, IHubContext<NotificationHub> hubContext, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

               
                var newUsers = dbContext.Users.Where(u => u.CreatedAt >= DateTime.Now.AddMinutes(-5)).ToList();

                foreach (var newUser in newUsers)
                {
                   
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", "New user created: " + newUser.name);
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }


    public static class SweetAlertNotificationMiddlewareExtensions
    {
        public static IApplicationBuilder UseSweetAlertNotificationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SweetAlertNotificationMiddleware>();
        }
    }
}
