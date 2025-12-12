using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico.Interfaces
{
    public interface IStatusOrdemServicoRepository
    {
        Task<IEnumerable<StatusOrdemServico>> GetAllAsync();
        Task<StatusOrdemServico?> GetByIdAsync(int id);
        Task<int> CreateAsync(StatusOrdemServico s);
        Task<bool> UpdateAsync(StatusOrdemServico s);
        Task<bool> DeleteAsync(int id);
    }
}