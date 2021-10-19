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
            ViewBag.Message = TempData["acb"];
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByEmailAsync(ViewBag.Message.ToString() + "@gmail.com");
            return View(user);
        }


        [HttpGet]
        public ActionResult ChangePass(string id)
        {
            TempData["user"] = id;
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

                var user = await manager.FindByEmailAsync(TempData["user"].ToString() + "@gmail.com");

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
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordHash", "Please input password");
                        return View();
                    }
                }
                TempData["acb"] = TempData["user"];
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

       /* public async Task<ActionResult> ShowCourse()
        {
            using (CMSContext context = new CMSContext())
            {
                var usersWithRoles = (from user in context.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.UserName,
                                          Email = user.Email,
                                          //More Propety

                                          CourseAssign = (from course in user.listCourse
                                                       join courses in context.Courses on course.Id
                                                       equals courses.Id
                                                       select courses.Name).ToList()
                                      }).ToList().Where(p => string.Join(",", p.UserId) == TempData["user"]).Select(p => new UserInRole()

                                      {
                                          UserId = p.UserId,
                                          Username = p.Username,
                                          Email = p.Email
                                       
                                      });
                return View(usersWithRoles);
            }
        }*/
    }
}