namespace PetShop.API.interfaces;

public interface IIHttpContextAccessor
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken);
}