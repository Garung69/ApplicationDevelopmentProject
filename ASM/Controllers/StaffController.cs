using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ASM.EF;
using ASM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASM.Controllers
{
    public class StaffController : Controller
    {
        [Authorize(Roles = SecurityRoles.Staff)]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpPost]
        public ActionResult CreateCategory(CourseCategoryEntity a) 
        {
            using (var abc = new EF.CMSContext())
            {
                abc.courseCategoryEntities.Add(a);
                abc.SaveChanges();
            }

            TempData["message"] = $"Successfully add class {a.Name} to system!";

            return RedirectToAction("ShowCategory");
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        public ActionResult ShowCategory()
        {
            using (var classes = new EF.CMSContext())
            {
                var Classroom = classes.courseCategoryEntities.OrderBy(a => a.Id).ToList();
                return View(Classroom);
            }
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            using (var classes = new EF.CMSContext())
            {
                var Class = classes.courseCategoryEntities.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpPost]
        public ActionResult EditCategory(int id, CourseCategoryEntity a)
        {
            using (var abc = new EF.CMSContext())
            {
                abc.Entry<CourseCategoryEntity>(a).State = System.Data.Entity.EntityState.Modified;

                abc.SaveChanges();
            }

            return RedirectToAction("ShowCategory");
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpGet]
        public ActionResult DeleteCategory(int id, CourseCategoryEntity a)
        {
            using (var classes = new EF.CMSContext())
            {
                var Class = classes.courseCategoryEntities.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            using (var abc = new EF.CMSContext())
            {
                var xxx = abc.courseCategoryEntities.FirstOrDefault(b => b.Id == id);
                if (xxx != null)
                {
                    abc.courseCategoryEntities.Remove(xxx);
                    abc.SaveChanges();
                }
                TempData["message"] = $"Successfully delete book with Id: {xxx.Id}";
                return RedirectToAction("ShowCategory");
            }
        }
        //-------------------------------------------------------------------------------------------------//
        
        private List<SelectListItem> getList()
        {
            using (var abc = new EF.CMSContext())
            {
                var stx = abc.courseCategoryEntities.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();
                return stx;
            }
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpGet]
        public ActionResult AddCourse()
        {
            ViewBag.Class = getList();
            return View();
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpPost]
        public ActionResult AddCourse(CourseEntity a)
        {
            using (var abc = new EF.CMSContext())
            {
                abc.Courses.Add(a);
                abc.SaveChanges();
            }

            TempData["message"] = $"Successfully add class {a.Name} to system!";

            return RedirectToAction("ShowCourse");
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpGet]
        public ActionResult EditCourse(int id,CourseEntity a)
        {
            ViewBag.Class = getList();
            using (var classes = new EF.CMSContext())
            {
                var Class = classes.Courses.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpPost]
        public ActionResult EditCourse(CourseEntity a)
        {
            using (var abc = new EF.CMSContext())
            {
                abc.Entry<CourseEntity>(a).State = System.Data.Entity.EntityState.Modified;

                abc.SaveChanges();
            }

            return RedirectToAction("ShowCategory");
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        public ActionResult ShowCourse()
        {
            using (var classes = new EF.CMSContext())
            {
                var Classroom = classes.Courses.OrderBy(a => a.Id).ToList();
                return View(Classroom);
            }
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpGet]
        public ActionResult DeleteCourse(int id, CourseCategoryEntity a)
        {
            using (var classes = new EF.CMSContext())
            {
                var Class = classes.courseCategoryEntities.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }
        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {
            using (var abc = new EF.CMSContext())
            {
                var xxx = abc.Courses.FirstOrDefault(b => b.Id == id);
                if (xxx != null)
                {
                    abc.Courses.Remove(xxx);
                    abc.SaveChanges();
                }
                TempData["message"] = $"Successfully delete book with Id: {xxx.Id}";
                return RedirectToAction("ShowCategory");
            }
        }

        public ActionResult ShowTrainer()
        {
            using (var ASMCtx = new EF.CMSContext())
            {
                var Staff = ASMCtx.Users.Where(s => s.Role == "trainer").ToList();
                return View(Staff);
            }
        }

        public ActionResult EditTrainer(string id)
        {
            using (var FAPCtx = new EF.CMSContext())
            {
                var student = FAPCtx.Users.FirstOrDefault(c => c.Id == id);

                if (student != null)
                {
                    TempData["StaffId"] = id;
                    return View(student);
                }
                else
                {
                    return RedirectToAction("AMTrainer");
                }

            }
        }

        [Authorize(Roles = SecurityRoles.Staff)]
        [HttpPost]
        public async Task<ActionResult> EditTrainer(string id, UserInfor trainer)
        {

            CustomValidationfTrainer(trainer);

            if (!ModelState.IsValid)
            {
                return View(trainer);
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
                    user.PhoneNumber = trainer.PhoneNumber;
                    user.WorkingPlace = trainer.WorkingPlace;
                    user.Type = trainer.Type;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("ShowTrainer");
            }
        }
        private void CustomValidationfTrainer(UserInfor staff)
        {
            if (string.IsNullOrEmpty(staff.Email))
            {
                ModelState.AddModelError("Email", "Please input Email");
            }
            if (string.IsNullOrEmpty(staff.Name))
            {
                ModelState.AddModelError("Email", "Please input Name");
            }
            if (!string.IsNullOrEmpty(staff.Email) && (staff.Email.Split('@')[1] != "gmail.com"))
            {
                ModelState.AddModelError("Email", "Please a valid Email (abc@gmail.com)");
            }
            if (!string.IsNullOrEmpty(staff.Email) && (staff.Email.Length >= 21))
            {
                ModelState.AddModelError("Email", "This email is not valid!");
            }
        }

    }
}