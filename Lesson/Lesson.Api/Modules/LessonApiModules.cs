using Autofac;
using Core.Modules;
using Lesson.Application.Services;
using Shared.Interfaces;

namespace Lesson.Api.Modules;

public class LessonApiModules : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserClaimsService>().As<IUserClaimsService>().SingleInstance();

        base.Load(builder);
    }
}