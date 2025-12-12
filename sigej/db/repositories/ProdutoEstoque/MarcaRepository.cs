using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class MarcaRepository : IMarcaRepository
    {
        public async Task<IEnumerable<Marca>> GetAllAsync()
        {
            var list = new List<Marca>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome FROM marca ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Marca
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString()
                });
            }
            return list;
        }

        public async Task<Marca?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome FROM marca WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Marca
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Marca m)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO marca (nome) VALUES (@nome) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@nome", m.Nome);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(Marca m)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE marca SET nome = @nome WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@nome", m.Nome);
            cmd.Parameters.AddWithValue("@id", m.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM marca WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}