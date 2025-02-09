using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Consul;
using Core.ExceptionHandler;
using Core.Identity;
using Core.Middlewares;
using Core.Modules;
using Core.ServiceBus;
using Core.ServiceHostConfiguration;
using Core.Tracing;
using Grpc.Health.V1;
using Grpc.HealthCheck;
using MassTransit;
using UserActivity.Api.Module;
using UserActivity.Application.Module;
using UserActivity.Infrastructure.Grpc.Services;
using UserActivity.Infrastructure.Module;
using UserActivity.Infrastructure.MongoDb;

var builder = WebApplication.CreateBuilder(args);

// builder.AddMyConsul(builder);
VaultService.SetVaultSecrets(builder);

#region [Register Modules]

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
    b =>
    {
        b.RegisterModule<ActivityServiceModule>();
        b.RegisterModule<BaseModule>();
        b.RegisterModule<ApplicationModule>();
        b.RegisterModule(new InfrastructureModules(builder.Configuration));
        b.RegisterModule(new CoreModule(builder.Configuration));
    });

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMassTransit(builder.Configuration);
builder.Services.AddMassTransitHostedService();
builder.Services.AddOpenTelemetryAndJaeger(builder.Configuration);
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddHttpClient();

builder.ConfigureHost();

#region Grpc

builder.Services.AddGrpc(o =>
{
    o.EnableDetailedErrors = true;
    o.MaxReceiveMessageSize =1024 * 1024 * 100;
    o.MaxSendMessageSize = 1024 * 1024 * 100; 
    
});

builder.Services.AddGrpcReflection();
builder.Services.AddGrpcHealthChecks();

#endregion

var app = builder.Build();
app.MapGrpcHealthChecksService();
app.UseGrpcWeb();
app.UseGrpcWeb(new GrpcWebOptions() { DefaultEnabled = true});
app.MapGrpcService<ActivityService>().EnableGrpcWeb().RequireAuthorization();
app.MapGrpcService<NotifyCreatedActivityService>().EnableGrpcWeb().RequireAuthorization();


var healthService = new HealthServiceImpl();
healthService.SetStatus("ActivityServiceGrpc", HealthCheckResponse.Types.ServingStatus.Serving);

// app.MapGrpcService<ActivityService>();
app.CustomExceptionHandler();
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<MongoDbInitializer>();
    initializer.CreateCollectionIfNotExists("ActivityDb");
}

app.MapGet("/health", () => Results.Ok("Healthy"));

app.AddExceptionHandlingMiddleware();
app.AddUserClaimsMiddleware();
app.AddTraceMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthorization();;
app.MapControllers();
app.Run();