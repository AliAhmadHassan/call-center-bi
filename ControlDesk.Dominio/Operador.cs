using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDesk.Dominio
{

    public class Operador
    {
        public Operador()
        {
            this.Id = -1;
        }
        public Operador(int Id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSOperadoresById", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("Id", Id);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    Nome = reader.GetString(reader.GetOrdinal("Operador"));
                    Inicio = reader.GetTimeSpan(reader.GetOrdinal("Inicio"));

                    TotalPausa07 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa07"));
                    TotalPausa08 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa08"));
                    TotalPausa09 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa09"));
                    TotalPausa10 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa10"));
                    TotalPausa11 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa11"));
                    TotalPausa12 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa12"));
                    TotalPausa13 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa13"));
                    TotalPausa14 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa14"));
                    TotalPausa15 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa15"));
                    TotalPausa16 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa16"));
                    TotalPausa17 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa17"));
                    TotalPausa18 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa18"));
                    TotalPausa19 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa19"));
                    TotalPausa20 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa20"));
                    TotalPausa21 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa21"));
                    IdGrupo = reader.GetInt32(reader.GetOrdinal("IdGrupo"));
                    

                    Fim = reader.GetTimeSpan(reader.GetOrdinal("Fim"));
                }
            }
        }

        [Display(Name = "Cod.")]
        public int Id { get; set; }
        public string Nome { get; set; }
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fim { get; set; }

        [Display(Name = "07:00")]
        public TimeSpan TotalPausa07 { get; set; }
        [Display(Name = "08:00")]
        public TimeSpan TotalPausa08 { get; set; }
        [Display(Name = "09:00")]
        public TimeSpan TotalPausa09 { get; set; }
        [Display(Name = "10:00")]
        public TimeSpan TotalPausa10 { get; set; }
        [Display(Name = "11:00")]
        public TimeSpan TotalPausa11 { get; set; }
        [Display(Name = "12:00")]
        public TimeSpan TotalPausa12 { get; set; }
        [Display(Name = "13:00")]
        public TimeSpan TotalPausa13 { get; set; }
        [Display(Name = "14:00")]
        public TimeSpan TotalPausa14 { get; set; }
        [Display(Name = "15:00")]
        public TimeSpan TotalPausa15 { get; set; }
        [Display(Name = "16:00")]
        public TimeSpan TotalPausa16 { get; set; }
        [Display(Name = "17:00")]
        public TimeSpan TotalPausa17 { get; set; }
        [Display(Name = "18:00")]
        public TimeSpan TotalPausa18 { get; set; }
        [Display(Name = "19:00")]
        public TimeSpan TotalPausa19 { get; set; }
        [Display(Name = "20:00")]
        public TimeSpan TotalPausa20 { get; set; }
        [Display(Name = "21:00")]
        public TimeSpan TotalPausa21 { get; set; }
        [Display(Name = "Grupo")]
        public int IdGrupo { get; set; }

        public void Salvar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUOperadores", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("Operador", Nome);
                cmd.Parameters.AddWithValue("Inicio", Inicio);
                cmd.Parameters.AddWithValue("Fim", Fim);

                cmd.Parameters.AddWithValue("TotalPausa07", TotalPausa07);
                cmd.Parameters.AddWithValue("TotalPausa08", TotalPausa08);
                cmd.Parameters.AddWithValue("TotalPausa09", TotalPausa09);
                cmd.Parameters.AddWithValue("TotalPausa10", TotalPausa10);
                cmd.Parameters.AddWithValue("TotalPausa11", TotalPausa11);
                cmd.Parameters.AddWithValue("TotalPausa12", TotalPausa12);
                cmd.Parameters.AddWithValue("TotalPausa13", TotalPausa13);
                cmd.Parameters.AddWithValue("TotalPausa14", TotalPausa14);
                cmd.Parameters.AddWithValue("TotalPausa15", TotalPausa15);
                cmd.Parameters.AddWithValue("TotalPausa16", TotalPausa16);
                cmd.Parameters.AddWithValue("TotalPausa17", TotalPausa17);
                cmd.Parameters.AddWithValue("TotalPausa18", TotalPausa18);
                cmd.Parameters.AddWithValue("TotalPausa19", TotalPausa19);
                cmd.Parameters.AddWithValue("TotalPausa20", TotalPausa20);
                cmd.Parameters.AddWithValue("TotalPausa21", TotalPausa21);
                cmd.Parameters.AddWithValue("IdGrupo", IdGrupo);
                

                if (this.Id == -1)
                {
                    cmd.CommandText = "SPIOperadores";
                    this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                    cmd.ExecuteNonQuery();
            }
        }

        public void Remover()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPDOperadores", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Operador> Operadores()
        {
            List<Operador> operadores = new List<Operador>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSOperadores", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Operador operadore = new Operador();
                    operadore.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    operadore.Nome = reader.GetString(reader.GetOrdinal("Operador"));
                    operadore.Inicio = reader.GetTimeSpan(reader.GetOrdinal("Inicio"));
                    operadore.Fim = reader.GetTimeSpan(reader.GetOrdinal("Fim"));
                    operadore.TotalPausa07 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa07"));
                    operadore.TotalPausa08 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa08"));
                    operadore.TotalPausa09 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa09"));
                    operadore.TotalPausa10 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa10"));
                    operadore.TotalPausa11 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa11"));
                    operadore.TotalPausa12 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa12"));
                    operadore.TotalPausa13 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa13"));
                    operadore.TotalPausa14 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa14"));
                    operadore.TotalPausa15 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa15"));
                    operadore.TotalPausa16 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa16"));
                    operadore.TotalPausa17 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa17"));
                    operadore.TotalPausa18 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa18"));
                    operadore.TotalPausa19 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa19"));
                    operadore.TotalPausa20 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa20"));
                    operadore.TotalPausa21 = reader.GetTimeSpan(reader.GetOrdinal("TotalPausa21"));
                    operadore.IdGrupo = reader.GetInt32(reader.GetOrdinal("IdGrupo"));
                    operadores.Add(operadore);
                }
            }

            return operadores;
        }
    }
}
