namespace sigej.db.models.ProdutosEMateriais
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int? CategoriaId { get; set; } 
        public int? UnidadeMedidaId { get; set; } 
        public int? MarcaId { get; set; } 
    }
}