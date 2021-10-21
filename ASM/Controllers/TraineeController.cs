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

        public async Task<ActionResult> Index()
        {
            TempData["username"] = TempData["username"];
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByEmailAsync(TempData["username"].ToString() + "@gmail.com");
            return View(user);
        }


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

                var user = await manager.FindByEmailAsync(TempData["username"].ToString() + "@gmail.com");

                if (user != null)
                {
                    if (userInfor.PasswordHash != null)
                    {
                        if (userInfor.PassTemp == userInfor.PasswordHash)
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
                return RedirectToAction("Index", "Trainee");
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
            if (!string.IsNullOrEmpty(staff.PasswordHash) && !string.IsNullOrEmpty(staff.PassTemp) && (staff.PasswordHash.Length <= 7) && (staff.PassTemp.Length <= 7))
            {
                ModelState.AddModelError("PasswordHash", "Password must longer than 7 charactors");
            }
        }

  /*      public async Task<ActionResult> ShowCourse()
        {
            TempData["username"] = TempData["username"];
            using (CMSContext context = new CMSContext())
            {
                var usersWithCourse = (from cousre in context.Courses
                                      select new
                                      {
                                          Name = cousre.Id,
                                          Username = cousre.Name,
                                          Email = cousre.Description,
                                          //More Propety

                                          TraineeAssigned = (from course in cousre.listTrainer
                                                          join trainees in context.Users on course.Id
                                                          equals trainees.Id
                                                          select trainees.Name).ToList()
                                      }).ToList().Where(p => p.Username == TempData["username"].ToString()).Select(p => new UserInCourse()

                                      {
                                          listCourseAssign = p.CourseAssign,
                                          Username = p.Username,
                                          Email = p.Email
                                      });
                return View(usersWithCourse);
            }
        }*/
    }
}