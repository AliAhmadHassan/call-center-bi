using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDesk.Dominio
{
    public class Campanha
    {
        public Campanha()
        {

        }
        public Campanha(int Id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSCampanhasById", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("Id", Id);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    Descricao = reader.GetString(reader.GetOrdinal("Descricao"));
                    IdTotalIp = reader.GetInt32(reader.GetOrdinal("IdTotalIp"));
                }
            }
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public int IdTotalIp { get; set; }

        public void Salvar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUCampanhas", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("Descricao", Descricao);
                cmd.Parameters.AddWithValue("IdTotalIp", IdTotalIp);

                if (this.Id == -1)
                {
                    cmd.CommandText = "SPICampanhas";
                    this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                    cmd.ExecuteNonQuery();
            }
        }

        public void Remover()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPDCampanhas", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Campanha> Campanhas()
        {
            List<Campanha> campanhas = new List<Campanha>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSCampanhas", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Campanha campanha = new Campanha();
                    campanha.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    campanha.Descricao = reader.GetString(reader.GetOrdinal("Descricao"));
                    campanha.IdTotalIp = reader.GetInt32(reader.GetOrdinal("IdTotalIp"));
                    campanhas.Add(campanha);
                }
            }

            return campanhas;
        }
    }
}
