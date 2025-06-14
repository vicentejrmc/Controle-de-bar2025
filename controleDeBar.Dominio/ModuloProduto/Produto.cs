using controleDeBar.Dominio.Compatilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controleDeBar.Dominio.ModuloProduto
{
    public class Produto : EntidadeBase<Produto>
    {
        public string Nome { get; set; }
        public decimal Valor{ get; set; }

        public Produto(){}

        public Produto(string nome, decimal valor)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Valor = valor;
        }

        public override void AtualizarRegistro(Produto registroEditado)
        {
            Nome = registroEditado.Nome;
            Valor = registroEditado.Valor;
        }
    }
}
