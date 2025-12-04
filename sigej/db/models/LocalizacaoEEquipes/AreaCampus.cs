namespace sigej.db.models.LocalizacaoEEquipes
{
    public class AreaCampus
    {
        public int Id { get; set; }
        public int TipoAreaId { get; set; } 
        public string Descricao { get; set; }
        public string Bloco { get; set; }
    }
}