using ASM.EF;
using ASM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ASM.Models.UserInfor;

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




        [HttpGet]
        public ActionResult EditTrainer(string id)
        {
            using (var FAPCtx = new EF.CMSContext())
            {
                var student = FAPCtx.Users.FirstOrDefault(c => c.Id == id);

                if (student != null)
                {
                    TempData["acb"] = id;
                    return View(student);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
        }
        [HttpPost]
        public async Task<ActionResult> EditTrainer(string id, UserInfor trainer)
        {

            //CustomValidationTrainer(trainer);

            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(trainer.Email);

                if (user != null)
                {
                    user.UserName = trainer.Email.Split('@')[0];
                    user.Email = trainer.Email;                  
                    user.Name = trainer.Name;
                    user.Type = trainer.Type;
                    user.Education = trainer.Education;
                    user.WorkingPlace = trainer.WorkingPlace;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("Index", "Trainer");
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        
        
      

        //private void CustomValidationTrainer(UserInfor trainer)
        //{
        //    throw new NotImplementedException();
        //}



        public ActionResult ChangePass()
        {
            return View();
        }
    }
}