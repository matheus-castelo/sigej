using System;

namespace sigej.db.models.PessoasEEstrutura
{
    public class Funcionario
    {
        public int Id { get; set; }
        public int PessoaId { get; set; } 
        public int TipoFuncionarioId { get; set; } 
        public int SetorId { get; set; } 
        public DateTime? DataAdmissao { get; set; } 
        public DateTime? DataDemissao { get; set; } 
    }
}