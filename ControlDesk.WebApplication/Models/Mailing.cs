using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlDesk.WebApplication.Models
{
    public class Mailing:Dominio.Mailing
    {
        public TimeSpan Idle { get; set; }

        public List<Mailing> Mailings(DateTime Data)
        {
            List<Dominio.Campanha> campanhas = new Dominio.Campanha().Campanhas();
            List<Dominio.GrupoIdle> idles = new Dominio.GrupoIdle().GrupoIdles();
            List<Dominio.Grupo> Grupos = new Dominio.Grupo().Grupos();
            List<Mailing> Mailings = new List<Mailing>();
            

            foreach(Dominio.Mailing dominioMailing in new Dominio.Mailing().Mailings(Data))
            {
                if (campanhas.Where(c => c.Descricao.Equals(dominioMailing.Campanha)).Count() == 0)
                    continue;

                Mailing mailing = new Mailing();
                mailing.Atendidas = dominioMailing.Atendidas;
                mailing.Campanha = dominioMailing.Campanha;
                mailing.CpfRestantes = dominioMailing.CpfRestantes;
                mailing.Data = dominioMailing.Data;
                mailing.DataAtualizacao = dominioMailing.DataAtualizacao;
                mailing.Desligadas = dominioMailing.Desligadas;
                mailing.Id = dominioMailing.Id;
                mailing.PercMailing = dominioMailing.PercMailing;
                mailing.TMA = dominioMailing.TMA;
                mailing.TME = dominioMailing.TME;
                mailing.TMO = dominioMailing.TMO;
                mailing.TNA = dominioMailing.TNA;

                Dominio.Grupo grupo = Grupos.Where(c=> c.Descricao.Equals(dominioMailing.Campanha)).FirstOrDefault();
                Dominio.GrupoIdle idle = idles.Where(c=>c.IdGrupo.Equals(grupo.IdTotalIpPausa)).FirstOrDefault();

                if (idle != null)
                    mailing.Idle = idle.Idle;
                
                Mailings.Add(mailing);

            }

            return Mailings;
        }
    }
}