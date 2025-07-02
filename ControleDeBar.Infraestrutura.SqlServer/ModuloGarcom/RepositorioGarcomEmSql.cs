using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloGarcom
{
    public class RepositorioGarcomEmSql : IRepositorioGarcom
    {
        private readonly string connectionString =
           "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

        public void CadastrarRegistro(Garcom novoRegistro)
        {
            var sqlInserir =
                @"INSERT INTO [TBGARCOM]
                    (
                       [ID],
                       [NOME],
                       [CPF]
                    )
                    VALUES
                    (
                       @ID,
                       @NOME,
                       @CPF 
                    );";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoInserir = new SqlCommand(sqlInserir, coneccaoComBanco);

            ConfigurarParametrosGarcom(novoRegistro, comandoInserir);

            coneccaoComBanco.Open();
            comandoInserir.ExecuteNonQuery();

            coneccaoComBanco.Close();
        }

        public bool EditarRegistro(Guid idRegistro, Garcom registroEditado)
        {
            throw new NotImplementedException();
        }

        public bool ExcluirRegistro(Guid idRegistro)
        {
            throw new NotImplementedException();
        }

        public Garcom SelecionarRegistroPorId(Guid idRegistro)
        {
            throw new NotImplementedException();
        }

        public List<Garcom> SelecionarRegistros()
        {
            var sqlSelecionarTodos =
                @"SELECT
                    [ID],
                    [NOME],
                    [CPF]
                FROM
                    [TBGARCOM]";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand cmd = new SqlCommand(sqlSelecionarTodos, coneccaoComBanco);
            SqlDataReader reader = cmd.ExecuteReader();

            var garcons = new List<Garcom>();

            while (reader.Read())
            {
                var garcom = ConverterParaGarcom(reader);
                garcons.Add(garcom);
            }

            coneccaoComBanco.Close();

            return garcons;
        }

        private Garcom ConverterParaGarcom(SqlDataReader reader)
        {
            var garcom = new Garcom(
               Convert.ToString(reader["NOME"])!,
               Convert.ToString(reader["CPF"])!
            );

            garcom.Id = Guid.Parse(reader["ID"].ToString()!);

            return garcom;
        }

        private void ConfigurarParametrosGarcom(Garcom garcom, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", garcom.Id);
            comando.Parameters.AddWithValue("NOME", garcom.Nome);
            comando.Parameters.AddWithValue("CPF", garcom.Cpf);
        }
    }
}
