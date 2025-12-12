using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico.Interfaces
{
    public interface IAndamentoOrdemServicoRepository
    {
        Task<IEnumerable<AndamentoOrdemServico>> GetAllAsync();
        Task<AndamentoOrdemServico?> GetByIdAsync(int id);
        Task<int> CreateAsync(AndamentoOrdemServico a);
        Task<bool> UpdateAsync(AndamentoOrdemServico a);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AndamentoOrdemServico>> GetByOrdemServicoIdAsync(int osId);
    }
}