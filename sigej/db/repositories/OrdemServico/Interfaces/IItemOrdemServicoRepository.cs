using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico.Interfaces
{
    public interface IItemOrdemServicoRepository
    {
        Task<IEnumerable<ItemOrdemServico>> GetAllAsync();
        Task<ItemOrdemServico?> GetByIdAsync(int id);
        Task<IEnumerable<ItemOrdemServico>> GetByOsIdAsync(int osId);
        Task<int> CreateAsync(ItemOrdemServico i);
        Task<bool> UpdateAsync(ItemOrdemServico i);
        Task<bool> DeleteAsync(int id);
    }
}