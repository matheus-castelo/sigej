using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.AreaCampus.Interfaces;
using sigej.domain.models.PessoasEEstrutura;

namespace sigej.db.repositories.AreaCampus
{
    public class TipoAreaCampusRepository : ITipoAreaCampusRepository
    {
        public async Task<IEnumerable<TipoAreaCampus>> GetAllAsync()
        {
            var list = new List<TipoAreaCampus>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM tipo_area_campus ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new TipoAreaCampus
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                });
            }
            return list;
        }

        public async Task<TipoAreaCampus?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, descricao FROM tipo_area_campus WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TipoAreaCampus
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Descricao = reader["descricao"].ToString()
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(TipoAreaCampus t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO tipo_area_campus (descricao) VALUES (@desc) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(TipoAreaCampus t)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE tipo_area_campus SET descricao = @desc WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@desc", t.Descricao);
            cmd.Parameters.AddWithValue("@id", t.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM tipo_area_campus WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}