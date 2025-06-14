using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using controleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloConta;
using ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;
using ControleDeBar.Infraestrutura.Arquivos.ModuloMesa;
using ControleDeBar.Infraestrutura.Arquivos.ModuloProduto;
using ControleDeBarWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBarWebApp.Controllers
{
    [Route("contas")]
    public class ContaController : Controller
    {
        private readonly ContextoDados contextoDados;
        private readonly IRepositorioConta repositorioConta;
        private readonly IRepositorioGarcom repositorioGarcom;
        private readonly IRepositorioMesa repositorioMesa;
        private readonly IRepositorioProduto repositorioProduto;

        public ContaController()
        {
            contextoDados = new ContextoDados(true);
            repositorioConta = new RepositorioContaEmArquivo(contextoDados);
            repositorioGarcom = new RepositorioGarcomEmArquivo(contextoDados);
            repositorioMesa = new RepositorioMesaEmArquivo(contextoDados);
            repositorioProduto = new RepositorioProdutoEmArquivo(contextoDados);
        }

        [HttpGet]
        public IActionResult Index(string status)
        {
            List<Conta> registros;

            switch(status)
            {
                case "abertas": registros = repositorioConta.SelecionarContasAbertas();
                    break;
                case "fechadas": registros = repositorioConta.SelecionarContasFechadas();
                    break;
                default: registros = repositorioConta.SelecionarContas();
                    break;    
            }

            var contas = repositorioConta.SelecionarContas();
            var visualizarVM = new VisualizarContasViewModel(contas);
            return View(visualizarVM);
        }
    }
}
