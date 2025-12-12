using Bogus;
using Bogus.Extensions.Brazil;
using Npgsql;
using sigej.db.repositories.AreaCampus.Interfaces;
using sigej.db.repositories.Equipes.Interfaces;
using sigej.db.repositories.OrdemServico.Interfaces;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;

using dominioArea = sigej.domain.models.LocalizacaoEEquipes;
using dominioPE = sigej.domain.models.PessoasEEstrutura;
using dominioOS = sigej.domain.models.OS;
using dominioProd = sigej.domain.models.ProdutosEMateriais;
using dominioEst = sigej.domain.models.Estoque;

namespace sigej.db.seed
{
    public class DbSeeder
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

        public DbSeeder(
            ITipoAreaCampusRepository tipoAreaRepo, IAreaCampusRepository areaRepo, IEquipeRepository equipeRepo,
            IEquipeMembroRepository membroRepo, IPessoaRepository pessoaRepo, IFuncionarioRepository funcRepo,
            ISetorRepository setorRepo, ITipoFuncionarioRepository tipoFuncRepo, ICategoriaMaterialRepository categoriaRepo,
            IUnidadeMedidaRepository unidadeRepo, IMarcaRepository marcaRepo, ICorRepository corRepo,
            ITamanhoRepository tamanhoRepo, IProdutoRepository produtoRepo, IProdutoVariacaoRepository variacaoRepo,
            ILocalEstoqueRepository localEstoqueRepo, IEstoqueRepository estoqueRepo, ITipoMovimentoEstoqueRepository tipoMovRepo,
            IMovimentoEstoqueRepository movRepo, ITipoOrdemServicoRepository tipoOsRepo, IStatusOrdemServicoRepository statusOsRepo,
            IOrdemServicoRepository osRepo, IItemOrdemServicoRepository itemOsRepo, IAndamentoOrdemServicoRepository andamentoRepo)
        {
            _tipoAreaRepo = tipoAreaRepo; _areaRepo = areaRepo; _equipeRepo = equipeRepo; _membroRepo = membroRepo;
            _pessoaRepo = pessoaRepo; _funcRepo = funcRepo; _setorRepo = setorRepo; _tipoFuncRepo = tipoFuncRepo;
            _categoriaRepo = categoriaRepo; _unidadeRepo = unidadeRepo; _marcaRepo = marcaRepo; _corRepo = corRepo;
            _tamanhoRepo = tamanhoRepo; _produtoRepo = produtoRepo; _variacaoRepo = variacaoRepo; _localEstoqueRepo = localEstoqueRepo;
            _estoqueRepo = estoqueRepo; _tipoMovRepo = tipoMovRepo; _movRepo = movRepo; _tipoOsRepo = tipoOsRepo;
            _statusOsRepo = statusOsRepo; _osRepo = osRepo; _itemOsRepo = itemOsRepo; _andamentoRepo = andamentoRepo;
        }

        public async Task PopulateAsync()
        {
            Console.WriteLine("🌱 Iniciando Seed do Banco de Dados com Bogus...");

            var setores = await SeedSetores();
            var tiposFunc = await SeedTiposFuncionario();
            var tiposArea = await SeedTiposArea();
            var categorias = await SeedCategorias();
            var unidades = await SeedUnidades();
            var marcas = await SeedMarcas();
            var cores = await SeedCores();
            var tamanhos = await SeedTamanhos();
            
            var tiposMov = await SeedTiposMovimento(); 
            var tiposOs = await SeedTiposOS();
            
            var statusOs = await SeedStatusOS(); 

            var funcionarios = await SeedPessoasEFuncionarios(setores, tiposFunc);
            var areas = await SeedAreas(tiposArea);
            var equipes = await SeedEquipes(funcionarios);
            var locaisEstoque = await SeedLocaisEstoque(funcionarios);

            var variacoes = await SeedProdutos(categorias, unidades, marcas, cores, tamanhos);
            
            await SeedEstoqueInicial(variacoes, locaisEstoque);

            var ordensCriadas = await SeedOrdensServico(funcionarios, areas, tiposOs, statusOs, equipes, variacoes);

            await SeedAndamentos(ordensCriadas, statusOs, funcionarios);

           
            await SeedMovimentos(variacoes, locaisEstoque, tiposMov, funcionarios, ordensCriadas);

            Console.WriteLine("✅ Populate Concluído com Sucesso!");
        }

