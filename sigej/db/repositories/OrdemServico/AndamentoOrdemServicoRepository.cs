using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.OrdemServico.Interfaces;
using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico
{
    public class AndamentoOrdemServicoRepository : IAndamentoOrdemServicoRepository
    {
        public async Task<IEnumerable<AndamentoOrdemServico>> GetAllAsync()
        {
            var list = new List<AndamentoOrdemServico>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, os_id, data_hora, status_anterior_id, status_novo_id, funcionario_id, descricao, inicio_atendimento, fim_atendimento 
                                                      FROM andamento_ordem_servico ORDER BY data_hora", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<AndamentoOrdemServico?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, os_id, data_hora, status_anterior_id, status_novo_id, funcionario_id, descricao, inicio_atendimento, fim_atendimento 
                                                      FROM andamento_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<IEnumerable<AndamentoOrdemServico>> GetByOrdemServicoIdAsync(int osId)
        {
            var list = new List<AndamentoOrdemServico>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, os_id, data_hora, status_anterior_id, status_novo_id, funcionario_id, descricao, inicio_atendimento, fim_atendimento 
                                                      FROM andamento_ordem_servico WHERE os_id = @osId ORDER BY data_hora", conn);
            cmd.Parameters.AddWithValue("@osId", osId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<int> CreateAsync(AndamentoOrdemServico a)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO andamento_ordem_servico (os_id, data_hora, status_anterior_id, status_novo_id, funcionario_id, descricao, inicio_atendimento, fim_atendimento) 
                                                      VALUES (@osId, @data, @stAnt, @stNov, @funcId, @desc, @ini, @fim) RETURNING id", conn);

            cmd.Parameters.AddWithValue("@osId", a.OsId);
            cmd.Parameters.AddWithValue("@data", a.DataHora);
            cmd.Parameters.AddWithValue("@stAnt", a.StatusAnteriorId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@stNov", a.StatusNovoId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@funcId", a.FuncionarioId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@desc", a.Descricao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ini", a.InicioAtendimento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fim", a.FimAtendimento ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(AndamentoOrdemServico a)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE andamento_ordem_servico SET os_id=@osId, data_hora=@data, status_anterior_id=@stAnt, status_novo_id=@stNov, 
                                                      funcionario_id=@funcId, descricao=@desc, inicio_atendimento=@ini, fim_atendimento=@fim WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@osId", a.OsId);
            cmd.Parameters.AddWithValue("@data", a.DataHora);
            cmd.Parameters.AddWithValue("@stAnt", a.StatusAnteriorId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@stNov", a.StatusNovoId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@funcId", a.FuncionarioId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@desc", a.Descricao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ini", a.InicioAtendimento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fim", a.FimAtendimento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", a.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM andamento_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private AndamentoOrdemServico MapReader(NpgsqlDataReader reader)
        {
            return new AndamentoOrdemServico
            {
                Id = Convert.ToInt32(reader["id"]),
                OsId = Convert.ToInt32(reader["os_id"]),
                DataHora = Convert.ToDateTime(reader["data_hora"]),
                StatusAnteriorId = reader["status_anterior_id"] != DBNull.Value ? Convert.ToInt32(reader["status_anterior_id"]) : null,
                StatusNovoId = reader["status_novo_id"] != DBNull.Value ? Convert.ToInt32(reader["status_novo_id"]) : null,
                FuncionarioId = reader["funcionario_id"] != DBNull.Value ? Convert.ToInt32(reader["funcionario_id"]) : null,
                Descricao = reader["descricao"] != DBNull.Value ? reader["descricao"].ToString() : null,
                InicioAtendimento = reader["inicio_atendimento"] != DBNull.Value ? Convert.ToDateTime(reader["inicio_atendimento"]) : null,
                FimAtendimento = reader["fim_atendimento"] != DBNull.Value ? Convert.ToDateTime(reader["fim_atendimento"]) : null
            };
        }
    }
}