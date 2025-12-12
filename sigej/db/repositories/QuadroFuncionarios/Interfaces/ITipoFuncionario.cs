using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios.Interfaces
{
    public interface ITipoFuncionarioRepository
    {
        Task<IEnumerable<TipoFuncionario>> GetAllAsync(TipoFuncionario model);
        Task<TipoFuncionario?> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoFuncionario model);
        Task<bool> UpdateAsync(TipoFuncionario model);
        Task<bool> DeleteAsync(int id);
    }
}