using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.AreaCampus.Interfaces
{
    public interface ITipoAreaCampusRepository
    {
        Task<IEnumerable<TipoAreaCampus>> GetAllAsync();
        Task<TipoAreaCampus?> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoAreaCampus t);
        Task<bool> UpdateAsync(TipoAreaCampus t);
        Task<bool> DeleteAsync(int id);
    }
}