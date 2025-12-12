using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque.Interfaces
{
    public interface IProdutoVariacaoRepository
    {
        Task<IEnumerable<ProdutoVariacao>> GetAllAsync();
        Task<ProdutoVariacao?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProdutoVariacao pv);
        Task<bool> UpdateAsync(ProdutoVariacao pv);
        Task<bool> DeleteAsync(int id);
    }
}