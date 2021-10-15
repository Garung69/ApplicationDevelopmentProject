using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASM.Models;

namespace ASM.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(CourseCategoryEntity a) 
        {
            using (var abc = new EF.CMSContext())
            {
                abc.courseCategoryEntities.Add(a);
                abc.SaveChanges();
            }

            TempData["message"] = $"Successfully add class {a.Name} to system!";

            return RedirectToAction("LogIn","Login");
        }

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
        [HttpGet]
        public ActionResult DeleteCategory(int id, CourseCategoryEntity a)
        {
            using (var classes = new EF.CMSContext())
            {
                var Class = classes.courseCategoryEntities.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }

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

        [HttpGet]
        public ActionResult AddCourse()
        {
            ViewBag.Class = getList();
            return View();
        }
        
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

        public ActionResult ShowCourse()
        {
            using (var classes = new EF.CMSContext())
            {
                var Classroom = classes.Courses.OrderBy(a => a.Id).ToList();
                return View(Classroom);
            }
        }

        [HttpGet]
        public ActionResult DeleteCourse(int id, CourseCategoryEntity a)
        {
            using (var classes = new EF.CMSContext())
            {
                var Class = classes.courseCategoryEntities.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }

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
    }
}