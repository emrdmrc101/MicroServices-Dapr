using Microsoft.Extensions.Configuration;

namespace Core.Dapr;

public class AppIds(IConfiguration configuration)
{
    public string ActivityService =>
        configuration.GetValue<string>("Dapr:AppIds:ActivityService") ??
        throw new Exception("Dapr ActivityService not  found app id");
    
    public string ActivityServiceGrpc =>
        configuration.GetValue<string>("Dapr:AppIds:ActivityServiceGrpc") ??
        throw new Exception("Dapr ActivityServiceGrpc not  found app id");
    
    public string LessonService =>
        configuration.GetValue<string>("Dapr:AppIds:LessonService") ??
        throw new Exception("Dapr LessonService not  found app id");
    
    public string IdentityService =>
        configuration.GetValue<string>("Dapr:AppIds:IdentityService") ??
        throw new Exception("Dapr IdentityService not  found app id");
}