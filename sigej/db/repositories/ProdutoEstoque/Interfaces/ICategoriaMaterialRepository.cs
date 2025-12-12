using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface ICategoriaMaterialRepository
    {
        Task<IEnumerable<CategoriaMaterial>> GetAllAsync();
        Task<CategoriaMaterial?> GetByIdAsync(int id);
        Task<int> CreateAsync(CategoriaMaterial c);
        Task<bool> UpdateAsync(CategoriaMaterial c);
        Task<bool> DeleteAsync(int id);
    }
}