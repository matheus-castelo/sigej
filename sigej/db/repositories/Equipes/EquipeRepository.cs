using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.Equipes.Interfaces;
using sigej.domain.models.LocalizacaoEEquipes;

namespace sigej.db.repositories.Equipes
{
    public class EquipeRepository : IEquipeRepository
    {
        public async Task<IEnumerable<EquipeManutencao>> GetAllAsync()
        {
            var list = new List<EquipeManutencao>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome, turno FROM equipe_manutencao ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new EquipeManutencao
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString(),
                    Turno = reader["turno"] != DBNull.Value ? reader["turno"].ToString() : null
                });
            }
            return list;
        }

        public async Task<EquipeManutencao?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, nome, turno FROM equipe_manutencao WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new EquipeManutencao
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Nome = reader["nome"].ToString(),
                    Turno = reader["turno"] != DBNull.Value ? reader["turno"].ToString() : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(EquipeManutencao e)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO equipe_manutencao (nome, turno) VALUES (@nome, @turno) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@turno", e.Turno ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(EquipeManutencao e)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE equipe_manutencao SET nome = @nome, turno = @turno WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@turno", e.Turno ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", e.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM equipe_manutencao WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}