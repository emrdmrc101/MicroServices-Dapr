using Autofac;
using Core.Modules;
using Shared.Interfaces;
using UserActivity.Application.Services;

namespace UserActivity.Api.Module;

public class ActivityServiceModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserClaimsService>().As<IUserClaimsService>().SingleInstance();
        base.Load(builder);
    }
}