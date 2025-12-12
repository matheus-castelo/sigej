using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories.Equipes.Interfaces;
using dominioArea = sigej.domain.models.LocalizacaoEEquipes;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    [Tags("Equipes")]
    public class EquipesController : ControllerBase
    {
        private readonly IEquipeRepository _equipeRepo;
        private readonly IEquipeMembroRepository _membroRepo;

        public EquipesController(IEquipeRepository equipeRepo, IEquipeMembroRepository membroRepo)
        {
            _equipeRepo = equipeRepo;
            _membroRepo = membroRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetEquipes() => Ok(await _equipeRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipeById(int id)
        {
            var e = await _equipeRepo.GetByIdAsync(id);
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquipe([FromBody] dominioArea.EquipeManutencao e)
        {
            e.Id = await _equipeRepo.CreateAsync(e);
            return CreatedAtAction(nameof(GetEquipeById), new { id = e.Id }, e);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipe(int id, [FromBody] dominioArea.EquipeManutencao e)
        {
            e.Id = id;
            var ok = await _equipeRepo.UpdateAsync(e);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipe(int id)
        {
            var ok = await _equipeRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }

        #region Membros
        [HttpGet("{equipeId}/membros")]
        public async Task<IActionResult> GetMembrosByEquipe(int equipeId) => Ok(await _membroRepo.GetByEquipeIdAsync(equipeId));

        [HttpGet("membros/{id}")]
        public async Task<IActionResult> GetMembroById(int id)
        {
            var m = await _membroRepo.GetByIdAsync(id);
            return m == null ? NotFound() : Ok(m);
        }

        [HttpPost("membros")]
        public async Task<IActionResult> CreateMembro([FromBody] dominioArea.EquipeMembro m)
        {
            m.Id = await _membroRepo.CreateAsync(m);
            return CreatedAtAction(nameof(GetMembroById), new { id = m.Id }, m);
        }

        [HttpPut("membros/{id}")]
        public async Task<IActionResult> UpdateMembro(int id, [FromBody] dominioArea.EquipeMembro m)
        {
            m.Id = id;
            var ok = await _membroRepo.UpdateAsync(m);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("membros/{id}")]
        public async Task<IActionResult> DeleteMembro(int id)
        {
            var ok = await _membroRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion
    }
}