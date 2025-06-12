using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using ControleDeBarWebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ControleDeBarWebApp.Models
{
    public abstract class FormularioGarcomViewModel
    {
        [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
        [MinLength(3, ErrorMessage = "O campo \"Nome\" precisa conter ao menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O campo \"Nome\" precisa conter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo \"CPF\" é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
    ErrorMessage = "O campo \"CPF\" precisa seguir o formato: 000.000.000-00.")]
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

            foreach (var g in garsons)
            {
                var detalhesVM = g.ParaDetalhesVM();

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
