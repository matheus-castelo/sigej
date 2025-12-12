using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class CorRepository : ICorRepository
    {
        public async Task<IEnumerable<Cor>> GetAllAsync()
        {
            var list = new List<Cor>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome FROM cor ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Cor
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString()
                });
            }
            return list;
        }

        public async Task<Cor?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome FROM cor WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Cor
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Cor c)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO cor (nome) VALUES (@nome) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@nome", c.Nome);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(Cor c)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE cor SET nome = @nome WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@nome", c.Nome);
            cmd.Parameters.AddWithValue("@id", c.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM cor WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}