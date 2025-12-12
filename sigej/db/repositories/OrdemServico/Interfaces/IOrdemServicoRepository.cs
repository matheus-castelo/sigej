namespace sigej.db.repositories.OrdemServico.Interfaces
{
    public interface IOrdemServicoRepository
    {
        Task<IEnumerable<domain.models.OS.OrdemServico>> GetAllAsync();
        Task<domain.models.OS.OrdemServico?> GetByIdAsync(int id);
        Task<int> CreateAsync(domain.models.OS.OrdemServico o);
        Task<bool> UpdateAsync(domain.models.OS.OrdemServico o);
        Task<bool> DeleteAsync(int id);
    }
}