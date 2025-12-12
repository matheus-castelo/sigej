using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;
using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios
{
    public class PessoaRepository : IPessoaRepository
    {
        public async Task<IEnumerable<Pessoa>> GetAllAsync(Pessoa model)
        {
            var list = new List<Pessoa>();

            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, nome, cpf, matricula_siape, email, telefone, ativo FROM pessoa ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Pessoa
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString(),
                    Cpf = reader["cpf"] != DBNull.Value ? reader["cpf"].ToString() : null,
                    MatriculaSiape = reader["matricula_siape"] != DBNull.Value ? reader["matricula_siape"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Telefone = reader["telefone"] != DBNull.Value ? reader["telefone"].ToString() : null,
                    Ativo = Convert.ToBoolean(reader["ativo"])
                });
            }

            return list;
        }

        public async Task<Pessoa?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, nome, cpf, matricula_siape, email, telefone, ativo FROM pessoa WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Pessoa
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString(),
                    Cpf = reader["cpf"] != DBNull.Value ? reader["cpf"].ToString() : null,
                    MatriculaSiape = reader["matricula_siape"] != DBNull.Value ? reader["matricula_siape"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Telefone = reader["telefone"] != DBNull.Value ? reader["telefone"].ToString() : null,
                    Ativo = Convert.ToBoolean(reader["ativo"])
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Pessoa model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO pessoa (nome, cpf, matricula_siape, email, telefone, ativo) VALUES (@Nome, @Cpf, @MatriculaSiape, @Email, @Telefone, @Ativo) RETURNING id", conn);

            cmd.Parameters.AddWithValue("@Nome", model.Nome);
            cmd.Parameters.AddWithValue("@Cpf", model.Cpf ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MatriculaSiape", model.MatriculaSiape ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefone", model.Telefone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ativo", model.Ativo);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(Pessoa model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE pessoa SET nome=@Nome, cpf=@Cpf, matricula_siape=@MatriculaSiape, email=@Email, telefone=@Telefone, ativo=@Ativo WHERE id=@Id", conn);

            cmd.Parameters.AddWithValue("@Nome", model.Nome);
            cmd.Parameters.AddWithValue("@Cpf", model.Cpf ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@MatriculaSiape", model.MatriculaSiape ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefone", model.Telefone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Ativo", model.Ativo);
            cmd.Parameters.AddWithValue("@Id", model.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"DELETE FROM pessoa WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}