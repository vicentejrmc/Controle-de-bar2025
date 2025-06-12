using controleDeBar.Dominio.ModuloGarcom;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloGarcom
{
    public class RepositorioGarcomEmArquivo : RepositorioBaseEmArquivo<Garcom>, IRepositorioGarcom
    {
        public RepositorioGarcomEmArquivo(ContextoDados contexto) : base(contexto) { }
        protected override List<Garcom> ObterRegistros()
        {
            return contexto.Garcons;
        }
    }
}
