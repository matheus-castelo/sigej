namespace sigej.domain.dtos;

public class RelatorioConsumoEquipeDto
{
    public string Equipe { get; set; }
    public string Produto { get; set; }
    public int VariacaoId { get; set; }
    public decimal TotalConsumido { get; set; }
}