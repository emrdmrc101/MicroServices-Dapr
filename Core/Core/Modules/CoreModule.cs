using System.Diagnostics;
using Autofac;
using Core.Cache.Distributed;
using Core.Consul;
using Core.Domain.Cache.Interfaces;
using Core.Domain.Mapper.Interfaces;
using Core.Log;
using Core.Mapper;
using Core.Mediatr;
using Core.Tracing;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Modules;

public class CoreModule(ConfigurationManager configurationManager) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        string serviceName = configurationManager.GetValue<string>("ServiceName");

        builder.Register<Core.Domain.Log.Interfaces.ILogger>(r => LoggerFactory.CreateLogger(configurationManager))
            .As<Core.Domain.Log.Interfaces.ILogger>().SingleInstance();
        builder.Register<IDistributedCache>(r => DistributedCacheFactory.CreateCache(configurationManager))
            .As<IDistributedCache>().InstancePerDependency();
        builder.Register(r => new ActivityTracing(new ActivitySource(serviceName))).SingleInstance();

        #region [MediatR]

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        builder.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces();

        builder.RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();
        
        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .InstancePerDependency();
        
        builder.RegisterGeneric(typeof(ValidationBehavior<,>))
            .As(typeof(IPipelineBehavior<,>));

        #endregion

        var configurationBuilder = new ConfigurationBuilder()
            .AddConfiguration(configurationManager);

        builder.Register<IConfigurationBuilder>(r => configurationBuilder);
        builder.RegisterType<VaultService>().AsSelf().SingleInstance();
        builder.RegisterType<MapperService>().As<IMapperService>().SingleInstance();
        
        base.Load(builder);
    }
}