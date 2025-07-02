using controleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloProduto
{
    public class RepositorioProdutoEmSql : IRepositorioProduto
    {
        private readonly string connectionString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

        public void CadastrarRegistro(Produto novoRegistro)
        {
            var sqlInserir =
                @"INSERT INTO [TBPRODUTO]
                    (
                       [ID],
                       [NOME],
                       [VALOR]
                    )
                    VALUES
                    (
                       @ID,
                       @NOME,
                       @VALOR  
                    );";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoInserir = new SqlCommand(sqlInserir, coneccaoComBanco);

            ConfigurarParametrosProduto(novoRegistro, comandoInserir);

            coneccaoComBanco.Open();
            comandoInserir.ExecuteNonQuery();

            coneccaoComBanco.Close();
        } 

        public bool EditarRegistro(Guid idRegistro, Produto registroEditado) 
        {
            var sqlEditar =
               @"UPDATE [TBPRODUTO]
                    SET
                        [NOME] = @NOME,
                        [VALOR] = @VALOR
                    WHERE
                        [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, coneccaoComBanco);

            registroEditado.Id = idRegistro;
            ConfigurarParametrosProduto(registroEditado, comandoEdicao);
            coneccaoComBanco.Open();

            var linhasAfetadas = comandoEdicao.ExecuteNonQuery();
            coneccaoComBanco.Close();

            return linhasAfetadas > 0;
        }

        public bool ExcluirRegistro(Guid idRegistro)
        {
            var sqlEcluir =
                @"DELETE FROM [TBPRODUTO]
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

        public Produto SelecionarRegistroPorId(Guid idRegistro)
        {
            var sqlSelecionarPorId =
                @"SELECT
                    [ID],
                    [NOME],
                    [VALOR]
                FROM
                    [TBPRODUTO]
                WHERE
                    [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, coneccaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Produto produto = null;
            if (reader.Read())
                produto = ConverterParaProduto(reader);

            return produto;
        }

        public List<Produto> SelecionarRegistros()
        {
            var sqlSelecionarTodos =
                @"SELECT
                    [ID],
                    [NOME],
                    [VALOR]
                FROM
                    [TBPRODUTO]";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand cmd = new SqlCommand(sqlSelecionarTodos, coneccaoComBanco);
            SqlDataReader reader = cmd.ExecuteReader();

            var produtos = new List<Produto>();

            while (reader.Read())
            {
                var produto = ConverterParaProduto(reader);
                produtos.Add(produto);
            }

            coneccaoComBanco.Close();

            return produtos;
        }

        private Produto ConverterParaProduto(SqlDataReader reader)
        {
            var produto = new Produto(
               Convert.ToString(reader["NOME"])!,
               Convert.ToDecimal(reader["VALOR"])
            );

            produto.Id = Guid.Parse(reader["ID"].ToString()!);

            return produto;
        }

        private void ConfigurarParametrosProduto(Produto produto, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", produto.Id);
            comando.Parameters.AddWithValue("NOME", produto.Nome);
            comando.Parameters.AddWithValue("VALOR", produto.Valor);
        }
    }
}
