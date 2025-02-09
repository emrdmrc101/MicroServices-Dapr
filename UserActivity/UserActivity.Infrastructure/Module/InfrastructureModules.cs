using Activity;
using Autofac;
using Core.Modules;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UserActivity.Domain.Interfaces;
using UserActivity.Infrastructure.Grpc.Services;
using UserActivity.Infrastructure.MongoDb;
using UserActivity.Infrastructure.Objects;
using UserActivity.Infrastructure.Repositories;

namespace UserActivity.Infrastructure.Module;

public class InfrastructureModules(ConfigurationManager configurationManager) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
        { 
            var mongoSettings = new MongoDBSettings();
            configurationManager.GetSection("MongoDBSettings").Bind(mongoSettings);
            return mongoSettings;
        }).SingleInstance();
        
        builder.Register(context =>
        {
            var settings = context.Resolve<MongoDBSettings>();
            return new MongoClient(settings.ConnectionString);
        }).As<IMongoClient>().SingleInstance();
        
        builder.Register(context => new MongoDbInitializer()).As<MongoDbInitializer>().SingleInstance();
        
        builder.Register(context =>
        {
            var client = context.Resolve<IMongoClient>();
            var settings = context.Resolve<MongoDBSettings>();
            return client.GetDatabase(settings.DatabaseName);
        }).As<IMongoDatabase>().InstancePerLifetimeScope();


        builder.RegisterType<ActivityRepository>().As<IActivityRepository>().InstancePerDependency();
        builder.RegisterType<ActivityService>().As<ActivityServiceGrpc.ActivityServiceGrpcBase>().InstancePerDependency();
        builder.RegisterType<NotifyCreatedActivityService>().SingleInstance();
        
        base.Load(builder);
    }
}