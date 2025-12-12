using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    [Tags("Relatórios")]
    public class RelatoriosController : ControllerBase
    {
        private readonly RelatorioRepository _relatorioRepo;

        public RelatoriosController(RelatorioRepository relatorioRepo)
        {
            _relatorioRepo = relatorioRepo;
        }

        [HttpGet("estoque")]
        public async Task<IActionResult> GetRelatorioEstoque()
        {
            var dados = await _relatorioRepo.GetSaldoEstoqueAsync();
            return Ok(dados);
        }

        [HttpGet("movimentos")]
        public async Task<IActionResult> GetRelatorioMovimentos([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            if (inicio == default || fim == default)
                return BadRequest("Os parâmetros 'inicio' e 'fim' são obrigatórios.");

            var dados = await _relatorioRepo.GetMovimentosPeriodoAsync(inicio, fim);
            return Ok(dados);
        }

        [HttpGet("os-abertas")]
        public async Task<IActionResult> GetRelatorioOsAbertas()
        {
            var dados = await _relatorioRepo.GetOsAbertasAsync();
            return Ok(dados);
        }

        [HttpGet("consumo-equipes")]
        public async Task<IActionResult> GetRelatorioConsumoEquipes()
        {
            var dados = await _relatorioRepo.GetConsumoPorEquipeAsync();
            return Ok(dados);
        }

        [HttpGet("andamentos/{ordemId}")]
        public async Task<IActionResult> GetRelatorioAndamentos(int ordemId)
        {
            var dados = await _relatorioRepo.GetAndamentoOsAsync(ordemId);
            return Ok(dados);
        }
    }
}