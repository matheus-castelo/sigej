using Npgsql;
using sigej.db.connection;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.domain.models.Estoque;

namespace sigej.db.repositories.ProdutoEstoque
{
    public class MovimentoEstoqueRepository : IMovimentoEstoqueRepository
    {
        public async Task<IEnumerable<MovimentoEstoque>> GetAllAsync()
        {
            var list = new List<MovimentoEstoque>();
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, produto_variacao_id, local_estoque_id, tipo_movimento_id, quantidade, data_hora, funcionario_id, ordem_servico_id, observacao FROM movimento_estoque ORDER BY data_hora DESC", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new MovimentoEstoque
                {
                    Id = Convert.ToInt32(reader["id"]),
                    ProdutoVariacaoId = Convert.ToInt32(reader["produto_variacao_id"]),
                    LocalEstoqueId = Convert.ToInt32(reader["local_estoque_id"]),
                    TipoMovimentoId = Convert.ToInt32(reader["tipo_movimento_id"]),
                    Quantidade = Convert.ToDecimal(reader["quantidade"]),
                    DataHora = Convert.ToDateTime(reader["data_hora"]),
                    FuncionarioId = reader["funcionario_id"] != DBNull.Value ? Convert.ToInt32(reader["funcionario_id"]) : null,
                    OrdemServicoId = reader["ordem_servico_id"] != DBNull.Value ? Convert.ToInt32(reader["ordem_servico_id"]) : null,
                    Observacao = reader["observacao"] != DBNull.Value ? reader["observacao"].ToString() : null
                });
            }
            return list;
        }

        public async Task<MovimentoEstoque?> GetByIdAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("SELECT id, produto_variacao_id, local_estoque_id, tipo_movimento_id, quantidade, data_hora, funcionario_id, ordem_servico_id, observacao FROM movimento_estoque WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new MovimentoEstoque
                {
                    Id = Convert.ToInt32(reader["id"]),
                    ProdutoVariacaoId = Convert.ToInt32(reader["produto_variacao_id"]),
                    LocalEstoqueId = Convert.ToInt32(reader["local_estoque_id"]),
                    TipoMovimentoId = Convert.ToInt32(reader["tipo_movimento_id"]),
                    Quantidade = Convert.ToDecimal(reader["quantidade"]),
                    DataHora = Convert.ToDateTime(reader["data_hora"]),
                    FuncionarioId = reader["funcionario_id"] != DBNull.Value ? Convert.ToInt32(reader["funcionario_id"]) : null,
                    OrdemServicoId = reader["ordem_servico_id"] != DBNull.Value ? Convert.ToInt32(reader["ordem_servico_id"]) : null,
                    Observacao = reader["observacao"] != DBNull.Value ? reader["observacao"].ToString() : null
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(MovimentoEstoque m)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"INSERT INTO movimento_estoque (produto_variacao_id, local_estoque_id, tipo_movimento_id, quantidade, data_hora, funcionario_id, ordem_servico_id, observacao) 
                VALUES (@pvId, @leId, @tmId, @qtd, @data, @funcId, @osId, @obs) RETURNING id", conn);
            
            cmd.Parameters.AddWithValue("@pvId", m.ProdutoVariacaoId);
            cmd.Parameters.AddWithValue("@leId", m.LocalEstoqueId);
            cmd.Parameters.AddWithValue("@tmId", m.TipoMovimentoId);
            cmd.Parameters.AddWithValue("@qtd", m.Quantidade);
            cmd.Parameters.AddWithValue("@data", m.DataHora);
            cmd.Parameters.AddWithValue("@funcId", m.FuncionarioId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@osId", m.OrdemServicoId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@obs", m.Observacao ?? (object)DBNull.Value);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<bool> UpdateAsync(MovimentoEstoque m)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand(@"UPDATE movimento_estoque SET produto_variacao_id = @pvId, local_estoque_id = @leId, tipo_movimento_id = @tmId, 
                quantidade = @qtd, data_hora = @data, funcionario_id = @funcId, ordem_servico_id = @osId, observacao = @obs WHERE id = @id", conn);
            
            cmd.Parameters.AddWithValue("@pvId", m.ProdutoVariacaoId);
            cmd.Parameters.AddWithValue("@leId", m.LocalEstoqueId);
            cmd.Parameters.AddWithValue("@tmId", m.TipoMovimentoId);
            cmd.Parameters.AddWithValue("@qtd", m.Quantidade);
            cmd.Parameters.AddWithValue("@data", m.DataHora);
            cmd.Parameters.AddWithValue("@funcId", m.FuncionarioId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@osId", m.OrdemServicoId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@obs", m.Observacao ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", m.Id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var conn = Database.GetConnection();
            await using var cmd = new NpgsqlCommand("DELETE FROM movimento_estoque WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}