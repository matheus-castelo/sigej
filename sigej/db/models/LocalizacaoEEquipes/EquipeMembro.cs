namespace sigej.db.models.LocalizacaoEEquipes
{
    public class EquipeMembro
    {
        public int Id { get; set; }
        public int EquipeId { get; set; } 
        public int FuncionarioId { get; set; } 
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; } 
        public string Funcao { get; set; }
    }
}