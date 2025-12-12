using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.OrdemServico.Interfaces;
using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico
{
    public class TipoOrdemServicoRepository : ITipoOrdemServicoRepository
    {
        public async Task<IEnumerable<TipoOrdemServico>> GetAllAsync()
        {
            var list = new List<TipoOrdemServico>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM tipo_ordem_servico ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new TipoOrdemServico
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                });
            }
            return list;
        }

        public async Task<TipoOrdemServico?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM tipo_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TipoOrdemServico
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(TipoOrdemServico t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO tipo_ordem_servico (descricao) VALUES (@desc) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(TipoOrdemServico t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE tipo_ordem_servico SET descricao = @desc WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);
            cmd.Parameters.AddWithValue("@id", t.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM tipo_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}