using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.AreaCampus.Interfaces;

namespace sigej.db.repositories.AreaCampus
{
    public class AreaCampusRepository : IAreaCampusRepository
    {
        public async Task<IEnumerable<domain.models.LocalizacaoEEquipes.AreaCampus>> GetAllAsync()
        {
            var list = new List<domain.models.LocalizacaoEEquipes.AreaCampus>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, tipo_area_id, descricao, bloco FROM area_campus ORDER BY id", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new domain.models.LocalizacaoEEquipes.AreaCampus
                {
                    Id = Convert.ToInt32(reader["id"]),
                    TipoAreaId = Convert.ToInt32(reader["tipo_area_id"]),
                    Descricao = reader["descricao"].ToString(),
                    Bloco = reader["bloco"] != DBNull.Value ? reader["bloco"].ToString() : null
                });
            }
            return list;
        }

        public async Task<domain.models.LocalizacaoEEquipes.AreaCampus?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, tipo_area_id, descricao, bloco FROM area_campus WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new domain.models.LocalizacaoEEquipes.AreaCampus
                {
                    Id = Convert.ToInt32(reader["id"]),
                    TipoAreaId = Convert.ToInt32(reader["tipo_area_id"]),
                    Descricao = reader["descricao"].ToString(),
                    Bloco = reader["bloco"] != DBNull.Value ? reader["bloco"].ToString() : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(domain.models.LocalizacaoEEquipes.AreaCampus a)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO area_campus (tipo_area_id, descricao, bloco) VALUES (@tipoId, @desc, @bloco) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@tipoId", a.TipoAreaId);
            cmd.Parameters.AddWithValue("@desc", a.Descricao);
            cmd.Parameters.AddWithValue("@bloco", a.Bloco ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(domain.models.LocalizacaoEEquipes.AreaCampus a)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE area_campus SET tipo_area_id = @tipoId, descricao = @desc, bloco = @bloco WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@tipoId", a.TipoAreaId);
            cmd.Parameters.AddWithValue("@desc", a.Descricao);
            cmd.Parameters.AddWithValue("@bloco", a.Bloco ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", a.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM area_campus WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}