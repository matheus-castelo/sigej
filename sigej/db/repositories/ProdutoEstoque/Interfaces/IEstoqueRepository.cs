using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface IEstoqueRepository
    {
        Task<IEnumerable<Estoque>> GetAllAsync();
        Task<Estoque?> GetByIdAsync(int produtoVariacaoId, int localEstoqueId);
        Task<bool> CreateAsync(Estoque e);
        Task<bool> UpdateAsync(Estoque e);
        Task<bool> DeleteAsync(int produtoVariacaoId, int localEstoqueId);
    }
}