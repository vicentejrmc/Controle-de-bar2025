using controleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;
using ControleDeBarWebApp.Extensions;
using ControleDeBarWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

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
        [ValidateAntiForgeryToken]
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

        [HttpGet("editar/{id:guid}")]
        public IActionResult Editar(Guid id)
        {
            var registro = repositorioProduto.SelecionarRegistroPorId(id);
            var editarVM = new EditarProdutoViewModel(id, registro.Nome, registro.Valor);
            return View(editarVM);
        }

        [HttpPost("editar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Guid id, EditarProdutoViewModel editarVM)
        {
            var registros = repositorioProduto.SelecionarRegistros();

            foreach (var item in registros)
            {
                if (!item.Id.Equals(id) && item.Nome.Equals(editarVM.Nome))
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe um produto registrado com este nome.");
                    break;
                }
            }

            if (!ModelState.IsValid)
                return View(editarVM);

            var entidadeEditada = editarVM.ParaEntidade();
            repositorioProduto.EditarRegistro(id, entidadeEditada);
            return RedirectToAction(nameof(Index));
        }
    }
}
