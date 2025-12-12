namespace sigej.domain.dtos;

public class RelatorioEstoqueDto
{
    public string Produto { get; set; }
    public int VariacaoId { get; set; }
    public string Local { get; set; }
    public decimal QuantidadeAtual { get; set; }
    public decimal PontoReposicao { get; set; }
}