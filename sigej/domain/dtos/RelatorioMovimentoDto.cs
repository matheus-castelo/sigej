namespace sigej.domain.dtos;

public class RelatorioMovimentoDto
{
    public int Id { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int VariacaoId { get; set; }
    public string Local { get; set; } = string.Empty;
    public string TipoMovimento { get; set; } = string.Empty;
    public char Sinal { get; set; }
    public decimal Quantidade { get; set; }
    public DateTime DataHora { get; set; }
    public int? FuncionarioId { get; set; }
    
    public string? NumeroOrdemServico { get; set; } 
}