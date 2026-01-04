using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDesk.Dominio
{
    public class Grupo
    {
        public Grupo()
        {

        }
        public Grupo(int Id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSGruposById", conn))
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
                    IdTotalIpMailing = reader.GetInt32(reader.GetOrdinal("IdTotalIpMailing"));
                    IdTotalIpPausa = reader.GetInt32(reader.GetOrdinal("IdTotalIpPausa"));
                }
            }
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public int IdTotalIpMailing { get; set; }
        public int IdTotalIpPausa { get; set; }

        public void Salvar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPUGrupos", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("Descricao", Descricao);
                cmd.Parameters.AddWithValue("IdTotalIpMailing", IdTotalIpMailing);
                cmd.Parameters.AddWithValue("IdTotalIpPausa", IdTotalIpPausa);


                if (this.Id == -1)
                {
                    cmd.CommandText = "SPIGrupos";
                    this.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                    cmd.ExecuteNonQuery();
            }
        }

        public void Remover()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPDGrupos", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("Id", Id);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Grupo> Grupos()
        {
            List<Grupo> grupos = new List<Grupo>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSGrupos", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Grupo grupo = new Grupo();
                    grupo.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    grupo.Descricao = reader.GetString(reader.GetOrdinal("Descricao"));
                    grupo.IdTotalIpMailing = reader.GetInt32(reader.GetOrdinal("IdTotalIpMailing"));
                    grupo.IdTotalIpPausa = reader.GetInt32(reader.GetOrdinal("IdTotalIpPausa"));
                    grupos.Add(grupo);
                }
            }

            return grupos;
        }
    }
}
