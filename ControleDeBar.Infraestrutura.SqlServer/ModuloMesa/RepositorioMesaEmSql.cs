using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloMesa
{
    public class RepositorioMesaEmSql : IRepositorioMesa
    {
        private readonly string connectionString =
          "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

        public void CadastrarRegistro(Mesa novoRegistro)
        {
            var sqlInserir =
                @"INSERT INTO [TBMESA]
                    (
                       [ID],
                       [NUMERO],
                       [CAPACIDADE],
                       [ESTAOCUPADA]
                    )
                    VALUES
                    (
                       @ID,
                       @NUMERO,
                       @CAPACIDADE,
                       @ESTAOCUPADA
                    );";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoInserir = new SqlCommand(sqlInserir, coneccaoComBanco);

            ConfigurarParametrosMesa(novoRegistro, comandoInserir);

            coneccaoComBanco.Open();
            comandoInserir.ExecuteNonQuery();

            coneccaoComBanco.Close();
        }

        public bool EditarRegistro(Guid idRegistro, Mesa registroEditado)
        {
            var sqlEditar =
               @"UPDATE [TBMESA]
                    SET
                        [NUMERO] = @NUMERO,
                        [CAPACIDADE] = @CAPACIDADE,
                        [ESTAOCUPADA] = @ESTAOCUPADA
                    WHERE
                        [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, coneccaoComBanco);

            registroEditado.Id = idRegistro;
            ConfigurarParametrosMesa(registroEditado, comandoEdicao);
            coneccaoComBanco.Open();

            var linhasAfetadas = comandoEdicao.ExecuteNonQuery();
            coneccaoComBanco.Close();

            return linhasAfetadas > 0;
        }

        public bool ExcluirRegistro(Guid idRegistro)
        {
            var sqlEcluir =
                 @"DELETE FROM [TBMESA  ]
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

        public Mesa SelecionarRegistroPorId(Guid idRegistro)    
        {
            var sqlSelecionarPorId =
                @"SELECT
                    [ID],
                    [NUMERO],
                    [CAPACIDADE],
                    [ESTAOCUPADA]
                FROM
                    [TBMESA]
                WHERE
                    [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, coneccaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Mesa mesa = null;
            if (reader.Read())
                mesa = ConverterParaMesa(reader);

            return mesa;
        }

        public List<Mesa> SelecionarRegistros()
        {
            var sqlSelecionarTodos =
               @"SELECT
                    [ID],
                    [NUMERO],
                    [CAPACIDADE],
                    [ESTAOCUPADA]
                FROM
                    [TBMESA]";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand cmd = new SqlCommand(sqlSelecionarTodos, coneccaoComBanco);
            SqlDataReader reader = cmd.ExecuteReader();

            var mesas = new List<Mesa>();

            while (reader.Read())
            {
                var mesa = ConverterParaMesa(reader);
                mesas.Add(mesa);
            }

            coneccaoComBanco.Close();

            return mesas;
        }

        private Mesa ConverterParaMesa(SqlDataReader reader)
        {
           var mesa = new Mesa(
                 Convert.ToInt32(reader["NUMERO"]),
                 Convert.ToInt32(reader["CAPACIDADE"])
               );

            mesa.Id = Guid.Parse(reader["ID"].ToString()!);
            mesa.EstaOcupada = Convert.ToBoolean(reader["ESTAOCUPADA"]);

            return mesa;
        }

        private void ConfigurarParametrosMesa(Mesa mesa, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", mesa.Id);
            comando.Parameters.AddWithValue("NUMERO", mesa.Numero);
            comando.Parameters.AddWithValue("CAPACIDADE", mesa.Capacidade);
            comando.Parameters.AddWithValue("ESTAOCUPADA", mesa.EstaOcupada);

        }
    }
}
