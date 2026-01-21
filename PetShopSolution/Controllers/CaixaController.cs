using Microsoft.AspNetCore.Mvc;
using DTOs;
using Enums;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Services;

namespace PetShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaixaController : ControllerBase
{
    private readonly AgendamentoService _service;
    private readonly IResponsavelService _responsavelService;
    private IPaymentGateway _paymentGateway;
    private readonly IConfiguration _cfg;
    private readonly IRepositorio<Order> _orderRepo;

    public CaixaController(IConfiguration connection,
        AgendamentoService service,
        IResponsavelService responsavelService,
        IPaymentGateway paymentGateway,
        IRepositorio<Order> orderRepo)
    {
        _cfg = connection;
        _service = service;
        _responsavelService = responsavelService;
        _paymentGateway = paymentGateway;
        _orderRepo = orderRepo;
        _service.InitializeCollection(null, null, "Agendamento");
        _responsavelService.InitializeCollection(null, null, "Responsavel");
        _orderRepo.InitializeCollection(null, null, "Orders");
    }
    [HttpGet("GetByIdAsync/{id}")]
    public async Task<IActionResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var pg = await _orderRepo.GetByIdOrderAsync(id);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);

    }
    [HttpGet("GetPagamentosCompletosDoDiaAsync/{dataConsulta}")]
    public async Task<IActionResult> GetPagamentosCompletosDoDiaAsync(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var hoje = dataConsulta.Date;
        var pg = await _orderRepo.GetAllTodayPaidsCompletes(hoje, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpGet("GetPagamentosPendentesDoDiaAsync/{dataConsulta}")]
    public async Task<IActionResult> GetPagamentosPendentesDoDiaAsync(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var hoje = dataConsulta.Date;
        var pg = await _orderRepo.GetAllTodayPaidsPending(hoje, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpGet("GetPagamentosCanceladosDoDiaAsync/{dataConsulta}")]
    public async Task<IActionResult> GetPagamentosCanceladosDoDiaAsync(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var hoje = dataConsulta.Date;
        var pg = await _orderRepo.GetAllTodayPaidsCanceled(hoje, cancellationToken);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }
    [HttpGet("GetByClienteCpfAsync/{cpf}")]
    public async Task<IActionResult> GetByClienteCpfAsync(string cpf, CancellationToken cancellationToken)
    {
        var responsavel = await _responsavelService.GetResponsavelRg(cpf);
        if (responsavel == null) return new NotFoundResult();
        var pg = await _orderRepo.GetByUserIdOrderAsync(responsavel.Id);
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }

    [HttpPost("UpdateStatusAsync")]
    public async Task<IActionResult> UpdateStatusAsync(string id, PaidStatus status, CancellationToken cancellationToken)
    {
        var pg = await _orderRepo.Update(id, status.ToString());
        if (pg == null) return new NotFoundResult();
        return Ok(pg);
    }

    [HttpPost]
    [Route("Payment/ProcessCheckout")]
    public async Task<IActionResult> ProcessCheckout([FromBody] CheckoutWithCpf request)
    {
        var _agendamento = await _service.GetById(request.consultaId, CancellationToken.None);
        if (_agendamento == null) return BadRequest(new { success = false, message = "O agendamento não foi encontrado." });
        Responsavel? _responsavel = await _responsavelService.GetResponsavelRg(_agendamento.rg);
        if (_responsavel == null) return BadRequest(new { success = false, message = "O responsável pelo animal não foi localizado através do RG informado." });
        try
        {
            if (request.valor <= 0) return BadRequest(new { success = false, message = "Valor inválido para pagamento." });
            var order = new Order
            {
                id = request.consultaId,
                UserId = _responsavel.Id,
                transacao = "Consulta",
                TotalAmount = request.valor,
                PaymentMethod = request.PaymentMethod,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            PaymentResponse? paymentResult = null;
            if (request.PaymentMethod == "pix") paymentResult = await _paymentGateway.CreatePaymentAsync(order, _responsavel, request.PaymentMethod);

            if (!paymentResult.Success && paymentResult != null)
            {
                return BadRequest(new { success = false, message = paymentResult.Message });
            }
            if (paymentResult != null) order.TransactionId = paymentResult.TransactionId;
            if (paymentResult == null) order.TransactionId = Guid.NewGuid().ToString();
            if (paymentResult.Details != null)
            {
                order.PaymentData = new PaymentMetadata
                {
                    // Dados do PIX
                    PixQrCode = paymentResult.Details.PixQrCode,
                    PixQrCodeBase64 = paymentResult.Details.PixQrCodeImage,
                    ExpirationDate = paymentResult.Details.PixExpirationDate,

                    // Dados do Boleto
                    BoletoBarcode = paymentResult.Details.BoletoBarCode,
                    BoletoPdfUrl = paymentResult.Details.BoletoPdfUrl,
                    BoletoDueDate = paymentResult.Details.BoletoDueDate
                };
            }

            await _orderRepo.InsertOneAsync(order);

            string redirectUrl = "";
            return Ok(new
            {
                success = true,
                message = "Ordem de Pagamento gerado com sucesso",
                transactionId = order.TransactionId,
                paymentData = order.PaymentData, // Isso imprimirá o QR Code e Barcode no Swagger
                redirectUrl = redirectUrl
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Erro interno: " + ex.Message });
        }
    }

    [HttpPost]
    [Route("Payment/CompleteCheckout")]
    public async Task<IActionResult> CompleteCheckout([FromBody] Checkout request)
    {
        try
        {
            if (request == null) return BadRequest(new { success = false, message = "Valor inválido para pagamento." });

            // Chama o Gateway com o valor correto
            var paymentResult = await _orderRepo.Update(request.consultaId, request.status);

            if (paymentResult == null)
            {
                return BadRequest("Devido a um erro interno, não foi possível concluir a atualização da ordem de pagamento. Tente novamente mais tarde.");
            }
            return Ok(paymentResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Erro interno: " + ex.Message });
        }
    }

    [HttpGet]
    [Route("Checkout/{id}")]
    public async Task<IActionResult> CompleteCheckout(string id)
    {
        try
        {
            var paymentResult = await _orderRepo.GetByIdOrderAsync(id);
            return Ok(paymentResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Erro interno: " + ex.Message });
        }
    }
    [HttpDelete("DeleteAsync/{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var pg = await _orderRepo.Delete(id, cancellationToken);
        if (pg) return new NotFoundResult();
        return Ok(pg);
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> ReceiveNotification([FromBody] MercadoPagoWebhookRequest notification)
    {
        try
        {
            if (notification.Type == "payment" || notification.Topic == "payment")
            {
                var paymentId = notification.Data?.Id;
                if (string.IsNullOrEmpty(paymentId)) return Ok();

                var paymentInfo = await _paymentGateway.GetPaymentAsync(paymentId);

                var order = await _orderRepo.GetByTransectionOrderAsync(paymentId);

                string newStatus = paymentInfo.Message.ToLower() switch
                {
                    "approved" => "Paid",
                    "authorized" => "Pending",
                    "in_process" => "Pending",
                    "pending" => "Pending",
                    "rejected" => "Failed",
                    "cancelled" => "Canceled",
                    "refunded" => "Refunded",
                    "charged_back" => "ChargedBack",
                    _ => order.Status
                };
                var update = await _orderRepo.Update(order.id, newStatus);
                if (update == null) return BadRequest();
                return Ok();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
}