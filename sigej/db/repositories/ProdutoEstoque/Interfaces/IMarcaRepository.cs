using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface IMarcaRepository
    {
        Task<IEnumerable<Marca>> GetAllAsync();
        Task<Marca?> GetByIdAsync(int id);
        Task<int> CreateAsync(Marca m);
        Task<bool> UpdateAsync(Marca m);
        Task<bool> DeleteAsync(int id);
    }
}