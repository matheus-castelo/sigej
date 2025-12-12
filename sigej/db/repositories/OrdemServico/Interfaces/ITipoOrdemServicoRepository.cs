using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico.Interfaces
{
    public interface ITipoOrdemServicoRepository
    {
        Task<IEnumerable<TipoOrdemServico>> GetAllAsync();
        Task<TipoOrdemServico?> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoOrdemServico t);
        Task<bool> UpdateAsync(TipoOrdemServico t);
        Task<bool> DeleteAsync(int id);
    }
}