using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDesk.WebApplication.Controllers
{
    public class OperadorController : Controller
    {
        //
        // GET: /Operador/

        public ActionResult Index()
        {
            List<Models.Operador> operadores = new Models.Operador().Operadores();

            return View(operadores);
        }

        //
        // GET: /Operador/Details/5

        public ActionResult Details(int id)
        {
            Models.Operador operador = new Models.Operador().Operadores().Where(c => c.Id.Equals(id)).FirstOrDefault();
            return View(operador);
        }

        //
        // GET: /Operador/Create

        public ActionResult Create()
        {
            Models.Operador operador = new Models.Operador();
            return View(operador);
        }

        //
        // POST: /Operador/Create

        [HttpPost]
        public ActionResult Create(Dominio.Operador operador)
        {
            try
            {
                operador.Salvar();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Operador/Edit/5

        public ActionResult Edit(int id)
        {
            Models.Operador operador = new Models.Operador().Operadores().Where(c => c.Id.Equals(id)).FirstOrDefault();
            return View(operador);
        }

        //
        // POST: /Operador/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Dominio.Operador operador)
        {
            try
            {
                operador.Salvar();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Operador/Delete/5

        public ActionResult Delete(int id)
        {
            Models.Operador operador = new Models.Operador().Operadores().Where(c => c.Id.Equals(id)).FirstOrDefault();
            return View(operador);
        }

        //
        // POST: /Operador/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, Dominio.Operador operador)
        {
            try
            {
                operador.Remover();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
