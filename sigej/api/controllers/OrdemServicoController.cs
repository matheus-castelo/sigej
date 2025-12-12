using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories.OrdemServico.Interfaces;
using dominioOS = sigej.domain.models.OS;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    [Tags("Ordem de Serviço")]
    public class OrdemServicoController : ControllerBase
    {
        private readonly ITipoOrdemServicoRepository _tipoOsRepo;
        private readonly IStatusOrdemServicoRepository _statusOsRepo;
        private readonly IOrdemServicoRepository _osRepo;
        private readonly IItemOrdemServicoRepository _itemOsRepo;
        private readonly IAndamentoOrdemServicoRepository _andamentoRepo;

        public OrdemServicoController(
            ITipoOrdemServicoRepository tipoOsRepo,
            IStatusOrdemServicoRepository statusOsRepo,
            IOrdemServicoRepository osRepo,
            IItemOrdemServicoRepository itemOsRepo,
            IAndamentoOrdemServicoRepository andamentoRepo)
        {
            _tipoOsRepo = tipoOsRepo;
            _statusOsRepo = statusOsRepo;
            _osRepo = osRepo;
            _itemOsRepo = itemOsRepo;
            _andamentoRepo = andamentoRepo;
        }

        #region Configurações OS (Tipo/Status)
        [HttpGet("tipos-os")]
        public async Task<IActionResult> GetTiposOs() => Ok(await _tipoOsRepo.GetAllAsync());

        [HttpPost("tipos-os")]
        public async Task<IActionResult> CreateTipoOs([FromBody] dominioOS.TipoOrdemServico t)
        {
            t.Id = await _tipoOsRepo.CreateAsync(t);
            return Created("", t);
        }

        [HttpGet("status-os")]
        public async Task<IActionResult> GetStatusOs() => Ok(await _statusOsRepo.GetAllAsync());

        [HttpPost("status-os")]
        public async Task<IActionResult> CreateStatusOs([FromBody] dominioOS.StatusOrdemServico s)
        {
            s.Id = await _statusOsRepo.CreateAsync(s);
            return Created("", s);
        }
        #endregion

        #region OS CRUD
        [HttpGet]
        public async Task<IActionResult> GetOs() => Ok(await _osRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOsById(int id)
        {
            var o = await _osRepo.GetByIdAsync(id);
            return o == null ? NotFound() : Ok(o);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOs([FromBody] dominioOS.OrdemServico o)
        {
            o.Id = await _osRepo.CreateAsync(o);
            return CreatedAtAction(nameof(GetOsById), new { id = o.Id }, o);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOs(int id, [FromBody] dominioOS.OrdemServico o)
        {
            o.Id = id;
            var ok = await _osRepo.UpdateAsync(o);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOs(int id)
        {
            var ok = await _osRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion

        #region Itens e Andamentos
        [HttpGet("item-os")]
        public async Task<IActionResult> GetItensOs() => Ok(await _itemOsRepo.GetAllAsync());

        [HttpPost("item-os")]
        public async Task<IActionResult> CreateItemOs([FromBody] dominioOS.ItemOrdemServico i)
        {
            i.Id = await _itemOsRepo.CreateAsync(i);
            return Created("", i);
        }

        [HttpGet("andamentos")]
        public async Task<IActionResult> GetAndamentos() => Ok(await _andamentoRepo.GetAllAsync());

        [HttpPost("andamentos")]
        public async Task<IActionResult> CreateAndamento([FromBody] dominioOS.AndamentoOrdemServico a)
        {
            a.Id = await _andamentoRepo.CreateAsync(a);
            return Created("", a);
        }
        #endregion
    }
}