using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlDesk.WebApplication.Controllers
{
    public class MailingController : Controller
    {
        //
        // GET: /Mailing/

        public ActionResult Index()
        {
            

            List<Models.Mailing> mailings = new Models.Mailing().Mailings(DateTime.Today);

            return PartialView(mailings);
        }
    }
}
