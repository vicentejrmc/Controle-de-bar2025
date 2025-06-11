using controleDeBar.Dominio.Compatilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controleDeBar.Dominio.ModuloMesa
{
    public class Mesa : EntidadeBase<Mesa>
    {
        public int Numero { get; set; }
        public int Capacidade { get; set; }

        public Mesa() {}

        public Mesa(int numero, int capacidade) : this()
        {
            Id = Guid.NewGuid();
            Numero = numero;
            Capacidade = capacidade;
        }

        public override void AtualizarRegistro(Mesa registroEditado)
        {
            Numero = registroEditado.Numero;
            Capacidade = registroEditado.Capacidade;
        }
    }
}
