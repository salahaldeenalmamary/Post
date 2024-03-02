namespace Comment_Post.Service
{
    using Comment_Post.middleware;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    public class BackgroundTaskData
    {
        public string message { get; set; }
        public string Action { get; set; }
        
    }

    public class SignalRNotificationBackgroundService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<SignalRNotificationBackgroundService> _logger;
        private readonly ConcurrentQueue<BackgroundTaskData> _taskQueue = new ConcurrentQueue<BackgroundTaskData>();

        public SignalRNotificationBackgroundService(IHubContext<NotificationHub> hubContext, ILogger<SignalRNotificationBackgroundService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessBackgroundTasks(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); 
            }
        }

        public void EnqueueBackgroundTask(BackgroundTaskData taskData)
        {
            _taskQueue.Enqueue(taskData);
        }

        private async Task ProcessBackgroundTasks(CancellationToken stoppingToken)
        {
            while (_taskQueue.TryDequeue(out var taskData))
            {
                try
                {
                    var message = $"{taskData.message}/{taskData.Action} action executed!";
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing background task");
                }
            }
        }
    }

    
}
