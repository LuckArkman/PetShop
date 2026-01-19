namespace DTOs;

public class Tenant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public string? DatabaseName { get; set; }
    public string? ApiKey { get; set; }
}
