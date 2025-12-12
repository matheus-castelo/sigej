using sigej.domain.models.LocalizacaoEEquipes;

namespace sigej.db.repositories.Equipes.Interfaces
{
    public interface IEquipeMembroRepository
    {
        Task<IEnumerable<EquipeMembro>> GetAllAsync();
        Task<EquipeMembro?> GetByIdAsync(int id);
        Task<int> CreateAsync(EquipeMembro m);
        Task<bool> UpdateAsync(EquipeMembro m);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<EquipeMembro>> GetByEquipeIdAsync(int equipeId);
    }
}