using System;

namespace sigej.db.models.OS
{
    public class AndamentoOrdemServico
    {
        public int Id { get; set; }
        public int OsId { get; set; } 
        public DateTime DataHora { get; set; }
        public int? StatusAnteriorId { get; set; }
        public int? StatusNovoId { get; set; }
        public int? FuncionarioId { get; set; } 
        public string Descricao { get; set; }
        public DateTime? InicioAtendimento { get; set; }
        public DateTime? FimAtendimento { get; set; }
    }
}