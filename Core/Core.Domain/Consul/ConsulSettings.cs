using Consul;

namespace Core.Domain.Consul;

public class ConsulSettings
{
    public string Address { get; set; }
    public List<AgentServiceRegistration> Configurations { get; set; } = new();
}

