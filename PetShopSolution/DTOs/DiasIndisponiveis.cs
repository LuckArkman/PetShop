namespace DTOs;

public class DiasIndisponiveis
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime Data { get; set; }
    public string? Motivo { get; set; }

    public DiasIndisponiveis(DateTime data, string? motivo)
    {
        Data = data;
        Motivo = motivo;
    }
}