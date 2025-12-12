using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface IUnidadeMedidaRepository
    {
        Task<IEnumerable<UnidadeMedida>> GetAllAsync();
        Task<UnidadeMedida?> GetByIdAsync(int id);
        Task<int> CreateAsync(UnidadeMedida u);
        Task<bool> UpdateAsync(UnidadeMedida u);
        Task<bool> DeleteAsync(int id);
    }
}