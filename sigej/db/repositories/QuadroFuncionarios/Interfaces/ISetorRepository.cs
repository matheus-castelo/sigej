using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios.Interfaces
{
    public interface ISetorRepository
    {
        Task<IEnumerable<Setor>> GetAllAsync(Setor model);
        Task<Setor?> GetByIdAsync(int id);
        Task<int> CreateAsync(Setor model);
        Task<bool> UpdateAsync(Setor model);
        Task<bool> DeleteAsync(int id);
    }
}