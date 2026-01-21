using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public TenantService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public string? GetCurrentTenantId()
    {
        // Obtém o Tenant ID do Header da requisição
        var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].ToString();
        return string.IsNullOrEmpty(tenantId) ? "Default" : tenantId;
    }

    public string? GetDatabaseName()
    {
        var tenantId = GetCurrentTenantId();

        // Aqui poderíamos ter um dicionário ou consulta ao banco 'admin' para mapear tenantId -> dbName
        // Por simplicidade, usaremos o tenantId como parte do nome do banco ou retornaremos o configurado se for "Default"

        if (tenantId == "Default")
        {
            return _configuration["MongoDbSettings:DataBaseName"];
        }

        // Exemplo: PetShop_TenantA, PetShop_TenantB
        return $"PetShop_{tenantId}";
    }
}
