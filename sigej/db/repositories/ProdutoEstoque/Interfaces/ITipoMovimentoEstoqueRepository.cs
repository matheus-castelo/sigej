using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface ITipoMovimentoEstoqueRepository
    {
        Task<IEnumerable<TipoMovimentoEstoque>> GetAllAsync();
        Task<TipoMovimentoEstoque?> GetByIdAsync(int id);
        Task<int> CreateAsync(TipoMovimentoEstoque t);
        Task<bool> UpdateAsync(TipoMovimentoEstoque t);
        Task<bool> DeleteAsync(int id);
    }
}