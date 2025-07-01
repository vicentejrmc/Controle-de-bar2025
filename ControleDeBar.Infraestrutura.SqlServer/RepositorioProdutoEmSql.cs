using controleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer
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
            throw new NotImplementedException();
        }

        public bool ExcluirRegistro(Guid idRegistro)
        {
            throw new NotImplementedException();
        }

        public Produto SelecionarRegistroPorId(Guid idRegistro)
        {
            throw new NotImplementedException();
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
        } //ok

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
