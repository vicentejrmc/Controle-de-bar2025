
using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using controleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloConta
{
    public class RepositorioContaEmSql : IRepositorioConta
    {
        private readonly string connectionString =
           "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

        public void CadastrarConta(Conta conta)
        {
            var sqlInserir =
                @"INSERT INTO [TBCONTA]
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
                );";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoInserir = new SqlCommand(sqlInserir, coneccaoComBanco);

            ConfigurarParametrosConta(conta, comandoInserir);

            coneccaoComBanco.Open();
            comandoInserir.ExecuteNonQuery();

            coneccaoComBanco.Close();
        }

        public List<Conta> SelecionarContas()
        {
            var sqlSelecionarTodos =
                 @"SELECT
                    [ID], 
                    [TITULAR], 
                    [MESA_ID],
                    [GARCOM_ID],
                    [ABERTURA],
                    [FECHAMENTO],
                    [ESTAABERTA]
                 FROM
                    [TBCONTA];";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand cmd = new SqlCommand(sqlSelecionarTodos, coneccaoComBanco);
            SqlDataReader reader = cmd.ExecuteReader();

            var contas = new List<Conta>(); 

            while (reader.Read())
            {
                var conta = (Conta)ConverterParaConta(reader);
                contas.Add(conta);
            }

            coneccaoComBanco.Close();

            return contas;
        }

        public List<Conta> SelecionarContasAbertas()
        {
            var sqlSelecionarFechadas =
                @"SELECT
                    [ID], 
                    [TITULAR], 
                    [MESA_ID],
                    [GARCOM_ID],
                    [ABERTURA],
                    [FECHAMENTO],
                    [ESTAABERTA]
                FROM
                    [TBCONTA]
                WHERE
                    CAST[ESTAABERTA] = 1";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand cmd = new SqlCommand(sqlSelecionarFechadas, coneccaoComBanco);
            cmd.Parameters.AddWithValue("@ESTAABERTA", true);
            SqlDataReader reader = cmd.ExecuteReader();

            var contas = new List<Conta>();
            while (reader.Read())
            {
                var conta = (Conta)ConverterParaConta(reader);
                contas.Add(conta);
            }

            return contas;
        }

        public List<Conta> SelecionarContasFechadas()
        {
            var sqlSelecionarFechadas =
                @"SELECT
                    [ID], 
                    [TITULAR], 
                    [MESA_ID],
                    [GARCOM_ID],
                    [ABERTURA],
                    [FECHAMENTO],
                    [ESTAABERTA]
                FROM
                    [TBCONTA]
                WHERE
                    CAST[ESTAABERTA] = 0";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand cmd = new SqlCommand(sqlSelecionarFechadas, coneccaoComBanco);
            cmd.Parameters.AddWithValue("@ESTAABERTA", false);
            SqlDataReader reader = cmd.ExecuteReader();

            var contas = new List<Conta>();
            while (reader.Read())
            {
                var conta = (Conta)ConverterParaConta(reader);
                contas.Add(conta);
            }

            return contas;
        }

        public List<Conta> SelecionarContasPorPeriodo(DateTime data)
        {
            var sqlSelecionarPorPeriodo =
               @"SELECT
                    [ID], 
                    [TITULAR], 
                    [MESA_ID],
                    [GARCOM_ID],
                    [ABERTURA],
                    [FECHAMENTO],
                    [ESTAABERTA]
                FROM
                    [TBCONTA]
                WHERE
                    CAST([ABERTURA] AS DATE) = @DATA";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            coneccaoComBanco.Open();
            SqlCommand cmd = new SqlCommand(sqlSelecionarPorPeriodo, coneccaoComBanco);
            cmd.Parameters.AddWithValue("@DATA", data.Date);
            SqlDataReader reader = cmd.ExecuteReader();

            var contas = new List<Conta>();
            while (reader.Read())
            {
                var conta = (Conta)ConverterParaConta(reader);
                contas.Add(conta);
            }

            return contas;
        }

        public Conta SelecionarPorId(Guid idRegistro)
        {
            var sqlSelecionarPorId =
                 @"SELECT
                    [ID], 
                    [TITULAR], 
                    [MESA_ID],
                    [GARCOM_ID],
                    [ABERTURA],
                    [FECHAMENTO],
                    [ESTAABERTA]
                 FROM
                    [TBCONTA]
                 WHERE
                    [ID] = @ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, coneccaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Conta conta = null;
            if (reader.Read())
                conta = (Conta)ConverterParaConta(reader);

            return conta;

        }

        //Métodos Auxiliares
        public void AdicionarPedido(Pedido pedido)
        {
            var sqlAdicionarPedidido =
                @"INSERT INTO [TBPEDIDO]
                (
                    [ID],
                    [PRODUTO_ID],
                    [QUANTIDADE],
                    [CONTA_ID]
                )
                VALUES
                (
                    @ID,
                    @PRODUTO_ID,
                    @QUANTIDADE,
                    @CONTA_ID
                );";

            SqlConnection conexaoComBanco = new SqlConnection(connectionString);
            SqlCommand cmdAdicionar = new SqlCommand(sqlAdicionarPedidido, conexaoComBanco);
            ConfigurarParametrosPedido(cmdAdicionar, pedido);
            conexaoComBanco.Open();
            cmdAdicionar.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        private void CarregarPedidos(Conta conta)
        {
            var sqlPedidosDaConta =
                 @"SELECT
                      [ID],
                      [PRODUTO_ID],
                      [QUANTIDADE],
                      [CONTA_ID]
                 FROM
                      [TBPEDIDO]
                 WHERE
                      [CONTA_ID] = @CONTA_ID";

            SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sqlPedidosDaConta, coneccaoComBanco);
            cmd.Parameters.AddWithValue("CONTA_ID", conta.Id);
            coneccaoComBanco.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var pedido = ConverterParaPedido(reader, conta);
                conta.Pedidos.Add(pedido);
            }

            coneccaoComBanco.Close();
        }

        private Pedido ConverterParaPedido(SqlDataReader reader, Conta conta)
        {
            var produtoId = Guid.Parse(reader["PRODUTO"].ToString()!);
            var produto = (Produto)SelecionarProdutoPorId(produtoId);
            var quantidade = Convert.ToInt32(reader["QUANTIDADE"]);

            var pedido = new Pedido(produto, quantidade)
            {
                Id = Guid.Parse(reader["ID"].ToString()!)
            };
                
            return pedido;
        }

        private object SelecionarProdutoPorId(Guid produtoId)
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

            comandoSelecao.Parameters.AddWithValue("ID", produtoId);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Produto produto = null;
            if (reader.Read())
                produto = ConverterParaProduto(reader);

            return produto;
        }

        private object SelecionarMesaPorId(Guid mesaId)
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

            comandoSelecao.Parameters.AddWithValue("ID", mesaId);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Mesa mesa = null;
            if (reader.Read())
                mesa = ConverterParaMesa(reader);

            return mesa;
        }

        private object SelecionarGarcomPorId(Guid garcomId)
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

            comandoSelecao.Parameters.AddWithValue("ID", garcomId);

            coneccaoComBanco.Open();
            SqlDataReader reader = comandoSelecao.ExecuteReader();

            Garcom garcom = null;
            if (reader.Read())
                garcom = ConverterParaGarcom(reader);

            return garcom;
        }

        private Mesa? ConverterParaMesa(SqlDataReader reader)
        {
            var mesa = new Mesa(
                Convert.ToInt32(reader["NUMERO"]),
                Convert.ToInt32(reader["CAPACIDADE"])
              );

            mesa.Id = Guid.Parse(reader["ID"].ToString()!);
            mesa.EstaOcupada = Convert.ToBoolean(reader["ESTAOCUPADA"]);

            return mesa;
        }

        private Garcom? ConverterParaGarcom(SqlDataReader reader)
        {
            var garcom = new Garcom(
               Convert.ToString(reader["NOME"])!,
               Convert.ToString(reader["CPF"])!
            );

            garcom.Id = Guid.Parse(reader["ID"].ToString()!);

            return garcom;
        }

        private Produto? ConverterParaProduto(SqlDataReader reader)
        {
            var produto = new Produto(
               Convert.ToString(reader["NOME"])!,
               Convert.ToDecimal(reader["VALOR"])
            );

            produto.Id = Guid.Parse(reader["ID"].ToString()!);

            return produto;
        }

        private object ConverterParaConta(SqlDataReader reader)
        {
            var mesaId = Guid.Parse(reader["MESA_ID"].ToString()!);
            var garcomId = Guid.Parse(reader["GARCOM_ID"].ToString()!);

            var mesa = (Mesa)SelecionarMesaPorId(mesaId);
            var garcom = (Garcom)SelecionarGarcomPorId(garcomId);

            DateTime? fechamento = null;

            if (!reader["FECHAMENTO"].Equals(DBNull.Value))
                fechamento = Convert.ToDateTime(reader["FECHAMENTO"]);

            var conta = new Conta
            {
                Id = Guid.Parse(reader["ID"].ToString()!),
                Titular = Convert.ToString(reader["TITULAR"])!,
                Mesa = mesa,
                Garcom = garcom,
                Abertura = Convert.ToDateTime(reader["ABERTURA"]),
                Fechamento = fechamento,
                EstaAberta = Convert.ToBoolean(reader["ESTAABERTA"]),
            };

            CarregarPedidos(conta);

            return conta;
        }

        private void ConfigurarParametrosConta(Conta conta, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", conta.Id);
            comando.Parameters.AddWithValue("TITULAR", conta.Titular);
            comando.Parameters.AddWithValue("MESA_ID", conta.Mesa.Id);
            comando.Parameters.AddWithValue("GARCOM_ID", conta.Garcom.Id);
            comando.Parameters.AddWithValue("ABERTURA", conta.Abertura);
            comando.Parameters.AddWithValue("FECHAMENTO", conta.Fechamento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("ESTAABERTA", conta.EstaAberta);
        }

        private void ConfigurarParametrosPedido(SqlCommand comando, Pedido pedido)
        {
            comando.Parameters.AddWithValue("ID", pedido.Id);
            comando.Parameters.AddWithValue("PRODUTO_ID", pedido.Produto.Id);
            comando.Parameters.AddWithValue("QUANTIDADE", pedido.QuantidadeSolicitada);
        }
    }
}
