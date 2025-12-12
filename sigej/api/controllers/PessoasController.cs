using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;
using dominioPE = sigej.domain.models.PessoasEEstrutura;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepo;
        private readonly IFuncionarioRepository _funcRepo;
        private readonly ISetorRepository _setorRepo;
        private readonly ITipoFuncionarioRepository _tipoFuncRepo;

        public PessoasController(
            IPessoaRepository pessoaRepo,
            IFuncionarioRepository funcRepo,
            ISetorRepository setorRepo,
            ITipoFuncionarioRepository tipoFuncRepo)
        {
            _pessoaRepo = pessoaRepo;
            _funcRepo = funcRepo;
            _setorRepo = setorRepo;
            _tipoFuncRepo = tipoFuncRepo;
        }

        #region Pessoas
        [HttpGet]
        [Tags("Pessoas")]
        public async Task<IActionResult> GetPessoas() => Ok(await _pessoaRepo.GetAllAsync(new dominioPE.Pessoa()));

        [HttpGet("{id}")]
        [Tags("Pessoas")]
        public async Task<IActionResult> GetPessoaById(int id)
        {
            var p = await _pessoaRepo.GetByIdAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpPost]
        [Tags("Pessoas")]
        public async Task<IActionResult> CreatePessoa([FromBody] dominioPE.Pessoa p)
        {
            p.Id = await _pessoaRepo.CreateAsync(p);
            return CreatedAtAction(nameof(GetPessoaById), new { id = p.Id }, p);
        }

        [HttpPut("{id}")]
        [Tags("Pessoas")]
        public async Task<IActionResult> UpdatePessoa(int id, [FromBody] dominioPE.Pessoa p)
        {
            p.Id = id;
            var ok = await _pessoaRepo.UpdateAsync(p);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("{id}")]
        [Tags("Pessoas")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var ok = await _pessoaRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion

        #region Funcionarios
        [HttpGet("funcionarios")]
        [Tags("Funcionários")]
        public async Task<IActionResult> GetFuncionarios() => Ok(await _funcRepo.GetAllAsync(new dominioPE.Funcionario()));

        [HttpGet("funcionarios/{id}")]
        [Tags("Funcionários")]
        public async Task<IActionResult> GetFuncionarioById(int id)
        {
            var f = await _funcRepo.GetByIdAsync(id);
            return f == null ? NotFound() : Ok(f);
        }

        [HttpPost("funcionarios")]
        [Tags("Funcionários")]
        public async Task<IActionResult> CreateFuncionario([FromBody] dominioPE.Funcionario f)
        {
            f.Id = await _funcRepo.CreateAsync(f);
            return CreatedAtAction(nameof(GetFuncionarioById), new { id = f.Id }, f);
        }

        [HttpPut("funcionarios/{id}")]
        [Tags("Funcionários")]
        public async Task<IActionResult> UpdateFuncionario(int id, [FromBody] dominioPE.Funcionario f)
        {
            f.Id = id;
            var ok = await _funcRepo.UpdateAsync(f);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("funcionarios/{id}")]
        [Tags("Funcionários")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            var ok = await _funcRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion

        #region Setores
        [HttpGet("setores")]
        [Tags("Estrutura")]
        public async Task<IActionResult> GetSetores() => Ok(await _setorRepo.GetAllAsync(new dominioPE.Setor()));

        [HttpGet("setores/{id}")]
        [Tags("Estrutura")]
        public async Task<IActionResult> GetSetorById(int id)
        {
            var s = await _setorRepo.GetByIdAsync(id);
            return s == null ? NotFound() : Ok(s);
        }

        [HttpPost("setores")]
        [Tags("Estrutura")]
        public async Task<IActionResult> CreateSetor([FromBody] dominioPE.Setor s)
        {
            s.Id = await _setorRepo.CreateAsync(s);
            return CreatedAtAction(nameof(GetSetorById), new { id = s.Id }, s);
        }

        [HttpPut("setores/{id}")]
        [Tags("Estrutura")]
        public async Task<IActionResult> UpdateSetor(int id, [FromBody] dominioPE.Setor s)
        {
            s.Id = id;
            var ok = await _setorRepo.UpdateAsync(s);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("setores/{id}")]
        [Tags("Estrutura")]
        public async Task<IActionResult> DeleteSetor(int id)
        {
            var ok = await _setorRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion

        #region Tipo Funcionario
        [HttpGet("tipo-funcionario")]
        [Tags("Funcionários")]
        public async Task<IActionResult> GetTipoFuncionarios() => Ok(await _tipoFuncRepo.GetAllAsync(new dominioPE.TipoFuncionario()));

        [HttpGet("tipo-funcionario/{id}")]
        [Tags("Funcionários")]
        public async Task<IActionResult> GetTipoFuncionarioById(int id)
        {
            var t = await _tipoFuncRepo.GetByIdAsync(id);
            return t == null ? NotFound() : Ok(t);
        }

        [HttpPost("tipo-funcionario")]
        [Tags("Funcionários")]
        public async Task<IActionResult> CreateTipoFuncionario([FromBody] dominioPE.TipoFuncionario t)
        {
            t.Id = await _tipoFuncRepo.CreateAsync(t);
            return CreatedAtAction(nameof(GetTipoFuncionarioById), new { id = t.Id }, t);
        }

        [HttpPut("tipo-funcionario/{id}")]
        [Tags("Funcionários")]
        public async Task<IActionResult> UpdateTipoFuncionario(int id, [FromBody] dominioPE.TipoFuncionario t)
        {
            t.Id = id;
            var ok = await _tipoFuncRepo.UpdateAsync(t);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("tipo-funcionario/{id}")]
        [Tags("Funcionários")]
        public async Task<IActionResult> DeleteTipoFuncionario(int id)
        {
            var ok = await _tipoFuncRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion
    }
}