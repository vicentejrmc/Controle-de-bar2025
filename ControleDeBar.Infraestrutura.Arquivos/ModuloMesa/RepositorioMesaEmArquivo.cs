using controleDeBar.Dominio.ModuloMesa;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloMesa
{
    public class RepositorioMesaEmArquivo : RepositorioBaseEmArquivo<Mesa>, IRepositorioMesa
    {
        public RepositorioMesaEmArquivo(ContextoDados contexto) : base(contexto) { }

        protected override List<Mesa> ObterRegistros()
        {
            return contexto.Mesas;
        }
    }
}
