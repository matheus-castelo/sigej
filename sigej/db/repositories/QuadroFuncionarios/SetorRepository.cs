using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;
using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.QuadroFuncionarios
{
    public class SetorRepository : ISetorRepository
    {
        public async Task<IEnumerable<Setor>> GetAllAsync(Setor model)
        {
            var list = new List<Setor>();

            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, nome, sigla FROM setor ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Setor
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString(),
                    Sigla = reader["sigla"] != DBNull.Value ? reader["sigla"].ToString() : null
                });
            }

            return list;
        }

        public async Task<Setor?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"SELECT id, nome, sigla FROM setor WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Setor
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString(),
                    Sigla = reader["sigla"] != DBNull.Value ? reader["sigla"].ToString() : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Setor model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO setor (nome, sigla) VALUES (@Nome, @Sigla) RETURNING id", conn);

            cmd.Parameters.AddWithValue("@Nome", model.Nome);
            cmd.Parameters.AddWithValue("@Sigla", model.Sigla ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(Setor model)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE setor SET nome=@Nome, sigla=@Sigla WHERE id=@Id", conn);

            cmd.Parameters.AddWithValue("@Nome", model.Nome);
            cmd.Parameters.AddWithValue("@Sigla", model.Sigla ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", model.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"DELETE FROM setor WHERE id=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}