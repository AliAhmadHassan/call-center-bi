using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDesk.Dominio
{

    public class Pausa
    {
        public Pausa()
        {
            this.Id = -1;
        }
        public Pausa(int Id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSPausasById", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("Id", Id);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    Operador = reader.GetString(reader.GetOrdinal("Operador"));
                    QtdPausas = reader.GetInt32(reader.GetOrdinal("QtdPausas"));
                    TotalPausas = reader.GetTimeSpan(reader.GetOrdinal("TotalPausas"));
                    Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                    DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"));
                    Login = reader.GetDateTime(reader.GetOrdinal("Login"));
                    Logoff = reader.GetDateTime(reader.GetOrdinal("Logoff"));
                    Atendidas = reader.GetInt32(reader.GetOrdinal("Atendidas"));
                    TMA = reader.GetTimeSpan(reader.GetOrdinal("TMA"));
                }
            }
        }

        [Display(Name="Cod")]
        public int Id { get; set; }

        [Display(Name = "Operador")]
        public string Operador { get; set; }

        [Display(Name = "Pausas")]
        public int QtdPausas { get; set; }

        [Display(Name = "Tempo")]
        public TimeSpan TotalPausas { get; set; }

        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Display(Name = "Ultima Atualização")]
        public DateTime DataAtualizacao { get; set; }

        [Display(Name = "Login")]
        public DateTime Login { get; set; }

        [Display(Name = "Logoff")]
        public DateTime Logoff { get; set; }

        [Display(Name = "Qtd. Atendidas")]
        public int Atendidas { get; set; }

        public TimeSpan TMA { get; set; }

        public void Salvar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUPausas", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("Operador", Operador);
                cmd.Parameters.AddWithValue("QtdPausas", QtdPausas);
                cmd.Parameters.AddWithValue("TotalPausas", TotalPausas);
                cmd.Parameters.AddWithValue("Data", DateTime.Today);


                if (Login.Year != 1)
                    cmd.Parameters.AddWithValue("Login", Login);
                else
                    cmd.Parameters.AddWithValue("Login", DBNull.Value);

                if (Logoff.Year != 1)
                    cmd.Parameters.AddWithValue("Logoff", Logoff);
                else
                    cmd.Parameters.AddWithValue("Logoff", DBNull.Value);

                cmd.Parameters.AddWithValue("Atendidas", Atendidas);
                cmd.Parameters.AddWithValue("TMA", TMA);

                if (this.Id == -1)
                {
                    cmd.CommandText = "SPIPausas";
                    this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                    cmd.ExecuteNonQuery();
            }
        }

        public void Remover()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPDPausas", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public List<Pausa> Pausas(DateTime Data)
        {
            List<Pausa> pausas = new List<Pausa>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSPausasByData", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("Data", Data);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pausa pausa = new Pausa();
                    pausa.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    pausa.Operador = reader.GetString(reader.GetOrdinal("Operador"));
                    pausa.QtdPausas = reader.GetInt32(reader.GetOrdinal("QtdPausas"));
                    pausa.TotalPausas = reader.GetTimeSpan(reader.GetOrdinal("TotalPausas"));
                    pausa.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                    pausa.DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"));

                    if (reader["Login"].ToString() != "")
                        pausa.Login = Convert.ToDateTime(reader["Login"]);

                    if (reader["Logoff"].ToString() != "")
                        pausa.Logoff = Convert.ToDateTime(reader["Logoff"]);

                    pausa.Atendidas = reader.GetInt32(reader.GetOrdinal("Atendidas"));

                    pausa.TMA = reader.GetTimeSpan(reader.GetOrdinal("TMA"));

                    pausas.Add(pausa);
                }
            }

            return pausas;
        }
    }
}
