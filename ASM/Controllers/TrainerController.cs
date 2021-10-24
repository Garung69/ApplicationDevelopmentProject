using ASM.EF;
using ASM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static ASM.Models.UserInfor;

namespace ASM.Controllers
{
    [Authorize(Roles = SecurityRoles.Trainer)]
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
                @TempData["alert"] = "Change Profile successful";
                return RedirectToAction("Index", "Trainer");
            }
        }


        private void SetViewBag()
        {
            using (var bwCtx = new EF.CMSContext())
            {
                ViewBag.Publishers = bwCtx.Courses
                                  .Select(p => new SelectListItem
                                  {
                                      Text = p.Name,
                                      Value = p.Id.ToString()
                                  })
                                  .ToList();

                ViewBag.Formats = bwCtx.Courses.ToList(); //select *
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShowCourseAssign()
        {
            TempData["username"] = TempData["username"];
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByEmailAsync(TempData["username"].ToString() + "@gmail.com");

            var a = manager.Users.Include(x => x.listCourse).FirstOrDefault(b => b.Id == user.Id);

            if (a != null)
            {
                SetViewBag();
                return View(a);
            }
            else
            {
                return RedirectToAction("Index");
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

            CustomValidationTrainerPass(userInfor);

            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(TempData["username"].ToString() + "@gmail.com");
                var result = manager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, userInfor.PasswordHash);

                if (user != null)
                {
                    if (result == PasswordVerificationResult.Success)
                    {
                        String newPassword = userInfor.PassTemp;
                        String hashedNewPassword = manager.PasswordHasher.HashPassword(newPassword);
                        user.PasswordHash = hashedNewPassword;
                        await store.UpdateAsync(user);
                        @TempData["alert"] = "Change PassWord successful";
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordHash", "Old Password incorrect!");
                        TempData["username"] = TempData["username"];
                        return View();
                    }

                }
                TempData["username"] = TempData["username"];
                return RedirectToAction("Index", "Trainer");
            }
        }
        public void CustomValidationTrainerPass(UserInfor staff)
        {
            if (string.IsNullOrEmpty(staff.PasswordHash))
            {
                ModelState.AddModelError("PasswordHash", "Please input old Password");
            }
            if (string.IsNullOrEmpty(staff.PassTemp))
            {
                ModelState.AddModelError("PassTemp", "Please input new Password");
            }
            if (!string.IsNullOrEmpty(staff.PassTemp) && !string.IsNullOrEmpty(staff.PassTemp) && (staff.PasswordHash.Length <= 7) && (staff.PassTemp.Length <= 7))
            {
                ModelState.AddModelError("PassTemp", "Password must longer than 7 charactors");
            }
            if (string.IsNullOrEmpty(staff.PassTempConfirm))
            {
                ModelState.AddModelError("PassTempConfirm", "Please input Confirm Password");
            }
            if (!string.IsNullOrEmpty(staff.PassTempConfirm) && !string.IsNullOrEmpty(staff.PassTemp) && (staff.PassTemp != staff.PassTempConfirm))
            {
                ModelState.AddModelError("PassTempConfirm", "New password and Confirm password not match");
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