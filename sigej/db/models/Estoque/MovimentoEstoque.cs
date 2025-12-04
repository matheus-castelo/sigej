using System;

namespace sigej.db.models.Estoque
{
    public class MovimentoEstoque
    {
        public int Id { get; set; }
        public int ProdutoVariacaoId { get; set; } 
        public int LocalEstoqueId { get; set; } 
        public int TipoMovimentoId { get; set; } 
        public decimal Quantidade { get; set; }
        public DateTime DataHora { get; set; }
        public int? FuncionarioId { get; set; } 
        public int? OrdemServicoId { get; set; }
        public string Observacao { get; set; }
    }
}