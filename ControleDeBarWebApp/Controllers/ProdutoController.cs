using controleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;
using ControleDeBarWebApp.Extensions;
using ControleDeBarWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBarWebApp.Controllers
{
    [Route("produtos")]
    public class ProdutoController : Controller
    {
        private readonly ContextoDados contextoDados;
        private readonly IRepositorioProduto repositorioProduto;

        public ProdutoController()
        {
            contextoDados = new ContextoDados(true);
            repositorioProduto = new RepositorioProdutoEmArquivo(contextoDados);
        }

        [HttpGet]
        public IActionResult Index()
        {
           var registros = repositorioProduto.SelecionarRegistros();

            var visualizarVM = new VisualizarProdutosViewModel(registros);

            return View(visualizarVM);
        }

        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            var cadastrarVM = new CadastrarProdutoViewModel();
            return View(cadastrarVM);
        }

        [HttpPost("cadastrar")]
        public IActionResult Cadastrar(CadastrarProdutoViewModel cadastrarVM)
        {
            var registros = repositorioProduto.SelecionarRegistros();

            foreach (var produto in registros)
            {
                if (produto.Nome.Equals(cadastrarVM.Nome))
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe um produto cadastrado com esse nome.");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(cadastrarVM);
            }

            var novoProduto = cadastrarVM.ParaEntidade();
            repositorioProduto.CadastrarRegistro(novoProduto);

            return RedirectToAction(nameof(Index));
        }
    }
}
