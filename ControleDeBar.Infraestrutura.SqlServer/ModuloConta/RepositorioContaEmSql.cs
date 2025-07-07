using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using controleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloConta
{
    public class RepositorioContaEmSql : IRepositorioConta
    {
        private readonly string connectionString =
  "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

        public void CadastrarConta(Conta novaConta)
        {
            var sqlInserirConta =
                @"INSERT INTO TBCONTA
                    (
                        [ID], 
                        [TITULAR], 
                        [MESA_ID], 
                        [GARCOM_ID], 
                        [ABERTURA], 
                        [FECHAMENTO],
                        [ESTAABERTA]
                    )
                VALUES
                    (
                        @ID,
                        @TITULAR,
                        @MESA_ID,
                        @GARCOM_ID,
                        @ABERTURA,
                        @FECHAMENTO,
                        @ESTAABERTA
                    )";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoInserir = new SqlCommand(sqlInserirConta, coneccaoComBanco);

            ConfigurarParametrosDeConta(novaConta, comandoInserir);
            coneccaoComBanco.Open();
            comandoInserir.ExecuteNonQuery();

            InserirPedidoNaConta(novaConta, coneccaoComBanco);
           
            coneccaoComBanco.Close();
        }

        private void ConfigurarParametrosDeConta(Conta conta, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", conta.Id);
            comando.Parameters.AddWithValue("TITULAR", conta.Titular);
            comando.Parameters.AddWithValue("MESA_ID", conta.Mesa.Id);
            comando.Parameters.AddWithValue("GARCOM_ID", conta.Mesa.Id);
            comando.Parameters.AddWithValue("ABERTURA", conta.Abertura);
            comando.Parameters.AddWithValue("FECHAMENTO", conta.Fechamento ==
                    DateTime.MinValue ? (object)DBNull.Value : conta.Fechamento);
            comando.Parameters.AddWithValue("ESTAABERTA", conta.EstaAberta);
        }

        public List<Conta> SelecionarContas()
        {
            var sqlSelecionarTodas =
                 @"SELECT
                    CT.[ID] AS CONTA_ID,
                    CT.[TITULAR],
                    CT.[ABERTURA],
                    CT.[FECHAMENTO],
                    CT.[ESTAABERTA],
                    MS.[ID] AS MESA_ID,
                    MS.[NUMERO] AS MESA_NUMERO,
                    MS.[CAPACIDADE] AS MESA_CAPACIDADE,
                    MS.[ESTAOCUPADA] AS MESA_OCUPADA,
                    GC.[ID] AS GARCOM_ID,
                    GC.[NOME] AS GARCOM_NOME,
                    GC.[CPF] AS GARCOM_CPF
                FROM
                    [TBCONTA] AS CT
                INNER JOIN
                    [TBMESA] AS MS ON CT.[MESA_ID] = MS.[ID]
                INNER JOIN
                    [TBGARCOM] AS GC ON CT.[GARCOM_ID] = GC.[ID]";


            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand comando = new SqlCommand(sqlSelecionarTodas, coneccaoComBanco);
            SqlDataReader reader = comando.ExecuteReader();

            var contas = new List<Conta>();

            while (reader.Read())
            {
                var conta = ConverterParaConta(reader);
                contas.Add(conta);
            }

            coneccaoComBanco.Close();

            return contas;
        }

        public List<Conta> SelecionarContasAbertas()
        {
            var sqlContasAbertas =
                 @"SELECT
                    CT.[ID] AS CONTA_ID,
                    CT.[TITULAR],
                    CT.[ABERTURA],
                    CT.[FECHAMENTO],
                    CT.[ESTAABERTA],
                    MS.[ID] AS MESA_ID,
                    MS.[NUMERO] AS MESA_NUMERO,
                    MS.[CAPACIDADE] AS MESA_CAPACIDADE,
                    MS.[ESTAOCUPADA] AS MESA_OCUPADA,
                    GC.[ID] AS GARCOM_ID,
                    GC.[NOME] AS GARCOM_NOME,
                    GC.[CPF] AS GARCOM_CPF
                FROM
                    [TBCONTA] AS CT
                INNER JOIN
                    [TBMESA] AS MS ON CT.[MESA_ID] = MS.[ID]
                INNER JOIN
                    [TBGARCOM] AS GC ON CT.[GARCOM_ID] = GC.[ID]
                WHERE
                    CT.[ESTAABERTA] = 1";

            var contas = new List<Conta>();

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comando = new SqlCommand(sqlContasAbertas, coneccaoComBanco);
            coneccaoComBanco.Open();
            var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                var conta = ConverterParaConta(reader);
                contas.Add(conta);
            }

            return contas;
        }

        public List<Conta> SelecionarContasFechadas()
        {
            var sqlContasFechadas =
                 @"SELECT
                    CT.[ID] AS CONTA_ID,
                    CT.[TITULAR],
                    CT.[ABERTURA],
                    CT.[FECHAMENTO],
                    CT.[ESTAABERTA],
                    MS.[ID] AS MESA_ID,
                    MS.[NUMERO] AS MESA_NUMERO,
                    MS.[CAPACIDADE] AS MESA_CAPACIDADE,
                    MS.[ESTAOCUPADA] AS MESA_OCUPADA,
                    GC.[ID] AS GARCOM_ID,
                    GC.[NOME] AS GARCOM_NOME,
                    GC.[CPF] AS GARCOM_CPF
                FROM
                    [TBCONTA] AS CT
                INNER JOIN
                    [TBMESA] AS MS ON CT.[MESA_ID] = MS.[ID]
                INNER JOIN
                    [TBGARCOM] AS GC ON CT.[GARCOM_ID] = GC.[ID]
                WHERE
                    CT.[ESTAABERTA] = 0";

            var contas = new List<Conta>();

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comando = new SqlCommand(sqlContasFechadas, coneccaoComBanco);
            coneccaoComBanco.Open();
            var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                var conta = ConverterParaConta(reader);
                contas.Add(conta);
            }

            return contas;
        }

        public List<Conta> SelecionarContasPorPeriodo(DateTime data)
        {
            var sqlSelecionar =
               @"SELECT
                    CT.[ID] AS CONTA_ID,
                    CT.[TITULAR],
                    CT.[ABERTURA],
                    CT.[FECHAMENTO],
                    CT.[ESTAABERTA],
                    MS.[ID] AS MESA_ID,
                    MS.[NUMERO] AS MESA_NUMERO,
                    MS.[CAPACIDADE] AS MESA_CAPACIDADE,
                    MS.[ESTAOCUPADA] AS MESA_OCUPADA,
                    GC.[ID] AS GARCOM_ID,
                    GC.[NOME] AS GARCOM_NOME,
                    GC.[CPF] AS GARCOM_CPF
                FROM
                    [TBCONTA] AS CT
                INNER JOIN
                    [TBMESA] AS MS ON CT.[MESA_ID] = MS.[ID]
                INNER JOIN
                    [TBGARCOM] AS GC ON CT.[GARCOM_ID] = GC.[ID]
                WHERE
                    CAST(CT.[ABERTURA] AS DATE) = @DATA";

            var contas = new List<Conta>();

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comando = new SqlCommand(sqlSelecionar, coneccaoComBanco);
            coneccaoComBanco.Open();
            var reader = comando.ExecuteReader();

            while (reader.Read())
            {
                var conta = ConverterParaConta(reader);
                contas.Add(conta);
            }

            return contas;
        }

        public Conta SelecionarPorId(Guid idRegistro)
        {
            var sqlSelecionarPorId =
                @"SELECT
                    CT.[ID] AS CONTA_ID,
                    CT.[TITULAR],
                    CT.[ABERTURA],
                    CT.[FECHAMENTO],
                    CT.[ESTAABERTA],
                    MS.[ID] AS MESA_ID,
                    MS.[NUMERO] AS MESA_NUMERO,
                    MS.[CAPACIDADE] AS MESA_CAPACIDADE,
                    MS.[ESTAOCUPADA] AS MESA_OCUPADA,
                    GC.[ID] AS GARCOM_ID,
                    GC.[NOME] AS GARCOM_NOME,
                    GC.[CPF] AS GARCOM_CPF
                FROM
                    [TBCONTA] AS CT
                INNER JOIN
                    [TBMESA] AS MS ON CT.[MESA_ID] = MS.[ID]
                INNER JOIN
                    [TBGARCOM] AS GC ON CT.[GARCOM_ID] = GC.[ID]
                WHERE
                    CT.[ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, coneccaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Conta conta = null;
            if (reader.Read())
                conta = ConverterParaConta(reader);

            return conta;
        }


        //Métodos Auxiliares
        private Conta ConverterParaConta(SqlDataReader reader)
        {
            var mesa = new Mesa(Convert.ToInt32(reader["MESA_NUMERO"]),
                Convert.ToInt32(reader["MESA_CAPACIDADE"]))
            {
                Id = Guid.Parse(reader["MESA_ID"].ToString()!),
                EstaOcupada = Convert.ToBoolean(reader["MESA_OCUPADA"])
            };

            var garcom = new Garcom(Convert.ToString(reader["GARCOM_NOME"])!,
                Convert.ToString(reader["GARCOM_CPF"])!)
            {
                Id = Guid.Parse(reader["GARCOM_ID"].ToString()!)
            };

            var conta = new Conta(Convert.ToString(reader["TITULAR"])!, mesa, garcom)
            {
                Id = Guid.Parse(reader["CONTA_ID"].ToString()!),
                Abertura = Convert.ToDateTime(reader["ABERTURA"]),
                Fechamento = reader["FECHAMENTO"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FECHAMENTO"]),
                EstaAberta = Convert.ToBoolean(reader["ESTAABERTA"])
            };

            conta.Pedidos = SelecionarPedidosPorContaId(conta.Id);

            return conta;
        }

        private List<Pedido> SelecionarPedidosPorContaId(Guid contaId)
        {
            var sqlSelecionar =
                 @"SELECT
                    P.[ID],
                    P.[PRODUTO_ID],
                    P.[QUANTIDADESOLICITADA],
                    PR.[NOME] AS PRODUTO_NOME,
                    PR.[VALOR] AS PRODUTO_VALOR
                FROM
                    TBPEDIDO AS P
                INNER JOIN
                    TBPRODUTO AS PR ON P.[PRODUTO_ID] = PR.[ID]
                WHERE
                    P.[CONTA_ID] = @CONTA_ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand comando = new SqlCommand(sqlSelecionar, coneccaoComBanco);

            comando.Parameters.AddWithValue("@CONTA_ID", contaId);
            var reader = comando.ExecuteReader();

            var pedidos = new List<Pedido>();

            while (reader.Read())
            {
                var produto = new Produto(
                    Convert.ToString(reader["PRODUTO_NOME"])!,
                    Convert.ToDecimal(reader["PRODUTO_VALOR"])
                )
                {
                    Id = Guid.Parse(reader["PRODUTO_ID"].ToString()!)
                };

                var pedido = new Pedido(produto, Convert.ToInt32(reader["QUANTIDADESOLICITADA"]))
                {
                    Id = Guid.Parse(reader["ID"].ToString()!)
                };

                pedidos.Add(pedido);
            }

            return pedidos;
        }

        private void InserirPedidoNaConta(Conta conta, SqlConnection conexao)
        {
            foreach (var pedido in conta.Pedidos)
            {
                var sqlInserirPedido =
            @"INSERT INTO TBPEDIDO
                (
                [ID],
                [CONTA_ID],
                [PRODUTO_ID],
                [QUANTIDADESOLICITADA]
                )
            VALUES
                (
                @ID,
                @CONTA_ID,
                @PRODUTO_ID,
                @QUANTIDADESOLICITADA
                )";

                using (var comandoPedido = new SqlCommand(sqlInserirPedido, conexao))
                {
                    comandoPedido.Parameters.AddWithValue("@ID", pedido.Id);
                    comandoPedido.Parameters.AddWithValue("@CONTA_ID", conta.Id);
                    comandoPedido.Parameters.AddWithValue("@PRODUTO_ID", pedido.Produto.Id);
                    comandoPedido.Parameters.AddWithValue("@QUANTIDADESOLICITADA", pedido.QuantidadeSolicitada);
                    comandoPedido.ExecuteNonQuery();
                }
            }
        }
    }
}
