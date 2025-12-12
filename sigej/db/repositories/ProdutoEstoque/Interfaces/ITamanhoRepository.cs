using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface ITamanhoRepository
    {
        Task<IEnumerable<Tamanho>> GetAllAsync();
        Task<Tamanho?> GetByIdAsync(int id);
        Task<int> CreateAsync(Tamanho t);
        Task<bool> UpdateAsync(Tamanho t);
        Task<bool> DeleteAsync(int id);
    }
}