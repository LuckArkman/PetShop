using DTOs;
using Enums;
using Interfaces;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace PetShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatewayController : ControllerBase
{
    private readonly IConfiguration _cfg;
    private readonly ICaixaService _service;
    public GatewayController(ICaixaService service,
        IConfiguration cfg)
    {
        _service = service;
        _cfg = cfg;
    }
    
    [HttpPost("CreatePixCode")]
    public async Task<IActionResult> CreatePixCode (CreatePixPaymentRequest request)
    {
        var paymentClient = new PaymentClient();

        var paymentRequest = new PaymentCreateRequest
        {
            TransactionAmount = request.Amount,
            Description = request.Description,
            PaymentMethodId = "pix",
            ExternalReference = request.ExternalReference,
            Payer = new PaymentPayerRequest
            {
                Email = request.mail
            }
        };

        Payment payment = await paymentClient.CreateAsync(paymentRequest);
        return Ok(payment);
    }
    
    [HttpPost("CreatePaidCard")]
    public async Task<IActionResult> CreatePaidCard(CreateCardPaymentRequest request)
    {
        var paymentClient = new PaymentClient();

        var paymentRequest = new PaymentCreateRequest
        {
            TransactionAmount = request.Amount,
            Description = request.Description,
            PaymentMethodId = request.PaymentMethodId,
            Token = request.CardToken,
            Installments = request.Installments,
            ExternalReference = request.ExternalReference,
            Payer = new PaymentPayerRequest
            {
                Email = request.PayerEmail
            }
        };

        Payment payment = await paymentClient.CreateAsync(paymentRequest);

        return Ok(new
        {
            payment.Id,
            payment.Status,
            payment.StatusDetail,
            payment.ExternalReference
        });
    }
    
    [HttpPost("webhook")]
    public async Task<IActionResult> webhook(HttpRequest httpRequest)
    {
        var topic = httpRequest.Query["topic"].ToString();
        var id = httpRequest.Query["id"].ToString();

        if (topic == "payment" && long.TryParse(id, out var paymentId))
        {
            var paymentClient = new PaymentClient();
            var payment = await paymentClient.GetAsync(paymentId);
            Console.WriteLine($"Pagamento {payment.Id} - Status: {payment.Status}");
            long _id = long.Parse(id);

            var update = _service.UpdateStatusWebhook(_id, PaidStatus.Complete, CancellationToken.None);
            if(update != null)return Ok();
        }

        return BadRequest();
    }

}