        public async Task CleanDatabaseAsync()
        {
            Console.WriteLine("🧹 Limpando banco de dados...");
            await using var conn = sigej.db.connection.Database.GetConnection();
            await using var cmd = new NpgsqlCommand("", conn);

            cmd.CommandText = @"
                DELETE FROM movimento_estoque; 
                DELETE FROM andamento_ordem_servico;
                DELETE FROM item_ordem_servico;
                DELETE FROM ordem_servico;
                DELETE FROM estoque;
                DELETE FROM local_estoque;
                DELETE FROM produto_variacao;
                DELETE FROM produto;
                DELETE FROM equipe_membro;
                DELETE FROM equipe_manutencao;
                DELETE FROM funcionario;
                DELETE FROM pessoa;
                DELETE FROM area_campus;
                DELETE FROM setor;
                DELETE FROM tipo_funcionario;
                DELETE FROM tipo_area_campus;
                DELETE FROM categoria_material;
                DELETE FROM unidade_medida;
                DELETE FROM marca;
                DELETE FROM cor;
                DELETE FROM tamanho;
                DELETE FROM tipo_movimento_estoque;
                DELETE FROM tipo_ordem_servico;
                DELETE FROM status_ordem_servico;
            ";
            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("🧹 Limpeza Concluída!");
        }

        private async Task<List<int>> SeedSetores() { var ids = new List<int>(); foreach (var n in new[] { "TI", "Direção", "Infra", "RH", "Biblioteca" }) ids.Add(await _setorRepo.CreateAsync(new dominioPE.Setor { Nome = n, Sigla = n.Substring(0, 2).ToUpper() })); return ids; }
        private async Task<List<int>> SeedTiposFuncionario() { var ids = new List<int>(); foreach (var n in new[] { "Servidor", "Terceirizado", "Estagiário" }) ids.Add(await _tipoFuncRepo.CreateAsync(new dominioPE.TipoFuncionario { Descricao = n })); return ids; }
        private async Task<List<int>> SeedTiposArea() { var ids = new List<int>(); foreach (var n in new[] { "Sala", "Lab", "Jardim", "Pátio" }) ids.Add(await _tipoAreaRepo.CreateAsync(new dominioPE.TipoAreaCampus { Descricao = n })); return ids; }
        private async Task<List<int>> SeedCategorias() { var l = new List<int>(); foreach(var n in new[]{"Ferramentas","EPI","Insumos"}) l.Add(await _categoriaRepo.CreateAsync(new dominioProd.CategoriaMaterial{Nome=n})); return l; }
        private async Task<List<int>> SeedUnidades() { var l = new List<int>(); foreach(var n in new[]{"UN","KG","LT"}) l.Add(await _unidadeRepo.CreateAsync(new dominioProd.UnidadeMedida{Sigla=n, Descricao=n})); return l; }
        private async Task<List<int>> SeedMarcas() { var l = new List<int>(); foreach(var n in new[]{"Marca A","Marca B","Marca C"}) l.Add(await _marcaRepo.CreateAsync(new dominioProd.Marca{Nome=n})); return l; }
        private async Task<List<int>> SeedCores() { var l = new List<int>(); foreach(var n in new[]{"Azul","Vermelho","Verde"}) l.Add(await _corRepo.CreateAsync(new dominioProd.Cor{Nome=n})); return l; }
        private async Task<List<int>> SeedTamanhos() { var l = new List<int>(); foreach(var n in new[]{"P","M","G"}) l.Add(await _tamanhoRepo.CreateAsync(new dominioProd.Tamanho{Descricao=n})); return l; }
        private async Task<List<int>> SeedTiposMovimento() { var l = new List<int>(); l.Add(await _tipoMovRepo.CreateAsync(new dominioEst.TipoMovimentoEstoque{Descricao="Entrada", Sinal='+'})); l.Add(await _tipoMovRepo.CreateAsync(new dominioEst.TipoMovimentoEstoque{Descricao="Saída", Sinal='-'})); return l; }
        private async Task<List<int>> SeedTiposOS() { var l = new List<int>(); foreach(var n in new[]{"Elétrica","Hidráulica","Alvenaria"}) l.Add(await _tipoOsRepo.CreateAsync(new dominioOS.TipoOrdemServico{Descricao=n})); return l; }
        private async Task<List<int>> SeedStatusOS() { var l = new List<int>(); foreach(var n in new[]{"Aberta","Em Andamento","Concluída"}) l.Add(await _statusOsRepo.CreateAsync(new dominioOS.StatusOrdemServico{Descricao=n})); return l; }

