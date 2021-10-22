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
        public async Task<ActionResult>  Index()
        {
            TempData["username"] = TempData["username"];
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByEmailAsync(TempData["username"].ToString()+"@gmail.com");
            return View(user);
        }




        [HttpGet]
        public ActionResult EditTrainer(string id)
        {
            using (var FAPCtx = new EF.CMSContext())
            {
                var student = FAPCtx.Users.FirstOrDefault(c => c.UserName == id);

                if (student != null)
                {
                    TempData["username"] = id;
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
                TempData["username"] = TempData["username"];
                return RedirectToAction("Index", "Trainer");
            }
        }





        //private void CustomValidationTrainer(UserInfor trainer)
        //{
        //    
        //}


        [HttpGet]
        public ActionResult ChangePass(string id)
        {
            TempData["username"] = id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePass(FormCollection fc, UserInfor userInfor)
        {

            CustomValidationTrainer(userInfor);

            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(TempData["username"].ToString()+"@gmail.com");

                if (user != null)
                {
                    if(userInfor.PasswordHash != null)
                    {
                        if(userInfor.PassTemp == userInfor.PasswordHash)
                        {
                            String newPassword = userInfor.PasswordHash; 
                            String hashedNewPassword = manager.PasswordHasher.HashPassword(newPassword);
                            await store.SetPasswordHashAsync(user, hashedNewPassword);
                            await store.UpdateAsync(user);
                        }
                        else
                        {
                            ModelState.AddModelError("PasswordHash", "Password and confirm Password incorrect!");
                            TempData["username"] = TempData["username"];
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordHash", "Please input password");
                        TempData["username"] = TempData["username"];
                        return View();
                    }        
                }
                TempData["username"] = TempData["username"];
                return RedirectToAction("Index", "Trainer");
            }
        }
        public void CustomValidationTrainer(UserInfor staff)
        {
            if (string.IsNullOrEmpty(staff.PassTemp))
            {
                ModelState.AddModelError("PassTemp", "Please input Password");
            }
            if (string.IsNullOrEmpty(staff.PasswordHash))
            {
                ModelState.AddModelError("PasswordHash", "Please confirm Password");
            }
            if (!string.IsNullOrEmpty(staff.PasswordHash) && (staff.PasswordHash.Length <= 7) && (staff.PassTemp.Length <= 7))
            {
                ModelState.AddModelError("PasswordHash", "Password must longer than 7 charactors");
            }
        }

        public ActionResult ShowCourse()
        {
            using (var classes = new EF.CMSContext())
            {
                var Course = classes.Courses.OrderBy(a => a.Id).ToList();
                return View(Course);
            }
        }
    }
}