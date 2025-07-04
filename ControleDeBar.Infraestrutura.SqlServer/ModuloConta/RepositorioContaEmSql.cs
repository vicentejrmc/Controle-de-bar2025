using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using controleDeBar.Dominio.ModuloProduto;
using Microsoft.Data.SqlClient;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloConta;

public class RepositorioContaEmSql : IRepositorioConta
{
    private readonly string connectionString =
        "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=controleDeBarDb;Integrated Security=True";

    public void CadastrarConta(Conta novaConta)
    {
        var sqlInserir =
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
        SqlCommand comandoInserir = new SqlCommand(sqlInserir, coneccaoComBanco);

        ConfigurarParametrosConta(novaConta, comandoInserir);

        coneccaoComBanco.Open();
        comandoInserir.ExecuteNonQuery();

        coneccaoComBanco.Close();

    }

    private void ConfigurarParametrosConta(Conta novaConta, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", novaConta.Id);
        comando.Parameters.AddWithValue("@TITULAR", novaConta.Titular);
        comando.Parameters.AddWithValue("@MESA_ID", novaConta.Mesa.Id);
        comando.Parameters.AddWithValue("@GARCOM_ID", novaConta.Garcom.Id);
        comando.Parameters.AddWithValue("@ABERTURA", novaConta.Abertura);
        comando.Parameters.AddWithValue("@FECHAMENTO", novaConta.Fechamento == DateTime.MinValue ? (object)DBNull.Value : novaConta.Fechamento);
        comando.Parameters.AddWithValue("@ESTAABERTA", novaConta.EstaAberta);
    }

    public List<Conta> SelecionarContas()
    {
        var sqlSelecionarTodos =
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
                    [TBMESA] AS MS
                ON
                    CT.[MESA_ID] = MS.[ID]
                INNER JOIN
                    [TBGARCOM] AS GC
                ON
                    CT.[GARCOM_ID] = GC.[ID]";

        var contas = new List<Conta>();

        SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
        coneccaoComBanco.Open();
        SqlCommand cmd = new SqlCommand(sqlSelecionarTodos, coneccaoComBanco);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var mesa = new Mesa(
                Convert.ToInt32(reader["MESA_NUMERO"]),
                Convert.ToInt32(reader["MESA_CAPACIDADE"])
            )
            {
                Id = Guid.Parse(reader["MESA_ID"].ToString()!),
                EstaOcupada = Convert.ToBoolean(reader["MESA_OCUPADA"])
            };

            var garcom = new Garcom(
                Convert.ToString(reader["GARCOM_NOME"])!,
                Convert.ToString(reader["GARCOM_CPF"])!
            )
            {
                Id = Guid.Parse(reader["GARCOM_ID"].ToString()!)
            };

            var conta = new Conta
            {
                Id = Guid.Parse(reader["CONTA_ID"].ToString()!),
                Titular = Convert.ToString(reader["TITULAR"])!,
                Abertura = Convert.ToDateTime(reader["ABERTURA"]),
                Fechamento = reader["FECHAMENTO"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FECHAMENTO"]),
                EstaAberta = Convert.ToBoolean(reader["ESTAABERTA"]),
                Mesa = mesa,
                Garcom = garcom,
                Pedidos = SelecionarPedidosPorContaId(Guid.Parse(reader["CONTA_ID"].ToString()!))
            };

            contas.Add(conta);
        }

        return contas;
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

    private Conta ConverterParaConta(SqlDataReader reader)
    {
        var conta = new Conta
        {
            Id = Guid.Parse(reader["ID"].ToString()!),
            Titular = Convert.ToString(reader["TITULAR"])!,
            Abertura = Convert.ToDateTime(reader["ABERTURA"]),
            Fechamento = reader["FECHAMENTO"] == DBNull.Value ? DateTime.MinValue :
                Convert.ToDateTime(reader["FECHAMENTO"]),
            EstaAberta = Convert.ToBoolean(reader["ESTAABERTA"])
        };

        var mesaId = Guid.Parse(reader["MESA_ID"].ToString()!);
        conta.Mesa = SelecionarMesaPorId(mesaId);

        var garcomId = Guid.Parse(reader["GARCOM_ID"].ToString()!);
        conta.Garcom = SelecionarGarcomPorId(garcomId);

        conta.Pedidos = SelecionarPedidosPorContaId(conta.Id);

        return conta;
    }

    private List<Pedido> SelecionarPedidosPorContaId(Guid pedidoId)
    {
        var pedidos = new List<Pedido>();

        var sqlSelecionarPorId =
            @"SELECT
                [ID],
                [PRODUTO_ID],
                [QUANTIDADESOLICITADA]
            FROM
                [TBPEDIDO]
            WHERE
                [CONTA_ID] = @CONTA_ID";

        SqlConnection coneccaoComBanco = new SqlConnection(connectionString);
        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, coneccaoComBanco);


        comandoSelecao.Parameters.AddWithValue("CONTA_ID", pedidoId);

        coneccaoComBanco.Open();
        SqlDataReader reader = comandoSelecao.ExecuteReader();

        while (reader.Read())
        {
            var produtoId = Guid.Parse(reader["PRODUTO_ID"].ToString()!);
            var produto = SelecionarProdutoPorId(produtoId);

            var pedido = new Pedido
            {
                Id = Guid.Parse(reader["ID"].ToString()!),
                Produto = produto,
                QuantidadeSolicitada = Convert.ToInt32(reader["QUANTIDADESOLICITADA"])
            };

            pedidos.Add(pedido);
        }

        return pedidos;
    }

    private Produto SelecionarProdutoPorId(Guid produtoId)
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

    private Garcom SelecionarGarcomPorId(Guid garcomId)
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

    private Mesa SelecionarMesaPorId(Guid mesaId)
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
}

