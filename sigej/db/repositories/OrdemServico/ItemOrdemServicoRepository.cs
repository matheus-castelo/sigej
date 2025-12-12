using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.OrdemServico.Interfaces;
using sigej.domain.models.OS;

namespace sigej.db.repositories.OrdemServico
{
    public class ItemOrdemServicoRepository : IItemOrdemServicoRepository
    {
        public async Task<IEnumerable<ItemOrdemServico>> GetAllAsync()
        {
            var list = new List<ItemOrdemServico>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, os_id, produto_variacao_id, quantidade_prevista, quantidade_usada FROM item_ordem_servico", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<ItemOrdemServico?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, os_id, produto_variacao_id, quantidade_prevista, quantidade_usada FROM item_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<IEnumerable<ItemOrdemServico>> GetByOsIdAsync(int osId)
        {
            var list = new List<ItemOrdemServico>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, os_id, produto_variacao_id, quantidade_prevista, quantidade_usada FROM item_ordem_servico WHERE os_id = @osId", conn);
            cmd.Parameters.AddWithValue("@osId", osId);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<int> CreateAsync(ItemOrdemServico i)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("INSERT INTO item_ordem_servico (os_id, produto_variacao_id, quantidade_prevista, quantidade_usada) VALUES (@os, @prod, @qtdP, @qtdU) RETURNING id", conn);
            cmd.Parameters.AddWithValue("@os", i.OsId);
            cmd.Parameters.AddWithValue("@prod", i.ProdutoVariacaoId);
            cmd.Parameters.AddWithValue("@qtdP", i.QuantidadePrevista);
            cmd.Parameters.AddWithValue("@qtdU", i.QuantidadeUsada);
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(ItemOrdemServico i)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("UPDATE item_ordem_servico SET os_id=@os, produto_variacao_id=@prod, quantidade_prevista=@qtdP, quantidade_usada=@qtdU WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@os", i.OsId);
            cmd.Parameters.AddWithValue("@prod", i.ProdutoVariacaoId);
            cmd.Parameters.AddWithValue("@qtdP", i.QuantidadePrevista);
            cmd.Parameters.AddWithValue("@qtdU", i.QuantidadeUsada);
            cmd.Parameters.AddWithValue("@id", i.Id);
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM item_ordem_servico WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private ItemOrdemServico MapReader(NpgsqlDataReader reader)
        {
            return new ItemOrdemServico
            {
                Id = Convert.ToInt32(reader["id"]),
                OsId = Convert.ToInt32(reader["os_id"]),
                ProdutoVariacaoId = Convert.ToInt32(reader["produto_variacao_id"]),
                QuantidadePrevista = Convert.ToDecimal(reader["quantidade_prevista"]),
                QuantidadeUsada = Convert.ToDecimal(reader["quantidade_usada"])
            };
        }
    }
}