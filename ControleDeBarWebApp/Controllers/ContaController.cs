using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using controleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using ControleDeBar.Infraestrutura.SqlServer.ModuloConta;
using ControleDeBar.Infraestrutura.SqlServer.ModuloGarcom;
using ControleDeBar.Infraestrutura.SqlServer.ModuloMesa;
using ControleDeBar.Infraestrutura.SqlServer.ModuloProduto;
using ControleDeBarWebApp.Extensions;
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
            repositorioConta = new RepositorioContaEmSql();
            repositorioGarcom = new RepositorioGarcomEmSql();
            repositorioMesa = new RepositorioMesaEmSql();
            repositorioProduto = new RepositorioProdutoEmSql();
        }

        [HttpGet]
        public IActionResult Index(string status)
        {
            List<Conta> registros;

            switch (status)
            {
                case "abertas":
                    registros = repositorioConta.SelecionarContasAbertas();
                    break;
                case "fechadas":
                    registros = repositorioConta.SelecionarContasFechadas();
                    break;
                default:
                    registros = repositorioConta.SelecionarContas();
                    break;
            }

            var visualizarVM = new VisualizarContasViewModel(registros);
            return View(visualizarVM);
        }

        [HttpGet("abrir")]
        public IActionResult Abrir()
        {
            var mesas = repositorioMesa.SelecionarRegistros();
            var garcons = repositorioGarcom.SelecionarRegistros();

            var abrirVM = new AbrirContaViewModel(mesas, garcons);

            return View(abrirVM);
        }

        [HttpPost("abrir")]
        [ValidateAntiForgeryToken]
        public IActionResult Abrir(AbrirContaViewModel abrirVM)
        {
            var registros = repositorioConta.SelecionarContas();

            foreach (var conta in registros)
            {
                if (conta.Titular.Equals(abrirVM.Titular) && conta.EstaAberta)
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe uma conta aberta para este titular.");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(abrirVM);
            }

            var mesas = repositorioMesa.SelecionarRegistros();
            var garcons = repositorioGarcom.SelecionarRegistros();
            var entidade = abrirVM.ParaEntidade(mesas, garcons);
            repositorioConta.CadastrarConta(entidade);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet, Route("/contas/{id:guid}/fechar")]
        public IActionResult Fechar(Guid id)
        {
            var registro = repositorioConta.SelecionarPorId(id);

            var fecharContaVM = new FecharContaViewModel(
                registro.Id,
                registro.Titular,
                registro.Mesa.Numero,
                registro.Garcom.Nome,
                registro.CalcularValorTotal()
            );

            return View(fecharContaVM);
        }

        [HttpPost, Route("/contas/{id:guid}/fechar")]
        public IActionResult FecharConfirmado(Guid id)
        {
            var registroSelecionado = repositorioConta.SelecionarPorId(id);
            registroSelecionado.Fechar();
            contextoDados.Salvar();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet, Route("/contas/{id:guid}/gerenciar-pedidos")]
        public IActionResult GerenciarPedidos(Guid id)
        {
            var contaSelecionada = repositorioConta.SelecionarPorId(id);
            var produtos = repositorioProduto.SelecionarRegistros();
            var gerenciarPedidosVm = new GerenciarPedidosViewModel(contaSelecionada, produtos);

            return View(gerenciarPedidosVm);
        }

        [HttpPost, Route("/contas/{id:guid}/adicionar-pedido")]
        public IActionResult AdicionarPedido(Guid id, AdicionarPedidoViewModel adicionarPedidoVm)
        {
            var contaSelecionada = repositorioConta.SelecionarPorId(id);
            var produtoSelecionado = repositorioProduto.SelecionarRegistroPorId(adicionarPedidoVm.IdProduto);


            var pedido = contaSelecionada.RegistrarPedido(
                produtoSelecionado,
                adicionarPedidoVm.QuantidadeSolicitada
            );

            repositorioConta.AdicionarPedido(pedido);

            var produtos = repositorioProduto.SelecionarRegistros();
            var gerenciarPedidosVm = new GerenciarPedidosViewModel(contaSelecionada, produtos);

            return View("GerenciarPedidos", gerenciarPedidosVm);
        }

        [HttpPost, Route("/contas/{id:guid}/remover-pedido/{idPedido:guid}")]
        public IActionResult RemoverPedido(Guid id, Guid idPedido)
        {
            var contaSelecionada = repositorioConta.SelecionarPorId(id);
            var pedidoRemovido = contaSelecionada.RemoverPedido(idPedido);
            contextoDados.Salvar();
            repositorioConta.RemoverPedido(pedidoRemovido);
            var produtos = repositorioProduto.SelecionarRegistros();
            var gerenciarPedidosVm = new GerenciarPedidosViewModel(contaSelecionada, produtos);

            return View("GerenciarPedidos", gerenciarPedidosVm);
        }

        [HttpGet("faturamento")]
        public IActionResult Faturamento(DateTime? data)
        {
            if (!data.HasValue)
            return View();

            var contas = repositorioConta.SelecionarContasPorPeriodo(data.GetValueOrDefault());
            var faturamentoVM = new FaturamentoViewModel(contas);

            return View(faturamentoVM);
        }
    }
}
