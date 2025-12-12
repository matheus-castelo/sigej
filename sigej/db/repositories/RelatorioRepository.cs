using System.Data;
using Npgsql;
using sigej.db.connection;
using sigej.domain.dtos;

namespace sigej.db.repositories;

public class RelatorioRepository
{
    private NpgsqlConnection GetConn()
    {
        return Database.GetConnection();
    }

    public async Task<List<RelatorioEstoqueDto>> GetSaldoEstoqueAsync()
    {
        var sql = @"
            SELECT 
                p.descricao AS produto,
                pv.id AS variacao_id,
                le.descricao AS local, 
                e.quantidade AS quantidade_atual,
                e.ponto_reposicao
            FROM estoque e
            JOIN produto_variacao pv ON pv.id = e.produto_variacao_id
            JOIN produto p ON p.id = pv.produto_id
            JOIN local_estoque le ON le.id = e.local_estoque_id
            ORDER BY p.descricao, le.descricao;
        ";

        var lista = new List<RelatorioEstoqueDto>();
        using var conn = GetConn();
        if (conn.State != ConnectionState.Open) await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new RelatorioEstoqueDto
            {
                Produto = reader.GetString(0),
                VariacaoId = reader.GetInt32(1),
                Local = reader.GetString(2),
                QuantidadeAtual = reader.GetDecimal(3),
                PontoReposicao = reader.GetDecimal(4)
            });
        }
        return lista;
    }
    
    public async Task<List<RelatorioMovimentoDto>> GetMovimentosPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var sql = @"
            SELECT 
                m.id,
                p.descricao AS produto,
                pv.id AS variacao_id,
                le.descricao AS local,
                t.descricao AS tipo_movimento,
                t.sinal,
                m.quantidade,
                m.data_hora,
                m.funcionario_id,
                o.numero_sequencial
            FROM movimento_estoque m
            JOIN produto_variacao pv ON pv.id = m.produto_variacao_id
            JOIN produto p ON p.id = pv.produto_id
            JOIN local_estoque le ON le.id = m.local_estoque_id
            JOIN tipo_movimento_estoque t ON t.id = m.tipo_movimento_id 
            LEFT JOIN ordem_servico o ON o.id = m.ordem_servico_id
            WHERE m.data_hora BETWEEN @inicio AND @fim
            ORDER BY m.data_hora DESC;
        ";

        var lista = new List<RelatorioMovimentoDto>();
        using var conn = GetConn();
        if (conn.State != ConnectionState.Open) await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@inicio", inicio);
        cmd.Parameters.AddWithValue("@fim", fim);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(new RelatorioMovimentoDto
            {
                Id = reader.GetInt32(0),
                Produto = reader.GetString(1),
                VariacaoId = reader.GetInt32(2),
                Local = reader.GetString(3),
                TipoMovimento = reader.GetString(4),
                Sinal = reader.GetChar(5),
                Quantidade = reader.GetDecimal(6),
                DataHora = reader.GetDateTime(7),
                FuncionarioId = reader.IsDBNull(8) ? null : reader.GetInt32(8),
                NumeroOrdemServico = reader.IsDBNull(9) ? null : reader.GetString(9)
            });
        }
        return lista;
    }
    
    public async Task<List<RelatorioOsAbertaDto>> GetOsAbertasAsync()
    {
        var sql = @"
            SELECT
                os.id,
                os.numero_sequencial,
                os.descricao_problema,
                os.data_abertura,
                ac.descricao AS area,
                e.nome AS equipe
            FROM ordem_servico os
            JOIN area_campus ac ON ac.id = os.area_campus_id
            LEFT JOIN equipe_manutencao e ON e.id = os.equipe_id
            JOIN status_ordem_servico s ON s.id = os.status_id
            WHERE s.descricao NOT IN ('Concluída', 'Cancelada', 'concluída', 'cancelada')
            ORDER BY os.data_abertura ASC;
        ";

        var lista = new List<RelatorioOsAbertaDto>();
        using var conn = GetConn();
        if (conn.State != ConnectionState.Open) await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(new RelatorioOsAbertaDto
            {
                Id = reader.GetInt32(0),
                NumeroSequencial = reader.GetString(1),
                Descricao = reader.GetString(2),
                DataAbertura = reader.GetDateTime(3),
                Area = reader.GetString(4),
                Equipe = reader.IsDBNull(5) ? "" : reader.GetString(5)
            });
        }
        return lista;
    }

    public async Task<List<RelatorioConsumoEquipeDto>> GetConsumoPorEquipeAsync()
    {
        var sql = @"
            SELECT
                eq.nome AS equipe,
                p.descricao AS produto,
                pv.id AS variacao_id,
                SUM(m.quantidade) AS total_consumido
            FROM movimento_estoque m
            JOIN tipo_movimento_estoque t ON t.id = m.tipo_movimento_id
            JOIN produto_variacao pv ON pv.id = m.produto_variacao_id
            JOIN produto p ON p.id = pv.produto_id
            JOIN ordem_servico os ON os.id = m.ordem_servico_id
            JOIN equipe_manutencao eq ON eq.id = os.equipe_id
            WHERE t.sinal = '-' 
            GROUP BY eq.nome, p.descricao, pv.id
            ORDER BY eq.nome, p.descricao;
        ";

        var lista = new List<RelatorioConsumoEquipeDto>();
        using var conn = GetConn();
        if (conn.State != ConnectionState.Open) await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new RelatorioConsumoEquipeDto
            {
                Equipe = reader.GetString(0),
                Produto = reader.GetString(1),
                VariacaoId = reader.GetInt32(2),
                TotalConsumido = reader.GetDecimal(3)
            });
        }
        return lista;
    }
    
    public async Task<List<RelatorioAndamentoDto>> GetAndamentoOsAsync(int ordemId)
    {
        var sql = @"
            SELECT
                id,
                data_hora,
                descricao
            FROM andamento_ordem_servico
            WHERE os_id = @id
            ORDER BY data_hora ASC;
        ";

        var lista = new List<RelatorioAndamentoDto>();
        using var conn = GetConn();
        if (conn.State != ConnectionState.Open) await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", ordemId);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(new RelatorioAndamentoDto
            {
                Id = reader.GetInt32(0),
                DataHora = reader.GetDateTime(1),
                Descricao = reader.IsDBNull(2) ? "" : reader.GetString(2)
            });
        }
        return lista;
    }
}