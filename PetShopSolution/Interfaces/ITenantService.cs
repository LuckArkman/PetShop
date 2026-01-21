namespace Interfaces;

public interface ITenantService
{
    string? GetCurrentTenantId();
    string? GetDatabaseName();
}
