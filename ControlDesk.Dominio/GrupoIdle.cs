using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDesk.Dominio
{
    public class GrupoIdle
    {
        public GrupoIdle()
        {

        }
        public GrupoIdle(int Id)
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSGrupoIdleByIdGrupo", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                cmd.Parameters.AddWithValue("IdGrupo", Id);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IdGrupo = reader.GetInt32(reader.GetOrdinal("IdGrupo"));
                    Idle = reader.GetTimeSpan(reader.GetOrdinal("Idle"));
                }
            }
        }

        public int IdGrupo { get; set; }
        public TimeSpan Idle { get; set; }

        public void Salvar()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPIGrupoIdle", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("IdGrupo", IdGrupo);
                cmd.Parameters.AddWithValue("Idle", Idle);
                if (Idle.TotalSeconds >= 0)
                    this.IdGrupo = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void Remover()
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPDGrupoIdle", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }

        public List<GrupoIdle> GrupoIdles()
        {
            List<GrupoIdle> GrupoIdles = new List<GrupoIdle>();

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TotalIpConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SPSGrupoIdle", conn))
            {
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    GrupoIdle GrupoIdle = new GrupoIdle();
                    GrupoIdle.IdGrupo = reader.GetInt32(reader.GetOrdinal("IdGrupo"));
                    GrupoIdle.Idle = reader.GetTimeSpan(reader.GetOrdinal("Idle"));
                    GrupoIdles.Add(GrupoIdle);
                }
            }

            return GrupoIdles;
        }
    }
}
