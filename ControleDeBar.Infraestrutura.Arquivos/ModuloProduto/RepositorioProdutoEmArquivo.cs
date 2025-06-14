using controleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloProduto
{
    public class RepositorioProdutoEmArquivo : RepositorioBaseEmArquivo<Produto>, IRepositorioProduto
    {
        public RepositorioProdutoEmArquivo(ContextoDados contexto) : base(contexto)
        {
        }

        protected override List<Produto> ObterRegistros()
        {
            return contexto.Produtos;
        }
    }
}
