using Microsoft.AspNetCore.Http;
using Interfaces;

namespace Api.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Aqui podemos validar se o Tenant existe no banco principal/config
        var tenantId = context.Request.Headers["X-Tenant-Id"].ToString();

        if (string.IsNullOrEmpty(tenantId))
        {
            // Opcional: Bloquear requisições sem Tenant ou usar o Default
            // context.Response.StatusCode = 400;
            // await context.Response.WriteAsync("Tenant ID is missing.");
            // return;
        }

        await _next(context);
    }
}
