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
    public class AdminController : Controller
    {
        // GET: Admin
<<<<<<< HEAD
        public ActionResult Index()
        {
            using (var ASMCtx = new EF.CMSContext())
            {
                var Staff = ASMCtx.Users.Where(s => s.Role == "staff").ToList();
                return View(Staff); 
            }
        }

=======
        [HttpGet]
        public ActionResult AdminCreateStaff()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AdminCreateStaff(UserInfor staff)
        {

            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByEmailAsync(staff.Email);

            if (user == null)
            {
                user = new UserInfor
                {
                    UserName = staff.Email.Split('@')[0],
                    Email = staff.Email,
                    PhoneNumber = "None!",
                    Name = staff.Email.Split('@')[0],

                };
                await manager.CreateAsync(user, staff.PasswordHash);
                await CreateRole(staff.Email, "staff");
            }

            return RedirectToAction("LogIn");
        }
>>>>>>> bfbc4f08236c6d47977b37cc373e33e55cac0b3b

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
<<<<<<< HEAD
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Trainee });
=======
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Staff });
>>>>>>> bfbc4f08236c6d47977b37cc373e33e55cac0b3b

            }
            if (!await roleManager.RoleExistsAsync(SecurityRoles.Trainer))
            {
<<<<<<< HEAD
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Trainer });
=======
                await roleManager.CreateAsync(new IdentityRole { Name = SecurityRoles.Staff });
>>>>>>> bfbc4f08236c6d47977b37cc373e33e55cac0b3b
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
<<<<<<< HEAD
            return Content("done!");
        }


        [HttpGet]
        public ActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateStaff(UserInfor staff, FormCollection fc)
        {
            /*            CustomValidationfClass(classRoom);
                        if (!ModelState.IsValid)
                        {
                            //binding gap loi
                            PrepareViewBag();
                            TempData["TeacherIds"] = fc["TeacherIds[]"];
                            return View(classRoom); // return lai Create.cshtml
                            //di kem voi data ma user da go vao
                        }
                        else*/
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(staff.Email);

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = staff.Email.Split('@')[0],
                        Email = staff.Email,
                        Name = staff.Email.Split('@')[0],
                        Role = "staff",

                    };
                    await manager.CreateAsync(user, staff.PasswordHash);
                    await CreateRole(staff.Email, "staff");
                }
                return RedirectToAction("Index");
            }

        }

        public ActionResult DStaff(string id)
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
                    return RedirectToAction("Index");
                }

            }

        }

        [HttpGet]
        public ActionResult EditStaff(string id)
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
                    return RedirectToAction("Index");
                }

            }
        }

        [HttpPost]
        public async Task<ActionResult> EditStaff(string id, UserInfor staff)
        {

            /*            CustomValidationfStudent(student);

                        if (!ModelState.IsValid)
                        {
                            PrepareViewBag();
                            return View(student);
                        }
                        else*/
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                staff.UserName = staff.Email.Split('@')[0];
                staff.Name = staff.Email.Split('@')[0];
                staff.Role = "staff";
                await manager.UpdateAsync(staff);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> DeleteStaff(string id)
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


        [HttpPost]
        public async Task<ActionResult> DeleteStaff(string id, UserInfor staff)
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByIdAsync(id);

            if(user != null)
            {
                await manager.DeleteAsync(user);
            }

            return RedirectToAction("Index");

        }

        /// <summary>
        ///  //////////////////////////////////////////////////Trainer///////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        public ActionResult AMTrainer()
        {
            using (var ASMCtx = new EF.CMSContext())
            {
                var Staff = ASMCtx.Users.Where(s => s.Role == "trainer").ToList();
                return View(Staff);
            }
        }

        [HttpGet]
        public ActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTrainer(UserInfor staff, FormCollection fc)
        {
            /*            CustomValidationfClass(classRoom);
                        if (!ModelState.IsValid)
                        {
                            //binding gap loi
                            PrepareViewBag();
                            TempData["TeacherIds"] = fc["TeacherIds[]"];
                            return View(classRoom); // return lai Create.cshtml
                            //di kem voi data ma user da go vao
                        }
                        else*/
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(staff.Email);

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = staff.Email.Split('@')[0],
                        Email = staff.Email,
                        Name = staff.Email.Split('@')[0],
                        Role = "trainer",

                    };
                    await manager.CreateAsync(user, staff.PasswordHash);
                    await CreateRole(staff.Email, "trainer");
                }
                return RedirectToAction("AMTrainer");
            }

        }

        public ActionResult DTrainer(string id)
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
                    return RedirectToAction("AMTrainer");
                }

            }

        }

        [HttpGet]
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

        [HttpPost]
        public async Task<ActionResult> EditTrainer(string id, UserInfor staff)
        {

            /*            CustomValidationfStudent(student);

                        if (!ModelState.IsValid)
                        {
                            PrepareViewBag();
                            return View(student);
                        }
                        else*/
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                staff.UserName = staff.Email.Split('@')[0];
                staff.Name = staff.Email.Split('@')[0];
                staff.Role = "trainer";
                await manager.UpdateAsync(staff);
                return RedirectToAction("AMTrainer");
            }
        }

        [HttpGet]
        public ActionResult DeleteTrainer(string id)
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
                    return RedirectToAction("AMTrainer");
                }

            }
        }


        [HttpPost]
        public async Task<ActionResult> DeleteTrainer(string id, UserInfor staff)
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var user = await manager.FindByIdAsync(id);

            if (user != null)
            {
                await manager.DeleteAsync(user);
            }

            return RedirectToAction("Index");

        }







=======
            return Content("admin done!");
        }

>>>>>>> bfbc4f08236c6d47977b37cc373e33e55cac0b3b
    }
}