using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using ASM.EF;
using ASM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ASM.Controllers
{
    [Authorize(Roles = SecurityRoles.Staff)]
    public class StaffController : Controller
    {
        [Authorize(Roles = SecurityRoles.Staff)]
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

            CustomValidationfCAtegory(a);
            if (!ModelState.IsValid)
            {

                return View(a);
            }
            else
            {
                using (var abc = new EF.CMSContext())
                {
                    abc.courseCategoryEntities.Add(a);
                    abc.SaveChanges();
                }
                @TempData["alert"] = "You have successful add new Category";
            }


            return RedirectToAction("SearchCategory");
        }

        private void CustomValidationfCAtegory(CourseCategoryEntity category)
        {
            if (string.IsNullOrEmpty(category.Description))
            {
                ModelState.AddModelError("Description", "Please input Description");
            }
        }

        CMSContext db = new CMSContext();
        public ActionResult SearchCategory(string search)
        {
            return View(db.courseCategoryEntities.Where(x => x.Name.Contains(search) || search == null).ToList());
        }

        public ActionResult SearchCourse(string search)
        {
            return View(db.Courses.Where(y => y.Name.Contains(search) || search == null).ToList());
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
            CustomValidationfCAtegory(a);
            if (!ModelState.IsValid)
            {

                return View(a);
            }
            else
            {

                using (var abc = new EF.CMSContext())
                {
                    abc.Entry<CourseCategoryEntity>(a).State = System.Data.Entity.EntityState.Modified;

                    abc.SaveChanges();
                }
                @TempData["alert"] = "You have successful update a Category";
            }
            return RedirectToAction("SearchCategory");
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
                @TempData["alert"] = "You have successful delete a Category";
                return RedirectToAction("SearchCategory");
            }
        }
        //-------------------------------------------------------------------------------------------------//
        public void CustomValidationfCourses(CourseEntity course)
        {

            if (string.IsNullOrEmpty(course.Description))
            {
                ModelState.AddModelError("Description", "Please input Description");
            }
        }
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
            CustomValidationfCourses(a);
            if (!ModelState.IsValid)
            {
                ViewBag.Class = getList();
                return View(a);
            }
            else
            {
                using (var abc = new EF.CMSContext())
                {
                    abc.Courses.Add(a);
                    abc.SaveChanges();
                }

                @TempData["alert"] = "You have successful add new Course";
            }


            return RedirectToAction("SearchCourse");
        }

        [HttpGet]
        public ActionResult EditCourse(int id)
        {

            using (var classes = new EF.CMSContext())
            {
                ViewBag.Class = getList();
                var Class = classes.Courses.FirstOrDefault(c => c.Id == id);
                return View(Class);
            }
        }

        [HttpPost]
        public ActionResult EditCourse(CourseEntity a)
        {
            CustomValidationfCourses(a);

            if (!ModelState.IsValid)
            {
                ViewBag.Class = getList();
                return View(a);
            }
            else
            {
                using (var abc = new EF.CMSContext())
                {
                    abc.Entry<CourseEntity>(a).State = System.Data.Entity.EntityState.Modified;

                    abc.SaveChanges();
                }
            }
            @TempData["alert"] = "You have successful update new Course";
            return RedirectToAction("SearchCourse");
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
        public ActionResult DeleteCourse(int id, CourseEntity a)
        {
            using (var classes = new EF.CMSContext())
            {
                var Class = classes.Courses.FirstOrDefault(c => c.Id == id);
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
                @TempData["alert"] = "You have successful delete a Course";
                return RedirectToAction("SearchCourse");
            }
        }





        public ActionResult ShowTrainer()
        {
            using (CMSContext context = new CMSContext())
            {
                var usersWithRoles = (from user in context.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Username = user.UserName,
                                          Email = user.Email,
                                          WorkingPlace = user.WorkingPlace,
                                          Type = user.Type,
                                          PhoneNumber = user.PhoneNumber,
                                          Name = user.Name,
                                          //More Propety

                                          RoleNames = (from userRole in user.Roles
                                                       join role in context.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name).ToList()
                                      }).ToList().Where(p => string.Join(",", p.RoleNames) == "trainer").Select(p => new UserInRole()

                                      {
                                          UserId = p.UserId,
                                          Username = p.Username,
                                          Name = p.Name,
                                          Email = p.Email,
                                          Role = string.Join(",", p.RoleNames),
                                          WorkingPlace = p.WorkingPlace,
                                          Type = p.Type,
                                          Phone = p.PhoneNumber
                                      });
                return View(usersWithRoles);
            }
        }

        [HttpGet]
        public ActionResult EditTrainer(string id)
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);


            var a = manager.Users.Include(x => x.listCourse).FirstOrDefault(b => b.Id == id);

            if (a != null) // if a book is found, show edit view
            {
                //ViewBag.Publishers = GetPublishersDropDown();
                SetViewBag();
                return View(a);
            }
            else
            {
                return RedirectToAction("ShowTrainer"); //redirect to action in the same controller
            }
        }


        [HttpPost]
        public async Task<ActionResult> EditTrainer(string id, FormCollection f, UserInfor a)
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);
            var user = await manager.FindByIdAsync(a.Id);
            if (!ModelState.IsValid)
            {
                TempData["abc"] = f["formatIds[]"];
                SetViewBag();
                return View(a);
            }

            if (user != null)
            {
                using (var FAPCtx = new EF.CMSContext())
                {
                    a.UserName = a.Email.Split('@')[0];
                    a.PasswordHash = user.PasswordHash;
                    a.SecurityStamp = user.SecurityStamp;
                    FAPCtx.Entry<UserInfor>(a).State = System.Data.Entity.EntityState.Modified;

                    FAPCtx.Entry<UserInfor>(a).Collection(x => x.listCourse).Load();
                    a.listCourse = Convert(FAPCtx, f["formatIds[]"]);

                    FAPCtx.SaveChanges();
                }
            }
            @TempData["alert"] = "You have successful add a Trainer";
            return RedirectToAction("ShowTrainer");
        }
        //================================================================================================//


        private List<CourseEntity> Convert(
             EF.CMSContext bwCtx,
             string formatIds)
        {
            if (formatIds != null)
            {
                var abc = formatIds.Split(',')
                                       .Select(id => Int32.Parse(id))
                                       .ToArray();
                return bwCtx.Courses.Where(f => abc.Contains(f.Id)).ToList();
            }
            else
            {
                return bwCtx.Courses.Where(c => c.Id == 0).ToList();
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
        public ActionResult CreateTrainee()
        {
            SetViewBag();
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> CreateTrainee(UserInfor a, FormCollection f)
        {

            CustomValidationfTrainee(a);

            if (!ModelState.IsValid)
            {
                SetViewBag();
                return View(a); // return lai Create.cshtml
                                    //di kem voi data ma user da go vao
            }
            else
            {
                if (f["classId[]"].IsEmpty())
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(a.Email);

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = a.Email.Split('@')[0],
                        Email = a.Email,
                        Name = a.Name,
                        Role = "trainee",
                        PasswordHash = "123qwe123",
                        Education = a.Education,
                        Toeic = a.Toeic,
                        Department = a.Department,
                        Location = a.Location,
                        Experience=a.Experience,
                        DoB = a.DoB,
                        Age = a.Age,
                        ProgrammingLanguage = a.ProgrammingLanguage
                    };
                    user.listCourse = Convert(context, f["formatIds[]"]);
                    await manager.CreateAsync(user, user.PasswordHash);
                    await CreateRole(a.Email, "trainee");
                }

            }
            }
            
            @TempData["alert"] = "You have successful add a Trainee";
            return RedirectToAction("ShowTrainee");

        }

        private void CustomValidationfTrainee(UserInfor a)
        {
            if (string.IsNullOrEmpty(a.Email))
            {
                ModelState.AddModelError("Email", "Please input Email");
            }
            if (string.IsNullOrEmpty(a.Name))
            {
                ModelState.AddModelError("Email", "Please input Name");
            }
           
            if (!string.IsNullOrEmpty(a.Email) && (a.Email.Length >= 21))
            {
                ModelState.AddModelError("Email", "This email is not valid!");
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
            if (!string.IsNullOrEmpty(staff.Email) && (staff.Email.Split('@')[0] == null) && (staff.Email.Split('@')[1] == null) && (staff.Email.Split('@')[1] != "gmail.com"))
            {
                ModelState.AddModelError("Email", "Please a valid Email (abc@gmail.com)");
            }
            if (!string.IsNullOrEmpty(staff.Email) && (staff.Email.Length >= 21))
            {
                ModelState.AddModelError("Email", "This email is not valid!");
            }
        }


        public async Task<ActionResult> CreateRole(string email, string role)
        {
            var context = new CMSContext();
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<UserInfor>(context);
            var userManager = new UserManager<UserInfor>(userStore);

            if (!await roleManager.RoleExistsAsync(SecurityRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Admin });
            }

            if (!await roleManager.RoleExistsAsync(SecurityRoles.Staff))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Staff });
            }
            if (!await roleManager.RoleExistsAsync(SecurityRoles.Trainee))
            {

                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Trainee });

            }
            if (!await roleManager.RoleExistsAsync(SecurityRoles.Trainer))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Trainer });

            }

            var User = await userManager.FindByEmailAsync(email);

            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Admin) && role == "admin")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Admin);
            }
            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Staff) && role == "staff")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Staff);
            }
            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Trainer) && role == "trainer")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Trainer);
            }
            if (!await userManager.IsInRoleAsync(User.Id, SecurityRoles.Trainee) && role == "trainee")
            {
                userManager.AddToRole(User.Id, SecurityRoles.Trainee);
            }
            return Content("done!");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            

            
            
            
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);


            var a = manager.Users.Include(x => x.listCourse).FirstOrDefault(b => b.Id == id);

            if (a != null) // if a book is found, show edit view
            {
                //ViewBag.Publishers = GetPublishersDropDown();
                SetViewBag();
                return View(a);
            }
            else // if no book is found, back to index
            {
                return RedirectToAction("Index"); //redirect to action in the same controller
            }
        }



       






        public ActionResult ShowTrainee(string option, string search)
        {
            using (CMSContext context = new CMSContext())
            {
                var usersWithRoles = (from user in context.Users
                                      select new
                                      {
                                          UserId = user.Id,
                                          Name = user.Name,
                                          Email = user.Email,
                                          WorkingPlace = user.WorkingPlace,
                                          Type = user.Type,
                                          PhoneNumber = user.PhoneNumber,
                                          Toeic = user.Toeic,
                                          language = user.ProgrammingLanguage,
                                          //More Propety

                                          RoleNames = (from userRole in user.Roles
                                                       join role in context.Roles on userRole.RoleId
                                                       equals role.Id
                                                       select role.Name).ToList()
                                      }).ToList().Where(p => string.Join(",", p.RoleNames) == "trainee").Select(p => new UserInRole()

                                      {
                                          UserId = p.UserId,
                                          Name = p.Name,
                                          Email = p.Email,
                                          Role = string.Join(",", p.RoleNames),
                                          WorkingPlace = p.WorkingPlace,
                                          Type = p.Type,
                                          Phone = p.PhoneNumber,
                                          Toeic = p.Toeic,
                                          Language = p.language
                                      });
                return View(usersWithRoles);
            }
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            using (var FAPCtx = new EF.CMSContext())
            {
                var staff = FAPCtx.Users.FirstOrDefault(c => c.Id == id);

                if (staff != null)
                {
                    TempData["StaffId"] = id;
                    TempData["StaffUN"] = staff.UserName;
                    return View(staff);
                }
                else
                {
                    return RedirectToAction("MStaff");
                }

            }
        }

        [HttpGet]
        public ActionResult DeleteTrainee(string id)
        {
            using (var FAPCtx = new EF.CMSContext())
            {
                var staff = FAPCtx.Users.FirstOrDefault(c => c.Id == id);

                if (staff != null)
                {
                    TempData["StaffId"] = id;
                    TempData["StaffUN"] = staff.UserName;
                    return View(staff);
                }
                else
                {
                    return RedirectToAction("SHowTrainee");
                }

            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTrainee(string id, UserInfor staff)
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByIdAsync(id);

            if (user != null)
            {
                await manager.DeleteAsync(user);
            }
            @TempData["alert"] = "You have successful delete a Trainee";
            return RedirectToAction("ShowTrainee");

        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, FormCollection f, UserInfor a)
        {
            CustomValidationfTrainee(a);

            if (!ModelState.IsValid)
            {
                SetViewBag();
                return View(a); // return lai Create.cshtml
                                //di kem voi data ma user da go vao
            }
            else
            {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);
            var user = await manager.FindByEmailAsync(a.Email);
            if (!ModelState.IsValid)
            {
                TempData["abc"] = f["formatIds[]"];
                SetViewBag();
                return View(a);
            }

            if (user != null)
            {
                using (var FAPCtx = new EF.CMSContext())
                {
                    a.PasswordHash = user.PasswordHash;
                    a.SecurityStamp = user.SecurityStamp;
                    FAPCtx.Entry<UserInfor>(a).State = System.Data.Entity.EntityState.Modified;

                    FAPCtx.Entry<UserInfor>(a).Collection(x => x.listCourse).Load();
                    a.listCourse = Convert(FAPCtx, f["formatIds[]"]);

                    FAPCtx.SaveChanges();
                }
            }
            }

           
            @TempData["alert"] = "You have successful update a Trainee";
            return RedirectToAction("ShowTrainee");
        }

    }
}