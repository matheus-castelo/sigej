using System;

namespace sigej.db.models.OS
{
    public class OrdemServico
    {
        public int Id { get; set; }
        public string NumeroSequencial { get; set; }
        public int SolicitanteId { get; set; } 
        public int AreaCampusId { get; set; } 
        public int TipoOsId { get; set; } 
        public int? EquipeId { get; set; } 
        public int? LiderId { get; set; } 
        public int StatusId { get; set; } 
        public int Prioridade { get; set; } 
        public DateTime DataAbertura { get; set; }
        public DateTime? DataPrevista { get; set; }
        public string DescricaoProblema { get; set; }
    }
}