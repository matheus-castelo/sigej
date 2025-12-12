using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class CategoriaMaterialRepository : ICategoriaMaterialRepository
    {
        public async Task<IEnumerable<CategoriaMaterial>> GetAllAsync()
        {
            var list = new List<CategoriaMaterial>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome FROM categoria_material ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new CategoriaMaterial
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString()
                });
            }
            return list;
        }

        public async Task<CategoriaMaterial?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome FROM categoria_material WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CategoriaMaterial
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(CategoriaMaterial c)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO categoria_material (nome) VALUES (@nome) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@nome", c.Nome);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(CategoriaMaterial c)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE categoria_material SET nome = @nome WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@nome", c.Nome);
            cmd.Parameters.AddWithValue("@id", c.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM categoria_material WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}