using controleDeBar.Dominio.ModuloGarcom;
using ControleDeBarWebApp.Extensions;

namespace ControleDeBarWebApp.Models
{
    public abstract class FormularioGarcomViewModel
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }

    public class CadastrarGarcomViewModel : FormularioGarcomViewModel 
    {
        public CadastrarGarcomViewModel() { }

        public CadastrarGarcomViewModel(string nome, string cpf) : this()
        {
            Nome = nome;
            Cpf = cpf;
        }
    }

    public class EditarGarcomViewModel : FormularioGarcomViewModel
    {
        public Guid Id { get; set; }

        public EditarGarcomViewModel() { }

        public EditarGarcomViewModel(Guid id, string nome, string cpf) : this()
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
        }
    }

    public class ExcluirGarcomViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public ExcluirGarcomViewModel() { }

        public ExcluirGarcomViewModel(Guid id, string nome) : this()
        {
            Id = id;
            Nome = nome;
        }
    }

    public class VisualizarGarcomViewModel
    {
        public List<DetalhesGarcomViewModel> Registros { get; }

        public VisualizarGarcomViewModel(List<Garcom> garsons)
        {
            Registros = [];

            foreach (var m in garsons)
            {
                var detalhesVM = m.ParaDetalhesVM();

                Registros.Add(detalhesVM);
            }
        }
    }

    public class DetalhesGarcomViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }

        public DetalhesGarcomViewModel(Guid id, string nome, string cpf)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
        }
    }

    public class SelecionarGarcomViewModel
    {
        public Guid Id { get; set; }
        public string Nome{ get; set; }

        public SelecionarGarcomViewModel(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
