using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.Equipes.Interfaces;
using sigej.domain.models.LocalizacaoEEquipes;

namespace sigej.db.repositories.Equipes
{
    public class EquipeMembroRepository : IEquipeMembroRepository
    {
        public async Task<IEnumerable<EquipeMembro>> GetAllAsync()
        {
            var list = new List<EquipeMembro>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, equipe_id, funcionario_id, data_inicio, data_fim, funcao FROM equipe_membro ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<EquipeMembro?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, equipe_id, funcionario_id, data_inicio, data_fim, funcao FROM equipe_membro WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<IEnumerable<EquipeMembro>> GetByEquipeIdAsync(int equipeId)
        {
            var list = new List<EquipeMembro>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, equipe_id, funcionario_id, data_inicio, data_fim, funcao FROM equipe_membro WHERE equipe_id = @equipeId", conn);
            cmd.Parameters.AddWithValue("@equipeId", equipeId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<int> CreateAsync(EquipeMembro m)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO equipe_membro (equipe_id, funcionario_id, data_inicio, data_fim, funcao) 
                                                      VALUES (@eqId, @funcId, @inicio, @fim, @funcao) RETURNING id", conn);
            
            cmd.Parameters.AddWithValue("@eqId", m.EquipeId);
            cmd.Parameters.AddWithValue("@funcId", m.FuncionarioId);
            cmd.Parameters.AddWithValue("@inicio", m.DataInicio);
            cmd.Parameters.AddWithValue("@fim", m.DataFim ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@funcao", m.Funcao ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(EquipeMembro m)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE equipe_membro SET equipe_id = @eqId, funcionario_id = @funcId, 
                                                      data_inicio = @inicio, data_fim = @fim, funcao = @funcao WHERE id = @id", conn);
            
            cmd.Parameters.AddWithValue("@eqId", m.EquipeId);
            cmd.Parameters.AddWithValue("@funcId", m.FuncionarioId);
            cmd.Parameters.AddWithValue("@inicio", m.DataInicio);
            cmd.Parameters.AddWithValue("@fim", m.DataFim ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@funcao", m.Funcao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", m.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM equipe_membro WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private EquipeMembro MapReader(NpgsqlDataReader reader)
        {
            var dtInicio = reader["data_inicio"];
            var dtFim = reader["data_fim"];

            return new EquipeMembro
            {
                Id = Convert.ToInt32(reader["id"]),
                EquipeId = Convert.ToInt32(reader["equipe_id"]),
                FuncionarioId = Convert.ToInt32(reader["funcionario_id"]),
                DataInicio = dtInicio is DateOnly di ? di.ToDateTime(TimeOnly.MinValue) : Convert.ToDateTime(dtInicio),
                DataFim = dtFim is DateOnly df ? df.ToDateTime(TimeOnly.MinValue) : null,
                Funcao = reader["funcao"] != DBNull.Value ? reader["funcao"].ToString() : null
            };
        }
    }
}