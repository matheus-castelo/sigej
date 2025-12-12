using sigej.domain.models.LocalizacaoEEquipes;

namespace sigej.db.repositories.Equipes.Interfaces
{
    public interface IEquipeRepository
    {
        Task<IEnumerable<EquipeManutencao>> GetAllAsync();
        Task<EquipeManutencao?> GetByIdAsync(int id);
        Task<int> CreateAsync(EquipeManutencao e);
        Task<bool> UpdateAsync(EquipeManutencao e);
        Task<bool> DeleteAsync(int id);
    }
}