namespace sigej.db.models.Estoque
{
    public class Estoque
    {
        public int ProdutoVariacaoId { get; set; } 
        public int LocalEstoqueId { get; set; } 
        public decimal Quantidade { get; set; }
        public decimal PontoReposicao { get; set; }
    }
}