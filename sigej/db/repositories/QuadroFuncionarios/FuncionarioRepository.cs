using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;
using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        public async Task<IEnumerable<Funcionario>> GetAllAsync(Funcionario model)
        {
            var list = new List<Funcionario>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, pessoa_id, tipo_funcionario_id, setor_id, data_admissao, data_demissao FROM funcionario ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }

            return list;
        }

        public async Task<Funcionario?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, pessoa_id, tipo_funcionario_id, setor_id, data_admissao, data_demissao FROM funcionario WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<int> CreateAsync(Funcionario model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO funcionario (pessoa_id, tipo_funcionario_id, setor_id, data_admissao, data_demissao) VALUES (@PessoaId, @TipoFuncionarioId, @SetorId, @DataAdmissao, @DataDemissao) RETURNING id", conn);

            cmd.Parameters.AddWithValue("@PessoaId", model.PessoaId);
            cmd.Parameters.AddWithValue("@TipoFuncionarioId", model.TipoFuncionarioId);
            cmd.Parameters.AddWithValue("@SetorId", model.SetorId);
            cmd.Parameters.AddWithValue("@DataAdmissao", model.DataAdmissao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DataDemissao", model.DataDemissao ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(Funcionario model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE funcionario SET pessoa_id=@PessoaId, tipo_funcionario_id=@TipoFuncionarioId, setor_id=@SetorId, data_admissao=@DataAdmissao, data_demissao=@DataDemissao WHERE id=@Id", conn);

            cmd.Parameters.AddWithValue("@PessoaId", model.PessoaId);
            cmd.Parameters.AddWithValue("@TipoFuncionarioId", model.TipoFuncionarioId);
            cmd.Parameters.AddWithValue("@SetorId", model.SetorId);
            cmd.Parameters.AddWithValue("@DataAdmissao", model.DataAdmissao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DataDemissao", model.DataDemissao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", model.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"DELETE FROM funcionario WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private Funcionario MapReader(NpgsqlDataReader reader)
        {
            return new Funcionario
            {
                Id = Convert.ToInt32(reader["id"]),
                PessoaId = Convert.ToInt32(reader["pessoa_id"]),
                TipoFuncionarioId = Convert.ToInt32(reader["tipo_funcionario_id"]),
                SetorId = Convert.ToInt32(reader["setor_id"]),
                DataAdmissao = reader["data_admissao"] is DateOnly da ? da.ToDateTime(TimeOnly.MinValue) : null,
                DataDemissao = reader["data_demissao"] is DateOnly dd ? dd.ToDateTime(TimeOnly.MinValue) : null
            };
        }
    }
}