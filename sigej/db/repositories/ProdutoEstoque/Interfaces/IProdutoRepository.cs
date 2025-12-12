using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<Produto?> GetByIdAsync(int id);
        Task<int> CreateAsync(Produto p);
        Task<bool> UpdateAsync(Produto p);
        Task<bool> DeleteAsync(int id);
    }
}