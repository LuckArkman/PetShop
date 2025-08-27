using System.Net.Http.Headers;
using PetShop.API.interfaces;

namespace PetShop.API.Handlers;

public class JwtAuthHandler : DelegatingHandler, IIHttpContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtAuthHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
            
        var jwtToken = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken"); // Exemplo se o token estiver na sessão

        if (!string.IsNullOrEmpty(jwtToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}