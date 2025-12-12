namespace sigej.db.repositories.AreaCampus.Interfaces
{
    public interface IAreaCampusRepository
    {
        Task<IEnumerable<domain.models.LocalizacaoEEquipes.AreaCampus>> GetAllAsync();
        Task<domain.models.LocalizacaoEEquipes.AreaCampus?> GetByIdAsync(int id);
        Task<int> CreateAsync(domain.models.LocalizacaoEEquipes.AreaCampus a);
        Task<bool> UpdateAsync(domain.models.LocalizacaoEEquipes.AreaCampus a);
        Task<bool> DeleteAsync(int id);
    }
}