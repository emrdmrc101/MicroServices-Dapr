using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Consul;
using Core.Identity;
using Core.Middlewares;
using Core.Modules;
using Core.ServiceHostConfiguration;
using Core.Tracing;
using Gateway.Modules;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

VaultService.SetVaultSecrets(builder);

#region [Register Modules]

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
    b =>
    {
        b.RegisterModule<GatewayModule>();
        b.RegisterModule<BaseModule>();
        b.RegisterModule(new CoreModule(builder.Configuration));
    });

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile($"ocelot.json", false, true);
builder.Services.AddOcelot();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddOpenTelemetryAndJaeger(builder.Configuration);

builder.ConfigureHost();
var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/health")
    {
        await context.Response.WriteAsync("Healthy");
        return;
    }
    await next();
});

app.AddExceptionHandlingMiddleware();
app.AddTraceMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOcelot().Wait();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();