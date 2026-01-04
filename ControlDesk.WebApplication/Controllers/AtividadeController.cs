using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDesk.WebApplication.Controllers
{
    public class AtividadeController : Controller
    {
        //
        // GET: /Atividade/

        public ActionResult Index()
        {
            List<Dominio.Atividade> dominioAtividades = new Dominio.Atividade().Atividades(DateTime.Today);
            List<Dominio.Pausa> pausas = new Dominio.Pausa().Pausas(DateTime.Today);

            List<Models.Atividade> atividades = new List<Models.Atividade>();

            foreach(Dominio.Atividade dominioAtividade in dominioAtividades)
            {
                Dominio.Pausa dominioPausa = pausas.Where(c => c.Operador.Replace("...", "").Contains(dominioAtividade.Operador.Replace("...", ""))).FirstOrDefault();

                Models.Atividade atividade = new Models.Atividade();

                atividade.Campanha = dominioAtividade.Campanha;
                atividade.Data = dominioAtividade.Data;
                atividade.DataAtualizacaoAtividade = dominioAtividade.DataAtualizacao;
                atividade.Operador = dominioAtividade.Operador;
                atividade.Status = dominioAtividade.Status;
                atividade.TempoStatus = dominioAtividade.TempoStatus;

                if (dominioPausa != null)
                {
                    atividade.DataAtualizacaoPausa = dominioPausa.DataAtualizacao;
                    atividade.QtdPausas = dominioPausa.QtdPausas;
                    atividade.TotalPausas = dominioPausa.TotalPausas;
                    atividade.Atendidas = dominioPausa.Atendidas;
                    atividade.TMA = dominioPausa.TMA;
                }

                atividades.Add(atividade);
            }

            return PartialView(atividades.OrderBy(c => c.Status).ToList());
        }
    }
}
