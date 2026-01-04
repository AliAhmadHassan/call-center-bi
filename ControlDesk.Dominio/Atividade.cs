using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDesk.Dominio
{

    public class Atividade
    {
        public Atividade()
        {
            this.Id = -1;
        }
        public Atividade(int Id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSAtividadesById", conn))
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
                    Status = reader.GetString(reader.GetOrdinal("Status"));
                    TempoStatus = reader.GetTimeSpan(reader.GetOrdinal("TempoStatus"));
                    Campanha = reader.GetString(reader.GetOrdinal("Campanha"));
                    Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                    DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"));
                }
            }
        }

        [Display(Name="Cod")]
        public int Id { get; set; }

        [Display(Name = "Operador")]
        public string Operador { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Tempo")]
        public TimeSpan TempoStatus { get; set; }

        [Display(Name = "Campanha")]
        public string Campanha { get; set; }

        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Display(Name = "Ultima Atualização")]
        public DateTime DataAtualizacao { get; set; }

        public void Salvar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUAtividades", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("Operador", Operador);
                cmd.Parameters.AddWithValue("Status", Status);
                if (TempoStatus.TotalSeconds > 0)
                    cmd.Parameters.AddWithValue("TempoStatus", TempoStatus);
                else
                    cmd.Parameters.AddWithValue("TempoStatus", DBNull.Value);
                cmd.Parameters.AddWithValue("Campanha", Campanha);
                cmd.Parameters.AddWithValue("Data", Convert.ToDateTime("01/01/2000"));

                if (this.Id == -1)
                {
                    cmd.CommandText = "SPIAtividades";
                    this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                    cmd.ExecuteNonQuery();
            }
        }

        public void Remover()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPDAtividades", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualiza()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUAtividadesAtualiza", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }
        public List<Atividade> Atividades(DateTime Data)
        {
            List<Atividade> atividades = new List<Atividade>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSAtividadesByData", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("Data", Data);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Atividade atividade = new Atividade();
                    atividade.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    atividade.Operador = reader.GetString(reader.GetOrdinal("Operador"));
                    atividade.Status = reader.GetString(reader.GetOrdinal("Status"));
                    if (reader["TempoStatus"].ToString() != "")
                        atividade.TempoStatus = reader.GetTimeSpan(reader.GetOrdinal("TempoStatus"));
                    atividade.Campanha = reader.GetString(reader.GetOrdinal("Campanha"));
                    atividade.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                    atividade.DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"));
                    atividades.Add(atividade);
                }
            }

            return atividades;
        }
    }
}
