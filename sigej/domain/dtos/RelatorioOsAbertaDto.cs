namespace sigej.domain.dtos;

public class RelatorioOsAbertaDto
{
    public int Id { get; set; }
    public string NumeroSequencial { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataAbertura { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Equipe { get; set; } = string.Empty;
}