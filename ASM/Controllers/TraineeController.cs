using ASM.EF;
using ASM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASM.Controllers
{
    public class TraineeController : Controller
    {
        // GET: Trainee

        public ActionResult Index()
        {
            ViewBag.Message = TempData["xyz"];
            return View();
        }


        [HttpGet]
        public ActionResult EditTrainee(string id)
        {
            using (var FAPCtx = new EF.CMSContext())
            {
                var trainee = FAPCtx.Users.FirstOrDefault(c => c.Id == id);

                if (trainee != null)
                {
                    TempData["xyz"] = id;
                    return View(trainee);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
        }
        [HttpPost]
        public async Task<ActionResult> EditTrainee(string id, UserInfor trainee)
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

                var user = await manager.FindByEmailAsync(trainee.Email);

                if (user != null)
                {
                    user.UserName = trainee.Email.Split('@')[0];
                    user.Email = trainee.Email;
                    user.Name = trainee.Name;                  
                    user.Education = trainee.Education;
                    user.Age = trainee.Age;
                    user.Department = trainee.Department;
                    user.Location = trainee.Location;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("Index", "Trainee");
            }
        }
    }
}