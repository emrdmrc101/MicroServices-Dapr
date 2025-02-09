namespace Core.Domain.Consul;

public class ConsulServiceModel
{
    public string ServiceAddress { get; set; }
    public Dictionary<string, ServiceTaggedAddress> ServiceTaggedAddresses { get; set; } = new();
}

public class ServiceTaggedAddress
{
    public string Address { get; set; }

    public int Port { get; set; }
}