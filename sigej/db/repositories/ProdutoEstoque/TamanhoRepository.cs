using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class TamanhoRepository : ITamanhoRepository
    {
        public async Task<IEnumerable<Tamanho>> GetAllAsync()
        {
            var list = new List<Tamanho>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM tamanho ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Tamanho
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                });
            }
            return list;
        }

        public async Task<Tamanho?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM tamanho WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Tamanho
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Tamanho t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO tamanho (descricao) VALUES (@desc) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(Tamanho t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE tamanho SET descricao = @desc WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);
            cmd.Parameters.AddWithValue("@id", t.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM tamanho WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}