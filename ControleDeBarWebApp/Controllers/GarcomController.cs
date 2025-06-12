using controleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom;
using ControleDeBarWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeBarWebApp.Controllers
{
    [Route("garcons")]
    public class GarcomController : Controller
    {
        private readonly ContextoDados contextoDados;
        private readonly IRepositorioGarcom repositorioGarcom;

        public GarcomController()
        {
            contextoDados = new ContextoDados(true);
            repositorioGarcom = new RepositorioGarcomEmArquivo(contextoDados);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var registros = repositorioGarcom.SelecionarRegistros();

            var visualizarVM = new VisualizarGarcomViewModel(registros);

            return View(visualizarVM);
        }
    }
}
