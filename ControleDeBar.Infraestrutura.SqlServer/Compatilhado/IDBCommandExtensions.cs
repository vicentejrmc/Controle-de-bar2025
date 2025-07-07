using System.Data;

namespace ControleDeBar.Infraestrutura.SqlServer.Compatilhado
{
    public static class IDBCommandExtensions
    {
        public static void AdicionarParametro(this IDbCommand comando, string nome, object valor)
        {
            var parametro = comando.CreateParameter();
            parametro.ParameterName = nome;
            parametro.Value = valor;

            comando.Parameters.Add(parametro);
        }
    }
}
