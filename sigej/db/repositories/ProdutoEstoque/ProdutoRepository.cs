using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class ProdutoRepository : IProdutoRepository
    {
        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            var list = new List<Produto>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao, categoria_id, unidade_medida_id, marca_id FROM produto ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Produto
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString(),
                    CategoriaId = reader["categoria_id"] != DBNull.Value ? Convert.ToInt32(reader["categoria_id"]) : null,
                    UnidadeMedidaId = reader["unidade_medida_id"] != DBNull.Value ? Convert.ToInt32(reader["unidade_medida_id"]) : null,
                    MarcaId = reader["marca_id"] != DBNull.Value ? Convert.ToInt32(reader["marca_id"]) : null
                });
            }
            return list;
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao, categoria_id, unidade_medida_id, marca_id FROM produto WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Produto
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString(),
                    CategoriaId = reader["categoria_id"] != DBNull.Value ? Convert.ToInt32(reader["categoria_id"]) : null,
                    UnidadeMedidaId = reader["unidade_medida_id"] != DBNull.Value ? Convert.ToInt32(reader["unidade_medida_id"]) : null,
                    MarcaId = reader["marca_id"] != DBNull.Value ? Convert.ToInt32(reader["marca_id"]) : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Produto p)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO produto (descricao, categoria_id, unidade_medida_id, marca_id) VALUES (@desc, @catId, @unidId, @marcaId) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@desc", p.Descricao);
            cmd.Parameters.AddWithValue("@catId", p.CategoriaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@unidId", p.UnidadeMedidaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@marcaId", p.MarcaId ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(Produto p)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE produto SET descricao = @desc, categoria_id = @catId, unidade_medida_id = @unidId, marca_id = @marcaId WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@desc", p.Descricao);
            cmd.Parameters.AddWithValue("@catId", p.CategoriaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@unidId", p.UnidadeMedidaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@marcaId", p.MarcaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", p.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM produto WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}