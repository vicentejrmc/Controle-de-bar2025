using controleDeBar.Dominio.ModuloConta;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloConta
{
    public class RepositorioContaEmSql : IRepositorioConta
    {
        private readonly string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

        public void CadastrarConta(Conta conta)
        {
            throw new NotImplementedException();
        }

        public List<Conta> SelecionarContas()
        {
            throw new NotImplementedException();
        }

        public List<Conta> SelecionarContasAbertas()
        {
            throw new NotImplementedException();
        }

        public List<Conta> SelecionarContasFechadas()
        {
            throw new NotImplementedException();
        }

        public List<Conta> SelecionarContasPorPeriodo(DateTime data)
        {
            throw new NotImplementedException();
        }

        public Conta SelecionarPorId(Guid idRegistro)
        {
            throw new NotImplementedException();
        }
    }
}
