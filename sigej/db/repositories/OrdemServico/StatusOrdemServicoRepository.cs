using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.OrdemServico.Interfaces;
using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico
{
    public class StatusOrdemServicoRepository : IStatusOrdemServicoRepository
    {
        public async Task<IEnumerable<StatusOrdemServico>> GetAllAsync()
        {
            var list = new List<StatusOrdemServico>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM status_ordem_servico ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new StatusOrdemServico
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                });
            }
            return list;
        }

        public async Task<StatusOrdemServico?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM status_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new StatusOrdemServico
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(StatusOrdemServico s)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO status_ordem_servico (descricao) VALUES (@desc) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@desc", s.Descricao);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(StatusOrdemServico s)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE status_ordem_servico SET descricao = @desc WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@desc", s.Descricao);
            cmd.Parameters.AddWithValue("@id", s.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM status_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}