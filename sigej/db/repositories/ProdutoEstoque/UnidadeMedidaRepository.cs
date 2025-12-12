using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class UnidadeMedidaRepository : IUnidadeMedidaRepository
    {
        public async Task<IEnumerable<UnidadeMedida>> GetAllAsync()
        {
            var list = new List<UnidadeMedida>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, sigla, descricao FROM unidade_medida ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new UnidadeMedida
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Sigla = reader["sigla"].ToString(),
                    Descricao = reader["descricao"] != DBNull.Value ? reader["descricao"].ToString() : null
                });
            }
            return list;
        }

        public async Task<UnidadeMedida?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, sigla, descricao FROM unidade_medida WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UnidadeMedida
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Sigla = reader["sigla"].ToString(),
                    Descricao = reader["descricao"] != DBNull.Value ? reader["descricao"].ToString() : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(UnidadeMedida u)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO unidade_medida (sigla, descricao) VALUES (@sigla, @desc) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@sigla", u.Sigla);
            cmd.Parameters.AddWithValue("@desc", u.Descricao ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(UnidadeMedida u)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE unidade_medida SET sigla = @sigla, descricao = @desc WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@sigla", u.Sigla);
            cmd.Parameters.AddWithValue("@desc", u.Descricao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", u.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM unidade_medida WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}