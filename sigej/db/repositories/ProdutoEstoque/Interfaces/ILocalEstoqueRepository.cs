using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface ILocalEstoqueRepository
    {
        Task<IEnumerable<LocalEstoque>> GetAllAsync();
        Task<LocalEstoque?> GetByIdAsync(int id);
        Task<int> CreateAsync(LocalEstoque l);
        Task<bool> UpdateAsync(LocalEstoque l);
        Task<bool> DeleteAsync(int id);
    }
}