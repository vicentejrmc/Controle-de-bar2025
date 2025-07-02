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
            var sqlEditar =
                @"UPDATE [TBGARCOM]
                    SET
                        [NOME] = @NOME,
                        [CPF] = @CPF
                    WHERE
                        [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, coneccaoComBanco);

            registroEditado.Id = idRegistro;
            ConfigurarParametrosGarcom(registroEditado, comandoEdicao);
            coneccaoComBanco.Open();

            var linhasAfetadas = comandoEdicao.ExecuteNonQuery();
            coneccaoComBanco.Close();

            return linhasAfetadas > 0;
        }

        public bool ExcluirRegistro(Guid idRegistro)
        {
            var sqlEcluir =
                  @"DELETE FROM [TBGARCOM]
                    WHERE
                        [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoExclusao = new SqlCommand(sqlEcluir, coneccaoComBanco);
            comandoExclusao.Parameters.AddWithValue("ID", idRegistro);
            coneccaoComBanco.Open();

            var linhasAfetadas = comandoExclusao.ExecuteNonQuery();
            coneccaoComBanco.Close();

            return linhasAfetadas > 0;
        }

        public Garcom SelecionarRegistroPorId(Guid idRegistro)
        {
            var sqlSelecionarPorId =
                @"SELECT
                    [ID],
                    [NOME],
                    [CPF]
                FROM
                    [TBGARCOM]
                WHERE
                    [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, coneccaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Garcom garcom = null;
            if (reader.Read())
                garcom = ConverterParaGarcom(reader);

            return garcom;
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
