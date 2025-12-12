using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories.AreaCampus.Interfaces;
using dominioPE = sigej.domain.models.PessoasEEstrutura;
using dominioArea = sigej.domain.models.LocalizacaoEEquipes;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    [Tags("Área do Campus")]
    public class AreaCampusController : ControllerBase
    {
        private readonly ITipoAreaCampusRepository _tipoAreaRepo;
        private readonly IAreaCampusRepository _areaRepo;

        public AreaCampusController(ITipoAreaCampusRepository tipoAreaRepo, IAreaCampusRepository areaRepo)
        {
            _tipoAreaRepo = tipoAreaRepo;
            _areaRepo = areaRepo;
        }

        #region Tipo Area
        [HttpGet("tipo-area")]
        public async Task<IActionResult> GetTiposArea() => Ok(await _tipoAreaRepo.GetAllAsync());

        [HttpGet("tipo-area/{id}")]
        public async Task<IActionResult> GetTipoAreaById(int id)
        {
            var t = await _tipoAreaRepo.GetByIdAsync(id);
            return t == null ? NotFound() : Ok(t);
        }

        [HttpPost("tipo-area")]
        public async Task<IActionResult> CreateTipoArea([FromBody] dominioPE.TipoAreaCampus t)
        {
            t.Id = await _tipoAreaRepo.CreateAsync(t);
            return CreatedAtAction(nameof(GetTipoAreaById), new { id = t.Id }, t);
        }

        [HttpPut("tipo-area/{id}")]
        public async Task<IActionResult> UpdateTipoArea(int id, [FromBody] dominioPE.TipoAreaCampus t)
        {
            t.Id = id;
            var ok = await _tipoAreaRepo.UpdateAsync(t);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("tipo-area/{id}")]
        public async Task<IActionResult> DeleteTipoArea(int id)
        {
            var ok = await _tipoAreaRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion

        #region Areas
        [HttpGet("areas")]
        public async Task<IActionResult> GetAreas() => Ok(await _areaRepo.GetAllAsync());

        [HttpGet("areas/{id}")]
        public async Task<IActionResult> GetAreaById(int id)
        {
            var a = await _areaRepo.GetByIdAsync(id);
            return a == null ? NotFound() : Ok(a);
        }

        [HttpPost("areas")]
        public async Task<IActionResult> CreateArea([FromBody] dominioArea.AreaCampus a)
        {
            a.Id = await _areaRepo.CreateAsync(a);
            return CreatedAtAction(nameof(GetAreaById), new { id = a.Id }, a);
        }

        [HttpPut("areas/{id}")]
        public async Task<IActionResult> UpdateArea(int id, [FromBody] dominioArea.AreaCampus a)
        {
            a.Id = id;
            var ok = await _areaRepo.UpdateAsync(a);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("areas/{id}")]
        public async Task<IActionResult> DeleteArea(int id)
        {
            var ok = await _areaRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion
    }
}