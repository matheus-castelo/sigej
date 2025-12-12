using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using dominioEst = sigej.domain.models.Estoque;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    [Tags("Estoque")]
    public class EstoqueController : ControllerBase
    {
        private readonly ILocalEstoqueRepository _localEstoqueRepo;
        private readonly IEstoqueRepository _estoqueRepo;
        private readonly ITipoMovimentoEstoqueRepository _tipoMovRepo;
        private readonly IMovimentoEstoqueRepository _movRepo;

        public EstoqueController(
            ILocalEstoqueRepository localEstoqueRepo,
            IEstoqueRepository estoqueRepo,
            ITipoMovimentoEstoqueRepository tipoMovRepo,
            IMovimentoEstoqueRepository movRepo)
        {
            _localEstoqueRepo = localEstoqueRepo;
            _estoqueRepo = estoqueRepo;
            _tipoMovRepo = tipoMovRepo;
            _movRepo = movRepo;
        }

        [HttpGet("localestoque")]
        public async Task<IActionResult> GetLocaisEstoque() => Ok(await _localEstoqueRepo.GetAllAsync());

        [HttpPost("localestoque")]
        public async Task<IActionResult> CreateLocalEstoque([FromBody] dominioEst.LocalEstoque l)
        {
            l.Id = await _localEstoqueRepo.CreateAsync(l);
            return Created("", l);
        }

        [HttpGet]
        public async Task<IActionResult> GetEstoques() => Ok(await _estoqueRepo.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> CreateEstoque([FromBody] dominioEst.Estoque e)
        {
            var ok = await _estoqueRepo.CreateAsync(e);
            return !ok ? BadRequest() : Created("", e);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEstoque([FromBody] dominioEst.Estoque e)
        {
            var ok = await _estoqueRepo.UpdateAsync(e);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEstoque(int produtoVariacaoId, int localEstoqueId)
        {
            var ok = await _estoqueRepo.DeleteAsync(produtoVariacaoId, localEstoqueId);
            return !ok ? NotFound() : NoContent();
        }

        [HttpGet("tipomovimento")]
        public async Task<IActionResult> GetTipoMov() => Ok(await _tipoMovRepo.GetAllAsync());

        [HttpPost("tipomovimento")]
        public async Task<IActionResult> CreateTipoMov([FromBody] dominioEst.TipoMovimentoEstoque t)
        {
            t.Id = await _tipoMovRepo.CreateAsync(t);
            return Created("", t);
        }

        [HttpGet("movimentos")]
        public async Task<IActionResult> GetMovimentos() => Ok(await _movRepo.GetAllAsync());

        [HttpPost("movimentos")]
        public async Task<IActionResult> CreateMovimento([FromBody] dominioEst.MovimentoEstoque m)
        {
            m.Id = await _movRepo.CreateAsync(m);
            return Created("", m);
        }
    }
}