using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.db.repositories;
using dominioProd = sigej.domain.models.ProdutosEMateriais;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    [Tags("Produtos")]
    public class ProdutosController : ControllerBase
    {
        private readonly ICategoriaMaterialRepository _categoriaRepo;
        private readonly IUnidadeMedidaRepository _unidadeRepo;
        private readonly IMarcaRepository _marcaRepo;
        private readonly ICorRepository _corRepo;
        private readonly ITamanhoRepository _tamanhoRepo;
        private readonly IProdutoRepository _produtoRepo;
        private readonly IProdutoVariacaoRepository _variacaoRepo;

        public ProdutosController(
            ICategoriaMaterialRepository categoriaRepo,
            IUnidadeMedidaRepository unidadeRepo,
            IMarcaRepository marcaRepo,
            ICorRepository corRepo,
            ITamanhoRepository tamanhoRepo,
            IProdutoRepository produtoRepo,
            IProdutoVariacaoRepository variacaoRepo)
        {
            _categoriaRepo = categoriaRepo;
            _unidadeRepo = unidadeRepo;
            _marcaRepo = marcaRepo;
            _corRepo = corRepo;
            _tamanhoRepo = tamanhoRepo;
            _produtoRepo = produtoRepo;
            _variacaoRepo = variacaoRepo;
        }

        #region Auxiliares (Categorias, Unidades, Marcas...)
        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias() => Ok(await _categoriaRepo.GetAllAsync());

        [HttpPost("categorias")]
        public async Task<IActionResult> CreateCategoria([FromBody] dominioProd.CategoriaMaterial c)
        {
            c.Id = await _categoriaRepo.CreateAsync(c);
            return Created("", c);
        }

        [HttpGet("unidades")]
        public async Task<IActionResult> GetUnidades() => Ok(await _unidadeRepo.GetAllAsync());

        [HttpPost("unidades")]
        public async Task<IActionResult> CreateUnidade([FromBody] dominioProd.UnidadeMedida u)
        {
            u.Id = await _unidadeRepo.CreateAsync(u);
            return Created("", u);
        }

        [HttpGet("marcas")]
        public async Task<IActionResult> GetMarcas() => Ok(await _marcaRepo.GetAllAsync());

        [HttpPost("marcas")]
        public async Task<IActionResult> CreateMarca([FromBody] dominioProd.Marca m)
        {
            m.Id = await _marcaRepo.CreateAsync(m);
            return Created("", m);
        }

        [HttpGet("cores")]
        public async Task<IActionResult> GetCores() => Ok(await _corRepo.GetAllAsync());

        [HttpPost("cores")]
        public async Task<IActionResult> CreateCor([FromBody] dominioProd.Cor c)
        {
            c.Id = await _corRepo.CreateAsync(c);
            return Created("", c);
        }

        [HttpGet("tamanhos")]
        public async Task<IActionResult> GetTamanhos() => Ok(await _tamanhoRepo.GetAllAsync());

        [HttpPost("tamanhos")]
        public async Task<IActionResult> CreateTamanho([FromBody] dominioProd.Tamanho t)
        {
            t.Id = await _tamanhoRepo.CreateAsync(t);
            return Created("", t);
        }
        #endregion

        #region Produtos CRUD
        [HttpGet]
        public async Task<IActionResult> GetProdutos() => Ok(await _produtoRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProdutoById(int id)
        {
            var p = await _produtoRepo.GetByIdAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduto([FromBody] dominioProd.Produto p)
        {
            p.Id = await _produtoRepo.CreateAsync(p);
            return CreatedAtAction(nameof(GetProdutoById), new { id = p.Id }, p);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduto(int id, [FromBody] dominioProd.Produto p)
        {
            p.Id = id;
            var ok = await _produtoRepo.UpdateAsync(p);
            return !ok ? NotFound() : NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var ok = await _produtoRepo.DeleteAsync(id);
            return !ok ? NotFound() : NoContent();
        }
        #endregion

        #region Variações
        [HttpGet("variacoes")]
        public async Task<IActionResult> GetVariacoes() => Ok(await _variacaoRepo.GetAllAsync());

        [HttpPost("variacoes")]
        public async Task<IActionResult> CreateVariacao([FromBody] dominioProd.ProdutoVariacao v)
        {
            v.Id = await _variacaoRepo.CreateAsync(v);
            return Created("", v);
        }
        #endregion
    }
}