using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface ICorRepository
    {
        Task<IEnumerable<Cor>> GetAllAsync();
        Task<Cor?> GetByIdAsync(int id);
        Task<int> CreateAsync(Cor c);
        Task<bool> UpdateAsync(Cor c);
        Task<bool> DeleteAsync(int id);
    }
}