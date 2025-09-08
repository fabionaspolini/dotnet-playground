namespace database_load_playground.Entities;

public class Movimentacao
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal Valor { get; set; }
    public string? Descricao { get; set; }
}