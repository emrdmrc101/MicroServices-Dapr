using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Consul;
using Core.Middlewares;
using Core.Modules;
using Core.ServiceBus;
using Core.ServiceHostConfiguration;
using Core.Tracing;
using Identity.Api.Modules;
using Identity.Application.Modules;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Modules;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

VaultService.SetVaultSecrets(builder);

#region [Register Modules]

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
    b =>
    {
        b.RegisterModule<IdentityApiModules>();
        b.RegisterModule<ApplicationModule>();
        b.RegisterModule<InfrastructureModule>();
        b.RegisterType<IdentityDbContext>().AsSelf();
        b.RegisterModule(new CoreModule(builder.Configuration));
    });

#endregion

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetryAndJaeger(builder.Configuration);
builder.Services.AddMassTransit(builder.Configuration);
builder.Services.AddMassTransitHostedService();


builder.ConfigureHost();
builder.Services.AddDbContext<IdentityDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetValue<string>("Database:connectionString"));
});

var app = builder.Build();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.AddExceptionHandlingMiddleware();
app.AddTraceMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var dbContext = app.Services.GetService<IdentityDbContext>();
Console.WriteLine("Migrations...");
if (dbContext != null) await dbContext.Database.MigrateAsync();

app.Run();