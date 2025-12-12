using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios.Interfaces
{
    public interface IFuncionarioRepository
    {
        Task<IEnumerable<Funcionario>> GetAllAsync(Funcionario model);
        Task<Funcionario?> GetByIdAsync(int id);
        Task<int> CreateAsync(Funcionario model);
        Task<bool> UpdateAsync(Funcionario model);
        Task<bool> DeleteAsync(int id);
    }
}