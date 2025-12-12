using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios.Interfaces
{
    public interface IPessoaRepository
    {
        Task<IEnumerable<Pessoa>> GetAllAsync(Pessoa model);
        Task<Pessoa?> GetByIdAsync(int id);
        Task<int> CreateAsync(Pessoa model);
        Task<bool> UpdateAsync(Pessoa model);
        Task<bool> DeleteAsync(int id);
    }
}