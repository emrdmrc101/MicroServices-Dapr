using Activity;
using Core.Dapr;
using Lesson.Domain.Interfaces.Grpc;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;

namespace Lesson.Infrastructure.Grpc.Services;

public class ActivityService(AppIds appIds, IUserClaimsService claimsService, IConfiguration configuration) : BaseService(configuration, claimsService,appIds.ActivityServiceGrpc), IActivityService
{
   public Task GetActivitiesByLessonId(string lessonId)
   {
      var client = new ActivityServiceGrpc.ActivityServiceGrpcClient(Channel);

      var request = new GetActivitiesByLessonIdRequest { LessonId = lessonId };
      var response = client.GetActivitiesByLessonId(request, Metadata);
      
      return Task.CompletedTask;
   }
}