        private async Task<List<int>> SeedPessoasEFuncionarios(List<int> setores, List<int> tipos)
        {
            var faker = new Faker("pt_BR");
            var funcionarioIds = new List<int>();
            for (int i = 0; i < 30; i++)
            {
                var pId = await _pessoaRepo.CreateAsync(new dominioPE.Pessoa { Nome = faker.Name.FullName(), Cpf = new Bogus.Person("pt_BR").Cpf(false), Email = faker.Internet.Email(), Telefone = "8599999999", Ativo = true });
                funcionarioIds.Add(await _funcRepo.CreateAsync(new dominioPE.Funcionario { PessoaId = pId, SetorId = faker.PickRandom(setores), TipoFuncionarioId = faker.PickRandom(tipos), DataAdmissao = faker.Date.Past(2) }));
            }
            return funcionarioIds;
        }

        private async Task<List<int>> SeedAreas(List<int> tipos)
        {
            var faker = new Faker("pt_BR");
            var ids = new List<int>();
            for(int i=0; i<10; i++) ids.Add(await _areaRepo.CreateAsync(new dominioArea.AreaCampus { Descricao = $"Bloco {faker.Random.Number(100,900)} - {faker.Commerce.Department()}", TipoAreaId = faker.PickRandom(tipos), Bloco = "A" }));
            return ids;
        }

        private async Task<List<int>> SeedEquipes(List<int> funcIds)
        {
            var faker = new Faker("pt_BR");
            var equipeIds = new List<int>();
            for(int i=0; i<4; i++)
            {
                var eqId = await _equipeRepo.CreateAsync(new dominioArea.EquipeManutencao { Nome = $"Equipe {faker.Hacker.Noun()}", Turno = "Manhã" });
                equipeIds.Add(eqId);
                for(int m=0; m<3; m++) await _membroRepo.CreateAsync(new dominioArea.EquipeMembro { EquipeId = eqId, FuncionarioId = faker.PickRandom(funcIds), DataInicio = faker.Date.Past(1), Funcao = "Técnico" });
            }
            return equipeIds;
        }

        private async Task<List<int>> SeedLocaisEstoque(List<int> funcIds)
        {
            var ids = new List<int>();
            ids.Add(await _localEstoqueRepo.CreateAsync(new dominioEst.LocalEstoque { Descricao = "Almoxarifado Central", ResponsavelId = funcIds[0] }));
            ids.Add(await _localEstoqueRepo.CreateAsync(new dominioEst.LocalEstoque { Descricao = "Depósito Jardim", ResponsavelId = funcIds[1] }));
            return ids;
        }

