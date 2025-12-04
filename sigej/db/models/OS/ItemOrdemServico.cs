namespace sigej.db.models.OS
{
    public class ItemOrdemServico
    {
        public int Id { get; set; }
        public int OsId { get; set; } 
        public int ProdutoVariacaoId { get; set; } 
        public decimal QuantidadePrevista { get; set; }
        public decimal QuantidadeUsada { get; set; }
    }
}