using System.Collections.Concurrent;
using Activity;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace UserActivity.Infrastructure.Grpc.Services;

public class NotifyCreatedActivityService : NotificationServiceGrpcForActivity.NotificationServiceGrpcForActivityBase
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<IServerStreamWriter<ActivityNotificationResponse>>> _lessonClients =
        new();

    public override async Task SubscribeActivities(
        ActivityNotificationPushRequest request,
        IServerStreamWriter<ActivityNotificationResponse> responseStream,
        ServerCallContext context)
    {
        var lessonId = request.LessonId;

        var clients = _lessonClients.GetOrAdd(lessonId, _ => new ConcurrentBag<IServerStreamWriter<ActivityNotificationResponse>>());
        clients.Add(responseStream);

        Console.WriteLine($"Client connected for LessonId: {lessonId}");

        try
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }
        }
        finally
        {
            _lessonClients.TryRemove(lessonId, out _);
            Console.WriteLine($"Client disconnected for LessonId: {lessonId}");
        }
    }
    
    public void PublishNotification(string lessonId, ActivityNotificationRequest request)
    {
        if (_lessonClients.TryGetValue(lessonId, out var clients))
        {
            foreach (var client in clients)
            {
                try
                {
                    client.WriteAsync(new ActivityNotificationResponse
                    {
                        LessonId = lessonId,
                        ActivityType = request.ActivityType,
                        ContentType = request.ContentType,
                        FirstName = request.FirstName,
                        Id = request.Id,
                        LastName = request.LastName,
                        UserId = request.UserId,
                        Timestamp = DateTime.UtcNow.ToTimestamp().ToString()
                      
                    }).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to client: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine($"No clients connected for LessonId: {lessonId}");
        }
    }
}