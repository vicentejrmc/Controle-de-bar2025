using controleDeBar.Dominio.ModuloConta;
using controleDeBar.Dominio.ModuloGarcom;
using controleDeBar.Dominio.ModuloMesa;
using controleDeBar.Dominio.ModuloProduto;
using ControleDeBar.Infraestrutura.SqlServer.Compatilhado;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace ControleDeBar.Infraestrutura.SqlServer.ModuloConta
{
    public class RepositorioContaEmSql : RepositorioBaseEmSql<Conta>, IRepositorioConta
    {
        public RepositorioContaEmSql(IDbConnection conexaoComBanco) : base(conexaoComBanco){}

        protected override string SqlInserir =>
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

        protected override string SqlEditar =>
            @"UPDATE TBCONTA
              SET
                [TITULAR] = @TITULAR,
                [MESA_ID] = @MESA_ID,
                [GARCOM_ID] = @GARCOM_ID,
                [ABERTURA] = @ABERTURA,
                [FECHAMENTO] = @FECHAMENTO,
                [ESTAABERTA] = @ESTAABERTA
              WHERE
                [ID] = @ID";

        protected override string SqlExcluir =>
            @"DELETE FROM TBCONTA
              WHERE [ID] = @ID";

        protected override string SqlSelecionarPorId =>
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

        protected override string SqlSelecionarTodos =>
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

        protected override void ConfigurarParametrosRegistro(Conta registro, IDbCommand comando)
        {
            throw new NotImplementedException();
        }

        protected override Conta ConverterParaRegistro(IDataReader leitor)
        {
            throw new NotImplementedException();
        }
    }
}
