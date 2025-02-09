using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;

namespace Lesson.Infrastructure.Grpc.Services;

public class BaseService(IConfiguration configuration ,IUserClaimsService contextService, string appId, GrpcChannelOptions channelOptions = null)
{
    private string GrpcAdress => configuration.GetValue<string>("Dapr:GrpcAPI") ?? throw new Exception("Not found darp grpc api address");
    protected GrpcChannel Channel => GrpcChannel.ForAddress(GrpcAdress, channelOptions ?? new GrpcChannelOptions());

    private Metadata _metaData { get; set; } = new Metadata();

    public Metadata Metadata
    {
        get
        {
            _metaData.Add("Authorization", contextService.UserContext.Token);
            _metaData.Add("dapr-app-id", appId);

            return _metaData;
        }

        set { _metaData = value; }
    }
}