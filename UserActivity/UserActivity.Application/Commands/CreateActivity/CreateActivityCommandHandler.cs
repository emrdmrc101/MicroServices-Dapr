using Core.Domain.Mapper.Interfaces;
using Core.Tracing;
using MediatR;
using UserActivity.Domain.Interfaces;
using Shared.Interfaces;

namespace UserActivity.Application.Commands.CreateActivity;

public class CreateActivityCommandHandler(
    IActivityRepository activityRepository,
    ActivityTracing activityTracing,
    IUserClaimsService contextService,
    IMapperService mapperService
) : IRequestHandler<CreateActivityCommand, CreateActivityCommandResponse>
{
    public async Task<CreateActivityCommandResponse> Handle(CreateActivityCommand request,
        CancellationToken cancellationToken)
    {
        return await activityTracing.ExecuteWithTracingAsync<CreateActivityCommandResponse>(
            nameof(CreateActivityCommandHandler),
            async () =>
            {
                var activityEntity = mapperService.Map<CreateActivityCommand, Domain.Entities.Activities>(request);
               
                activityEntity.CreatorId = contextService.UserContext.UserId;
                activityEntity.Email = contextService.UserContext.Email;
                activityEntity.FirstName = contextService.UserContext.FirstName;
                activityEntity.LastName = contextService.UserContext.LastName;
                await activityRepository.CreateAsync(activityEntity);
                
                return await Task.FromResult(new CreateActivityCommandResponse()
                {
                    Success = true
                });
            });
    }
}