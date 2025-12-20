namespace DTOs;

public class CheckoutWithCpf
{
    public string consultaId { get; set; }
    public decimal valor { get; set; }
    public string PaymentMethod { get; set; } //"pix", "boleto", "cartao" 
}