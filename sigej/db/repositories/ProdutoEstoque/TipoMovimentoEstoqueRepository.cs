using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class TipoMovimentoEstoqueRepository : ITipoMovimentoEstoqueRepository
    {
        public async Task<IEnumerable<TipoMovimentoEstoque>> GetAllAsync()
        {
            var list = new List<TipoMovimentoEstoque>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao, sinal FROM tipo_movimento_estoque ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new TipoMovimentoEstoque
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString(),
                    Sinal = Convert.ToChar(reader["sinal"])
                });
            }
            return list;
        }

        public async Task<TipoMovimentoEstoque?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao, sinal FROM tipo_movimento_estoque WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TipoMovimentoEstoque
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString(),
                    Sinal = Convert.ToChar(reader["sinal"])
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(TipoMovimentoEstoque t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO tipo_movimento_estoque (descricao, sinal) VALUES (@desc, @sinal) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);
            cmd.Parameters.AddWithValue("@sinal", t.Sinal);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(TipoMovimentoEstoque t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE tipo_movimento_estoque SET descricao = @desc, sinal = @sinal WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);
            cmd.Parameters.AddWithValue("@sinal", t.Sinal);
            cmd.Parameters.AddWithValue("@id", t.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM tipo_movimento_estoque WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}