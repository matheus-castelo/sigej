using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;
using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios
{
    public class TipoFuncionarioRepository : ITipoFuncionarioRepository
    {
        public async Task<IEnumerable<TipoFuncionario>> GetAllAsync(TipoFuncionario model)
        {
            var list = new List<TipoFuncionario>();

            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, descricao FROM tipo_funcionario ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new TipoFuncionario
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                });
            }

            return list;
        }

        public async Task<TipoFuncionario?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, descricao FROM tipo_funcionario WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TipoFuncionario
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(TipoFuncionario model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO tipo_funcionario (descricao) VALUES (@Descricao) RETURNING id", conn);

            cmd.Parameters.AddWithValue("@Descricao", model.Descricao);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(TipoFuncionario model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE tipo_funcionario SET descricao=@Descricao WHERE id=@Id", conn);

            cmd.Parameters.AddWithValue("@Descricao", model.Descricao);
            cmd.Parameters.AddWithValue("@Id", model.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"DELETE FROM tipo_funcionario WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}