using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface IMovimentoEstoqueRepository
    {
        Task<IEnumerable<MovimentoEstoque>> GetAllAsync();
        Task<MovimentoEstoque?> GetByIdAsync(int id);
        Task<int> CreateAsync(MovimentoEstoque m);
        Task<bool> UpdateAsync(MovimentoEstoque m);
        Task<bool> DeleteAsync(int id);
    }
}