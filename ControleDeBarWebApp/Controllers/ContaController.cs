using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using controleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBarWebApp.Controllers
{
    public class ContaController : Controller
    {
        private readonly ContextoDados contextoDados;
        private readonly IRepositorioConta repositorioConta;
        private readonly IRepositorioGarcom repositorioGarcom;
        private readonly IRepositorioMesa repositorioMesa;
        private readonly IRepositorioProduto repositorioProduto;

        public IActionResult Index()
        {

            return View();
        }
    }
}
