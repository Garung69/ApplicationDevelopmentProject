using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASM.Controllers
{
    public class TrainerController : Controller
    {
        // GET: Trainer
        public ActionResult Index()
        {
            ViewBag.Message = TempData["acb"];
                return View();
        }

        public ActionResult EditTrainer()
        {
            return View();
        }

        public ActionResult ChangePass()
        {
            return View();
        }
    }
}