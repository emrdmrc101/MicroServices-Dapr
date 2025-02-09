using Autofac;
using Core.Dapr;
using Core.Modules;
using Lesson.Domain.Interfaces.Grpc;
using Lesson.Domain.Interfaces.Repositories;
using Lesson.Infrastructure.Repositories;
using ActivityService = Lesson.Infrastructure.Grpc.Services.ActivityService;

namespace Lesson.Infrastructure.Modules;

public class InfrastructureModules : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {   
        builder.RegisterType<LessonRepository>().As<ILessonRepository>().InstancePerDependency();
        builder.RegisterType<UserLessonRepository>().As<IUserLessonRepository>().InstancePerDependency();
        builder.RegisterType<ActivityService>().SingleInstance();
        builder.RegisterType<AppIds>().SingleInstance();
        builder.RegisterType<ActivityService>().As<IActivityService>().InstancePerDependency();

        base.Load(builder);
    }
}