using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sigej.db.repositories.AreaCampus.Interfaces;
using sigej.db.repositories.Equipes.Interfaces;
using sigej.db.repositories.OrdemServico.Interfaces;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;
using sigej.db.seed;

namespace sigej.api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ITipoAreaCampusRepository _tipoAreaRepo;
        private readonly IAreaCampusRepository _areaRepo;
        private readonly IEquipeRepository _equipeRepo;
        private readonly IEquipeMembroRepository _membroRepo;
        private readonly IPessoaRepository _pessoaRepo;
        private readonly IFuncionarioRepository _funcRepo;
        private readonly ISetorRepository _setorRepo;
        private readonly ITipoFuncionarioRepository _tipoFuncRepo;
        private readonly ICategoriaMaterialRepository _categoriaRepo;
        private readonly IUnidadeMedidaRepository _unidadeRepo;
        private readonly IMarcaRepository _marcaRepo;
        private readonly ICorRepository _corRepo;
        private readonly ITamanhoRepository _tamanhoRepo;
        private readonly IProdutoRepository _produtoRepo;
        private readonly IProdutoVariacaoRepository _variacaoRepo;
        private readonly ILocalEstoqueRepository _localEstoqueRepo;
        private readonly IEstoqueRepository _estoqueRepo;
        private readonly ITipoMovimentoEstoqueRepository _tipoMovRepo;
        private readonly IMovimentoEstoqueRepository _movRepo;
        private readonly ITipoOrdemServicoRepository _tipoOsRepo;
        private readonly IStatusOrdemServicoRepository _statusOsRepo;
        private readonly IOrdemServicoRepository _osRepo;
        private readonly IItemOrdemServicoRepository _itemOsRepo;
        private readonly IAndamentoOrdemServicoRepository _andamentoRepo;

        public AdminController(
            ITipoAreaCampusRepository tipoAreaRepo, IAreaCampusRepository areaRepo,
            IEquipeRepository equipeRepo, IEquipeMembroRepository membroRepo,
            IPessoaRepository pessoaRepo, IFuncionarioRepository funcRepo,
            ISetorRepository setorRepo, ITipoFuncionarioRepository tipoFuncRepo,
            ICategoriaMaterialRepository categoriaRepo, IUnidadeMedidaRepository unidadeRepo,
            IMarcaRepository marcaRepo, ICorRepository corRepo, ITamanhoRepository tamanhoRepo,
            IProdutoRepository produtoRepo, IProdutoVariacaoRepository variacaoRepo,
            ILocalEstoqueRepository localEstoqueRepo, IEstoqueRepository estoqueRepo,
            ITipoMovimentoEstoqueRepository tipoMovRepo, IMovimentoEstoqueRepository movRepo,
            ITipoOrdemServicoRepository tipoOsRepo, IStatusOrdemServicoRepository statusOsRepo,
            IOrdemServicoRepository osRepo, IItemOrdemServicoRepository itemOsRepo,
            IAndamentoOrdemServicoRepository andamentoRepo)
        {
            _tipoAreaRepo = tipoAreaRepo; _areaRepo = areaRepo;
            _equipeRepo = equipeRepo; _membroRepo = membroRepo;
            _pessoaRepo = pessoaRepo; _funcRepo = funcRepo;
            _setorRepo = setorRepo; _tipoFuncRepo = tipoFuncRepo;
            _categoriaRepo = categoriaRepo; _unidadeRepo = unidadeRepo;
            _marcaRepo = marcaRepo; _corRepo = corRepo; _tamanhoRepo = tamanhoRepo;
            _produtoRepo = produtoRepo; _variacaoRepo = variacaoRepo;
            _localEstoqueRepo = localEstoqueRepo; _estoqueRepo = estoqueRepo;
            _tipoMovRepo = tipoMovRepo; _movRepo = movRepo;
            _tipoOsRepo = tipoOsRepo; _statusOsRepo = statusOsRepo;
            _osRepo = osRepo; _itemOsRepo = itemOsRepo; _andamentoRepo = andamentoRepo;
        }

        private DbSeeder GetSeeder()
        {
            return new DbSeeder(
                _tipoAreaRepo, _areaRepo, _equipeRepo, _membroRepo, _pessoaRepo, _funcRepo,
                _setorRepo, _tipoFuncRepo, _categoriaRepo, _unidadeRepo, _marcaRepo, _corRepo,
                _tamanhoRepo, _produtoRepo, _variacaoRepo, _localEstoqueRepo, _estoqueRepo,
                _tipoMovRepo, _movRepo, _tipoOsRepo, _statusOsRepo, _osRepo, _itemOsRepo, _andamentoRepo
            );
        }

        [HttpPost("populate-database")]
        [Tags("Admin")]
        public async Task<IActionResult> PopulateDatabase()
        {
            var seeder = GetSeeder();
            await seeder.PopulateAsync();
            return Ok("Banco de dados populado com sucesso (sem limpar dados existentes)!");
        }

        [HttpDelete("clean-database")]
        [Tags("Admin")]
        public async Task<IActionResult> CleanDatabase()
        {
            var seeder = GetSeeder();
            await seeder.CleanDatabaseAsync();
            return Ok("Banco de dados limpo com sucesso!");
        }
    }
}