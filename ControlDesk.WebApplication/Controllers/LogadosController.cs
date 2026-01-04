using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDesk.WebApplication.Controllers
{
    public class LogadosController : Controller
    {
        public ActionResult Index()
        {
            List<Models.Logados> logados = new List<Models.Logados>();
            List<Dominio.Atividade> atividades = new Dominio.Atividade().Atividades(DateTime.Today);
            List<Dominio.Operador> operadores = new Dominio.Operador().Operadores().Where(c => c.Inicio.TotalSeconds <= DateTime.Now.TimeOfDay.TotalSeconds && c.Fim.TotalSeconds >= DateTime.Now.TimeOfDay.TotalSeconds).ToList();
            List<Dominio.Grupo> grupos = new Dominio.Grupo().Grupos();


            foreach (Dominio.Operador operador in operadores)
            {
                Dominio.Grupo grupo = grupos.Where(c => c.Id.Equals(operador.IdGrupo)).FirstOrDefault();


                Models.Logados logado = logados.Where(c => c.GrupoAtendimento.Trim().ToUpper().Equals(grupo.Descricao.Trim().ToUpper())).FirstOrDefault();

                if (logado == null)
                {
                    logado = new Models.Logados();

                    logados.Add(logado);
                }

                logado.GrupoAtendimento = grupo.Descricao.Trim().ToUpper();
                logado.Capacity++;

                string NomeOperador = operador.Nome.Trim().ToUpper();

                Dominio.Atividade atividade = atividades.Where(c => NomeOperador.Trim().ToUpper().Replace("...", "").Contains(c.Operador.Replace("...", "").Trim().ToUpper())).FirstOrDefault();
                if (atividade != null)
                {
                    if (atividade.Status != "Linha Presa")
                        logado.PessoasLogadas++;
                }
                else
                {

                }
            }

            return PartialView(logados);
        }
    }
}
