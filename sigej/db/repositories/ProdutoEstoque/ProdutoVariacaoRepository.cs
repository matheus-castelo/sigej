using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.ProdutosEMateriais;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class ProdutoVariacaoRepository : IProdutoVariacaoRepository
    {
        public async Task<IEnumerable<ProdutoVariacao>> GetAllAsync()
        {
            var list = new List<ProdutoVariacao>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, produto_id, cor_id, tamanho_id, codigo_barras, codigo_interno FROM produto_variacao ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ProdutoVariacao
                {
                    Id = Convert.ToInt32(reader["id"]),
                    ProdutoId = Convert.ToInt32(reader["produto_id"]),
                    CorId = reader["cor_id"] != DBNull.Value ? Convert.ToInt32(reader["cor_id"]) : null,
                    TamanhoId = reader["tamanho_id"] != DBNull.Value ? Convert.ToInt32(reader["tamanho_id"]) : null,
                    CodigoBarras = reader["codigo_barras"] != DBNull.Value ? reader["codigo_barras"].ToString() : null,
                    CodigoInterno = reader["codigo_interno"] != DBNull.Value ? reader["codigo_interno"].ToString() : null
                });
            }
            return list;
        }

        public async Task<ProdutoVariacao?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, produto_id, cor_id, tamanho_id, codigo_barras, codigo_interno FROM produto_variacao WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ProdutoVariacao
                {
                    Id = Convert.ToInt32(reader["id"]),
                    ProdutoId = Convert.ToInt32(reader["produto_id"]),
                    CorId = reader["cor_id"] != DBNull.Value ? Convert.ToInt32(reader["cor_id"]) : null,
                    TamanhoId = reader["tamanho_id"] != DBNull.Value ? Convert.ToInt32(reader["tamanho_id"]) : null,
                    CodigoBarras = reader["codigo_barras"] != DBNull.Value ? reader["codigo_barras"].ToString() : null,
                    CodigoInterno = reader["codigo_interno"] != DBNull.Value ? reader["codigo_interno"].ToString() : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(ProdutoVariacao pv)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO produto_variacao (produto_id, cor_id, tamanho_id, codigo_barras, codigo_interno) VALUES (@prodId, @corId, @tamId, @barras, @interno) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@prodId", pv.ProdutoId);
            cmd.Parameters.AddWithValue("@corId", pv.CorId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tamId", pv.TamanhoId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@barras", pv.CodigoBarras ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@interno", pv.CodigoInterno ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(ProdutoVariacao pv)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE produto_variacao SET produto_id = @prodId, cor_id = @corId, tamanho_id = @tamId, codigo_barras = @barras, codigo_interno = @interno WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@prodId", pv.ProdutoId);
            cmd.Parameters.AddWithValue("@corId", pv.CorId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tamId", pv.TamanhoId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@barras", pv.CodigoBarras ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@interno", pv.CodigoInterno ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", pv.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM produto_variacao WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}