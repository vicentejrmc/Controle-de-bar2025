using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controleDeBar.Dominio.ModuloConta
{
    public interface IRepositorioConta 
    {
        void CadastrarConta(Conta conta);
        Conta SelecionarPorId(Guid idRegistro);
        List<Conta> SelecionarContas();
        List<Conta> SelecionarContasAbertas();
        List<Conta> SelecionarContasFechadas();
        List<Conta> SelecionarContasPorPeriodo(DateTime data);
        void AdicionarPedido(Pedido pedido);
        void RemoverPedido(Pedido pedidoRemovido);
    }
}
