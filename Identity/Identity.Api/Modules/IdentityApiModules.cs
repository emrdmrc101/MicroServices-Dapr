using Autofac;
using Core.Modules;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services;
using Identity.Domain.Interfaces.Common;

namespace Identity.Api.Modules;

public class IdentityApiModules : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        builder.RegisterType<MapperService>().As<IMapperService>().SingleInstance();
        base.Load(builder);
    }
}