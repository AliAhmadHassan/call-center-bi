using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDesk.Dominio
{

    public class Mailing
    {
        public Mailing()
        {
            this.Id = -1;
        }
        public Mailing(int Id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSMailingById", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("Id", Id);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    Campanha = reader.GetString(reader.GetOrdinal("Operador"));
                    PercMailing = reader.GetInt32(reader.GetOrdinal("TempoStatus"));
                    CpfRestantes = reader.GetInt32(reader.GetOrdinal("Campanha"));
                    Atendidas = reader.GetInt32(reader.GetOrdinal("QtdPausas"));
                    Desligadas = reader.GetInt32(reader.GetOrdinal("TotalPausas"));
                    TNA = reader.GetInt32(reader.GetOrdinal("TotalPausas"));
                    TMA = reader.GetTimeSpan(reader.GetOrdinal("TotalPausas"));
                    TME = reader.GetTimeSpan(reader.GetOrdinal("TotalPausas"));
                    TMO = reader.GetTimeSpan(reader.GetOrdinal("TotalPausas"));
                    Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                    DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"));
                }
            }
        }

        [Display(Name = "Cod")]
        public int Id { get; set; }

        [Display(Name = "Campanha")]
        public string Campanha { get; set; }

        [Display(Name = "% Mailing")]
        public int PercMailing { get; set; }

        [Display(Name = "CPF Restantes")]
        public int CpfRestantes { get; set; }

        [Display(Name = "Atendidas")]
        public int Atendidas { get; set; }

        [Display(Name = "Desligadas")]
        public int Desligadas { get; set; }

        [Display(Name = "TNA")]
        public int TNA { get; set; }

        [Display(Name = "TMA")]
        public TimeSpan TMA { get; set; }

        [Display(Name = "TME")]
        public TimeSpan TME { get; set; }

        [Display(Name = "TMO")]
        public TimeSpan TMO { get; set; }

        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Display(Name = "Data da Atualização")]
        public DateTime DataAtualizacao { get; set; }

        public void Salvar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUMailing", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("Campanha", Campanha);
                cmd.Parameters.AddWithValue("PercMailing", PercMailing);
                cmd.Parameters.AddWithValue("CpfRestantes", CpfRestantes);
                cmd.Parameters.AddWithValue("Atendidas", Atendidas);
                cmd.Parameters.AddWithValue("Desligadas", Desligadas);
                cmd.Parameters.AddWithValue("TNA", TNA);
                cmd.Parameters.AddWithValue("TMA", TMA);
                cmd.Parameters.AddWithValue("TME", TME);
                cmd.Parameters.AddWithValue("TMO", TMO);
                cmd.Parameters.AddWithValue("Data", Convert.ToDateTime("01/01/2000"));

                if (this.Id == -1)
                {
                    cmd.CommandText = "SPIMailing";
                    this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                    cmd.ExecuteNonQuery();
            }
        }

        public void Remover()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPDMailing", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUMailingAtualiza", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public virtual List<Mailing> Mailings(DateTime Data)
        {
            List<Mailing> mailings = new List<Mailing>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSMailingByData", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("Data", Data);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Mailing mailing = new Mailing();
                    mailing.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    mailing.Campanha = reader.GetString(reader.GetOrdinal("Campanha"));
                    mailing.PercMailing = reader.GetInt32(reader.GetOrdinal("PercMailing"));
                    mailing.CpfRestantes = reader.GetInt32(reader.GetOrdinal("CpfRestantes"));
                    mailing.Atendidas = reader.GetInt32(reader.GetOrdinal("Atendidas"));
                    mailing.Desligadas = reader.GetInt32(reader.GetOrdinal("Desligadas"));
                    mailing.TNA = reader.GetInt32(reader.GetOrdinal("TNA"));
                    mailing.TMA = reader.GetTimeSpan(reader.GetOrdinal("TMA"));
                    mailing.TME = reader.GetTimeSpan(reader.GetOrdinal("TME"));
                    mailing.TMO = reader.GetTimeSpan(reader.GetOrdinal("TMO"));
                    mailing.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                    mailing.DataAtualizacao = reader.GetDateTime(reader.GetOrdinal("DataAtualizacao"));
                    mailings.Add(mailing);
                }
            }

            return mailings;
        }
    }
}
