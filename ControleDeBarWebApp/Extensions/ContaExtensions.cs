using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using ControleDeBarWebApp.Models;

namespace ControleDeBarWebApp.Extensions
{
    public static class ContaExtensions
    {
        public static Conta ParaEntidade(this AbrirContaViewModel abrirVM, List<Mesa> mesas, List<Garcom> garcoms)
        {
            Mesa? mesaSelecionada = null;

            foreach (var mesa in mesas)
            {
                if (mesa.Id == abrirVM.MesaId)
                {
                    mesaSelecionada = mesa;
                }
            }

            Garcom? garcomSelecionado = null;

            foreach (var garcom in garcoms)
            {
                if (garcom.Id == abrirVM.GarcomId)
                {
                    garcomSelecionado = garcom;
                }
            }

            return new Conta(abrirVM.Titular, mesaSelecionada, garcomSelecionado);
        }
        public static DetalhesContaViewModel ParaDetalhesVM(this Conta conta)
        {
            return new DetalhesContaViewModel(
                    conta.Id,
                    conta.Titular,
                    conta.Mesa.Numero,
                    conta.Garcom.Nome,
                    conta.EstaAberta,
                    conta.CalcularValorTotal(),
                    conta.Pedidos
            );
        }
    }
}
