using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class LocalEstoqueRepository : ILocalEstoqueRepository
    {
        public async Task<IEnumerable<LocalEstoque>> GetAllAsync()
        {
            var list = new List<LocalEstoque>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao, responsavel_id FROM local_estoque ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new LocalEstoque
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString(),
                    ResponsavelId = reader["responsavel_id"] != DBNull.Value ? Convert.ToInt32(reader["responsavel_id"]) : null
                });
            }
            return list;
        }

        public async Task<LocalEstoque?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao, responsavel_id FROM local_estoque WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new LocalEstoque
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString(),
                    ResponsavelId = reader["responsavel_id"] != DBNull.Value ? Convert.ToInt32(reader["responsavel_id"]) : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(LocalEstoque l)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO local_estoque (descricao, responsavel_id) VALUES (@descricao, @respId) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@descricao", l.Descricao);
            cmd.Parameters.AddWithValue("@respId", l.ResponsavelId ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(LocalEstoque l)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE local_estoque SET descricao = @descricao, responsavel_id = @respId WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@descricao", l.Descricao);
            cmd.Parameters.AddWithValue("@respId", l.ResponsavelId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", l.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM local_estoque WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}