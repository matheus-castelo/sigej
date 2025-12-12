using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using sigej.db.repositories;
using sigej.db.repositories.AreaCampus;
using sigej.db.repositories.AreaCampus.Interfaces;
using sigej.db.repositories.Equipes;
using sigej.db.repositories.Equipes.Interfaces;
using sigej.db.repositories.OrdemServico;
using sigej.db.repositories.OrdemServico.Interfaces;
using sigej.db.repositories.ProdutoEstoque;
using sigej.db.repositories.ProdutoEstoque.Interfaces;
using sigej.db.repositories.QuadroFuncionarios;
using sigej.db.repositories.QuadroFuncionarios.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITipoAreaCampusRepository, TipoAreaCampusRepository>();
builder.Services.AddScoped<IAreaCampusRepository, AreaCampusRepository>();

builder.Services.AddScoped<IEquipeRepository, EquipeRepository>();
builder.Services.AddScoped<IEquipeMembroRepository, EquipeMembroRepository>();

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<ISetorRepository, SetorRepository>();
builder.Services.AddScoped<ITipoFuncionarioRepository, TipoFuncionarioRepository>();

builder.Services.AddScoped<ICategoriaMaterialRepository, CategoriaMaterialRepository>();
builder.Services.AddScoped<IUnidadeMedidaRepository, UnidadeMedidaRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<ICorRepository, CorRepository>();
builder.Services.AddScoped<ITamanhoRepository, TamanhoRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoVariacaoRepository, ProdutoVariacaoRepository>();

builder.Services.AddScoped<ILocalEstoqueRepository, LocalEstoqueRepository>();
builder.Services.AddScoped<IEstoqueRepository, EstoqueRepository>();
builder.Services.AddScoped<ITipoMovimentoEstoqueRepository, TipoMovimentoEstoqueRepository>();
builder.Services.AddScoped<IMovimentoEstoqueRepository, MovimentoEstoqueRepository>();

builder.Services.AddScoped<ITipoOrdemServicoRepository, TipoOrdemServicoRepository>();
builder.Services.AddScoped<IStatusOrdemServicoRepository, StatusOrdemServicoRepository>();
builder.Services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
builder.Services.AddScoped<IItemOrdemServicoRepository, ItemOrdemServicoRepository>();
builder.Services.AddScoped<IAndamentoOrdemServicoRepository, AndamentoOrdemServicoRepository>();

builder.Services.AddScoped<RelatorioRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();