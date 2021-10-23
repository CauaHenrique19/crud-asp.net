using Npgsql;

namespace WepApi1
{
    public class Conexao
    {
        NpgsqlConnection conec = new NpgsqlConnection();

        public Conexao()
        {
            conec.ConnectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=caua987311363;Database=loja;";
        }

        public NpgsqlConnection Conectar()
        {
            if (conec.State == System.Data.ConnectionState.Closed)
            {
                conec.Open();
            }

            return conec;
        }

        public void Desconectar()
        {
            if (conec.State == System.Data.ConnectionState.Open)
            {
                conec.Close();
            }
        }
    }
}
