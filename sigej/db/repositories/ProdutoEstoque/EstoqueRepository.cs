using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class EstoqueRepository : IEstoqueRepository
    {
        public async Task<IEnumerable<Estoque>> GetAllAsync()
        {
            var list = new List<Estoque>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT produto_variacao_id, local_estoque_id, quantidade, ponto_reposicao FROM estoque", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Estoque
                {
                    ProdutoVariacaoId = Convert.ToInt32(reader["produto_variacao_id"]),
                    LocalEstoqueId = Convert.ToInt32(reader["local_estoque_id"]),
                    Quantidade = Convert.ToDecimal(reader["quantidade"]),
                    PontoReposicao = Convert.ToDecimal(reader["ponto_reposicao"])
                });
            }
            return list;
        }

        public async Task<Estoque?> GetByIdAsync(int produtoVariacaoId, int localEstoqueId)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT produto_variacao_id, local_estoque_id, quantidade, ponto_reposicao FROM estoque WHERE produto_variacao_id = @pvId AND local_estoque_id = @leId", conn);
            cmd.Parameters.AddWithValue("@pvId", produtoVariacaoId);
            cmd.Parameters.AddWithValue("@leId", localEstoqueId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Estoque
                {
                    ProdutoVariacaoId = Convert.ToInt32(reader["produto_variacao_id"]),
                    LocalEstoqueId = Convert.ToInt32(reader["local_estoque_id"]),
                    Quantidade = Convert.ToDecimal(reader["quantidade"]),
                    PontoReposicao = Convert.ToDecimal(reader["ponto_reposicao"])
                };
            }
            return null;
        }

        public async Task<bool> CreateAsync(Estoque e)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO estoque (produto_variacao_id, local_estoque_id, quantidade, ponto_reposicao) VALUES (@pvId, @leId, @qtd, @ponto)", conn);
            cmd.Parameters.AddWithValue("@pvId", e.ProdutoVariacaoId);
            cmd.Parameters.AddWithValue("@leId", e.LocalEstoqueId);
            cmd.Parameters.AddWithValue("@qtd", e.Quantidade);
            cmd.Parameters.AddWithValue("@ponto", e.PontoReposicao);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> UpdateAsync(Estoque e)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE estoque SET quantidade = @qtd, ponto_reposicao = @ponto WHERE produto_variacao_id = @pvId AND local_estoque_id = @leId", conn);
            cmd.Parameters.AddWithValue("@qtd", e.Quantidade);
            cmd.Parameters.AddWithValue("@ponto", e.PontoReposicao);
            cmd.Parameters.AddWithValue("@pvId", e.ProdutoVariacaoId);
            cmd.Parameters.AddWithValue("@leId", e.LocalEstoqueId);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int produtoVariacaoId, int localEstoqueId)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM estoque WHERE produto_variacao_id = @pvId AND local_estoque_id = @leId", conn);
            cmd.Parameters.AddWithValue("@pvId", produtoVariacaoId);
            cmd.Parameters.AddWithValue("@leId", localEstoqueId);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}