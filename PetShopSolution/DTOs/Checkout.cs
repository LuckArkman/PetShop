namespace DTOs;

public class Checkout
{
    public string consultaId { get; set; }
    public string status { get; set; } // "Pending", "Paid", "Canceled"
}