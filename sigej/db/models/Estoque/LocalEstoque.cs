namespace sigej.db.models.Estoque
{
    public class LocalEstoque
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int? ResponsavelId { get; set; }
    }
}