        private async Task<List<int>> SeedProdutos(List<int> cats, List<int> unids, List<int> marcas, List<int> cores, List<int> tams)
        {
            var faker = new Faker("pt_BR");
            var variacaoIds = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                var prodId = await _produtoRepo.CreateAsync(new dominioProd.Produto { Descricao = faker.Commerce.ProductName(), CategoriaId = faker.PickRandom(cats), MarcaId = faker.PickRandom(marcas), UnidadeMedidaId = faker.PickRandom(unids) });
                var varId = await _variacaoRepo.CreateAsync(new dominioProd.ProdutoVariacao { ProdutoId = prodId, CorId = faker.PickRandom(cores), TamanhoId = faker.PickRandom(tams), CodigoBarras = faker.Commerce.Ean13(), CodigoInterno = faker.Random.AlphaNumeric(6).ToUpper() });
                variacaoIds.Add(varId);
            }
            return variacaoIds;
        }

        private async Task SeedEstoqueInicial(List<int> varIds, List<int> locIds)
        {
            var faker = new Faker("pt_BR");
            foreach(var vId in varIds) foreach(var lId in locIds) await _estoqueRepo.CreateAsync(new dominioEst.Estoque { ProdutoVariacaoId = vId, LocalEstoqueId = lId, Quantidade = faker.Random.Int(0, 100), PontoReposicao = 10 });
        }

        private async Task<List<dominioOS.OrdemServico>> SeedOrdensServico(List<int> funcIds, List<int> areaIds, List<int> tipoIds, List<int> statusIds, List<int> equipeIds, List<int> varIds)
        {
            var faker = new Faker("pt_BR");
            var ordens = new List<dominioOS.OrdemServico>();
            
            for(int i=0; i<30; i++)
            {
                var funcionario = faker.PickRandom(funcIds);
                var pessoaId = (await _funcRepo.GetByIdAsync(funcionario)).PessoaId;
                
                var statusId = faker.PickRandom(statusIds); 

                var os = new dominioOS.OrdemServico {
                    NumeroSequencial = $"OS-2025-{i+100}",
                    SolicitanteId = pessoaId, 
                    AreaCampusId = faker.PickRandom(areaIds),
                    TipoOsId = faker.PickRandom(tipoIds),
                    StatusId = statusId,
                    EquipeId = faker.PickRandom(equipeIds),
                    Prioridade = faker.Random.Int(1, 5),
                    DataAbertura = faker.Date.Past(1), 
                    DescricaoProblema = faker.Lorem.Sentence()
                };

                os.Id = await _osRepo.CreateAsync(os);
                ordens.Add(os);

                if(faker.Random.Bool(0.7f)) 
                {
                    int qtdItems = faker.Random.Int(1, 3);
                    for(int k=0; k<qtdItems; k++)
                    {
                        await _itemOsRepo.CreateAsync(new dominioOS.ItemOrdemServico { 
                            OsId = os.Id, 
                            ProdutoVariacaoId = faker.PickRandom(varIds), 
                            QuantidadePrevista = faker.Random.Int(5, 10), 
                            QuantidadeUsada = faker.Random.Int(1, 5)
                        });
                    }
                }
            }
            return ordens;
        }


        private async Task SeedAndamentos(List<dominioOS.OrdemServico> ordens, List<int> statusIds, List<int> funcIds)
        {
            var faker = new Faker("pt_BR");

            foreach (var os in ordens)
            {
                await _andamentoRepo.CreateAsync(new dominioOS.AndamentoOrdemServico {
                    OsId = os.Id,
                    StatusAnteriorId = statusIds[0], 
                    StatusNovoId = statusIds[0],
                    FuncionarioId = faker.PickRandom(funcIds),
                    Descricao = "Abertura da OS",
                    DataHora = os.DataAbertura
                });

                if (os.StatusId == statusIds[1] || os.StatusId == statusIds[2])
                {
                    var dataInicio = os.DataAbertura.AddHours(faker.Random.Int(1, 24));
                    await _andamentoRepo.CreateAsync(new dominioOS.AndamentoOrdemServico
                    {
                        OsId = os.Id,
                        StatusAnteriorId = statusIds[0],
                        StatusNovoId = statusIds[1], 
                        FuncionarioId = faker.PickRandom(funcIds),
                        Descricao = "Início do atendimento técnico",
                        DataHora = dataInicio,
                        InicioAtendimento = dataInicio
                    });

                    if (os.StatusId == statusIds[2])
                    {
                        var dataFim = dataInicio.AddHours(faker.Random.Int(2, 48));
                        await _andamentoRepo.CreateAsync(new dominioOS.AndamentoOrdemServico
                        {
                            OsId = os.Id,
                            StatusAnteriorId = statusIds[1],
                            StatusNovoId = statusIds[2], 
                            FuncionarioId = faker.PickRandom(funcIds),
                            Descricao = "Serviço finalizado com sucesso",
                            DataHora = dataFim,
                            FimAtendimento = dataFim
                        });
                    }
                }
            }
        }

        private async Task SeedMovimentos(List<int> varIds, List<int> locIds, List<int> tipoMovIds, List<int> funcIds, List<dominioOS.OrdemServico> ordens)
        {
            var faker = new Faker("pt_BR");
            int tipoEntrada = tipoMovIds[0];
            int tipoSaida = tipoMovIds[1];

            for(int i=0; i<50; i++)
            {
                await _movRepo.CreateAsync(new dominioEst.MovimentoEstoque {
                    ProdutoVariacaoId = faker.PickRandom(varIds),
                    LocalEstoqueId = faker.PickRandom(locIds),
                    TipoMovimentoId = tipoEntrada,
                    Quantidade = faker.Random.Int(10, 50),
                    DataHora = faker.Date.Past(3),
                    FuncionarioId = faker.PickRandom(funcIds),
                    Observacao = "Entrada de Nota Fiscal"
                });
            }

            foreach(var os in ordens)
            {
                var itens = await _itemOsRepo.GetByOsIdAsync(os.Id); 
                
                foreach(var item in itens)
                {
                    if(item.QuantidadeUsada > 0)
                    {
                        await _movRepo.CreateAsync(new dominioEst.MovimentoEstoque
                        {
                            ProdutoVariacaoId = item.ProdutoVariacaoId,
                            LocalEstoqueId = faker.PickRandom(locIds), 
                            TipoMovimentoId = tipoSaida,
                            Quantidade = item.QuantidadeUsada,
                            DataHora = os.DataAbertura.AddHours(2), 
                            FuncionarioId = faker.PickRandom(funcIds),
                            OrdemServicoId = os.Id, 
                            Observacao = $"Saída para OS {os.NumeroSequencial}"
                        });
                    }
                }
            }
        }
    }
}