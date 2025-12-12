using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.OrdemServico.Interfaces;

namespace sigej.db.repositories.OrdemServico
{
    public class OrdemServicoRepository : IOrdemServicoRepository
    {
        public async Task<IEnumerable<sigej.domain.models.OS.OrdemServico>> GetAllAsync()
        {
            var list = new List<sigej.domain.models.OS.OrdemServico>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, numero_sequencial, solicitante_id, area_campus_id, tipo_os_id, equipe_id, lider_id, status_id, prioridade, data_abertura, data_prevista, descricao_problema 
                                                      FROM ordem_servico ORDER BY data_abertura DESC", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<sigej.domain.models.OS.OrdemServico?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, numero_sequencial, solicitante_id, area_campus_id, tipo_os_id, equipe_id, lider_id, status_id, prioridade, data_abertura, data_prevista, descricao_problema 
                                                      FROM ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<int> CreateAsync(sigej.domain.models.OS.OrdemServico o)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO ordem_servico (numero_sequencial, solicitante_id, area_campus_id, tipo_os_id, equipe_id, lider_id, status_id, prioridade, data_abertura, data_prevista, descricao_problema) 
                                                      VALUES (@num, @solId, @areaId, @tipoId, @eqId, @liderId, @stId, @prio, @dataAb, @dataPrev, @desc) RETURNING id", conn);

            cmd.Parameters.AddWithValue("@num", o.NumeroSequencial);
            cmd.Parameters.AddWithValue("@solId", o.SolicitanteId);
            cmd.Parameters.AddWithValue("@areaId", o.AreaCampusId);
            cmd.Parameters.AddWithValue("@tipoId", o.TipoOsId);
            cmd.Parameters.AddWithValue("@eqId", o.EquipeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@liderId", o.LiderId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@stId", o.StatusId);
            cmd.Parameters.AddWithValue("@prio", o.Prioridade);
            cmd.Parameters.AddWithValue("@dataAb", o.DataAbertura);
            cmd.Parameters.AddWithValue("@dataPrev", o.DataPrevista ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@desc", o.DescricaoProblema ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(sigej.domain.models.OS.OrdemServico o)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE ordem_servico SET numero_sequencial=@num, solicitante_id=@solId, area_campus_id=@areaId, tipo_os_id=@tipoId, 
                                                      equipe_id=@eqId, lider_id=@liderId, status_id=@stId, prioridade=@prio, data_abertura=@dataAb, data_prevista=@dataPrev, descricao_problema=@desc 
                                                      WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@num", o.NumeroSequencial);
            cmd.Parameters.AddWithValue("@solId", o.SolicitanteId);
            cmd.Parameters.AddWithValue("@areaId", o.AreaCampusId);
            cmd.Parameters.AddWithValue("@tipoId", o.TipoOsId);
            cmd.Parameters.AddWithValue("@eqId", o.EquipeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@liderId", o.LiderId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@stId", o.StatusId);
            cmd.Parameters.AddWithValue("@prio", o.Prioridade);
            cmd.Parameters.AddWithValue("@dataAb", o.DataAbertura);
            cmd.Parameters.AddWithValue("@dataPrev", o.DataPrevista ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@desc", o.DescricaoProblema ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", o.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private sigej.domain.models.OS.OrdemServico MapReader(NpgsqlDataReader reader)
        {
            return new sigej.domain.models.OS.OrdemServico
            {
                Id = Convert.ToInt32(reader["id"]),
                NumeroSequencial = reader["numero_sequencial"].ToString(),
                SolicitanteId = Convert.ToInt32(reader["solicitante_id"]),
                AreaCampusId = Convert.ToInt32(reader["area_campus_id"]),
                TipoOsId = Convert.ToInt32(reader["tipo_os_id"]),
                EquipeId = reader["equipe_id"] != DBNull.Value ? Convert.ToInt32(reader["equipe_id"]) : null,
                LiderId = reader["lider_id"] != DBNull.Value ? Convert.ToInt32(reader["lider_id"]) : null,
                StatusId = Convert.ToInt32(reader["status_id"]),
                Prioridade = Convert.ToInt32(reader["prioridade"]),
                DataAbertura = Convert.ToDateTime(reader["data_abertura"]),
                DataPrevista = reader["data_prevista"] is DateOnly dp ? dp.ToDateTime(TimeOnly.MinValue) : null,
                DescricaoProblema = reader["descricao_problema"] != DBNull.Value ? reader["descricao_problema"].ToString() : null
            };
        }
    }
}