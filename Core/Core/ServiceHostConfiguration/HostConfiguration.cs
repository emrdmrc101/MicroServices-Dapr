using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;

namespace Core.ServiceHostConfiguration;

public static class HostConfiguration
{
    public static void ConfigureHost(this WebApplicationBuilder webApplicationBuilder)
    {
        var ports = webApplicationBuilder.Configuration.GetSection("Ports").Get<Dictionary<string, int>>();
        webApplicationBuilder.WebHost.ConfigureKestrel(options =>
        {
            foreach (var port in ports)
            {
                switch (port.Key)
                {
                    case "grpc":
                        options.ListenLocalhost(port.Value,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
                        options.ListenAnyIP(port.Value,
                            listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
                        break;
                    case "http":
                        options.ListenLocalhost(port.Value,
                            listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                
                               
                            });
                        
                        options.ListenAnyIP(port.Value,
                            listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                
                               
                            });
                        break;
                }
            }
        });
    }
}