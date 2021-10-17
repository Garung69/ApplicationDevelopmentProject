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
        private void CustomValidationfStaff(UserInfor staff)
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


        [Authorize(Roles = SecurityRoles.Admin)]
        public ActionResult Index()
        {
            using (var ASMCtx = new EF.CMSContext())
            {
                var Staff = ASMCtx.Users.Where(s => s.Role == "staff").ToList();
                ViewBag.UN = TempData["acb"];
                return View(Staff);

/*                using (CMSContext db = new CMSContext())
                {
                    var users = (from user in db.Users
                                 from roles in user.Roles
                                 join role in db.Roles
                                 on roles.RoleId equals role.Id
                                 where role.Name == "admin"
                                 select new { }
                                 ).ToList();
                    return View(users);
                }*/

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

        [Authorize(Roles = SecurityRoles.Admin)]
        [HttpGet]
        public ActionResult CreateStaff()
        {
            return View();
        }

        [Authorize(Roles = SecurityRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult> CreateStaff(UserInfor staff, FormCollection fc)
        {
            CustomValidationfStaff(staff);
            if (!ModelState.IsValid)
            {
                return View(staff); 
            }
            else
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
                        Role = "staff",
                        PasswordHash = "123qwe123",
                        Name = staff.Name
                    };
                    await manager.CreateAsync(user, user.PasswordHash);
                    await CreateRole(staff.Email, "staff");
                }
                return RedirectToAction("Index");
            }

        }

        [Authorize(Roles = SecurityRoles.Admin)]
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

        [Authorize(Roles = SecurityRoles.Admin)]
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

        [Authorize(Roles = SecurityRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult> EditStaff(string id, UserInfor staff)
        {

            CustomValidationfStaff(staff);

            if (!ModelState.IsValid)
            {
                return View(staff);
            }
            else
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(staff.Email);

                if (user != null)
                {


                    user.UserName = staff.Email.Split('@')[0];
                    user.Email = staff.Email;
                    user.PasswordHash = "123qwe123";
                    user.Name = staff.Name;
                    await manager.UpdateAsync(user);
                }             
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = SecurityRoles.Admin)]
        [HttpGet]
        public ActionResult DeleteStaff(string id)
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

        [Authorize(Roles = SecurityRoles.Admin)]
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

        [Authorize(Roles = SecurityRoles.Admin)]
        public ActionResult AMTrainer()
        {
            using (var ASMCtx = new EF.CMSContext())
            {
                var Staff = ASMCtx.Users.Where(s => s.Role == "trainer").ToList();
                return View(Staff);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateTrainer()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateTrainer(UserInfor staff, FormCollection fc)
        {
            CustomValidationfStaff(staff);
            if (!ModelState.IsValid)
            {
                return View(staff); // return lai Create.cshtml
                                        //di kem voi data ma user da go vao
            }
            else
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
                        Name = staff.Name,
                        Role = "trainer",
                        PasswordHash = "123qwe123"

                    };
                    await manager.CreateAsync(user, user.PasswordHash);
                    await CreateRole(staff.Email, "trainer");
                }
                return RedirectToAction("AMTrainer");
            }

        }

        [Authorize(Roles = SecurityRoles.Admin)]
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

        [Authorize(Roles = SecurityRoles.Admin)]
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

        [Authorize(Roles = SecurityRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult> EditTrainer(string id, UserInfor staff)
        {

            CustomValidationfStaff(staff);

            if (!ModelState.IsValid)
            {
                return View(staff);
            }
            else
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var user = await manager.FindByEmailAsync(staff.Email);

                if (user != null)
                {
                    user.UserName = staff.Email.Split('@')[0];
                    user.Email = staff.Email;
                    user.PasswordHash = "123qwe123";
                    user.Name = staff.Name;
                    await manager.UpdateAsync(user);
                }
                return RedirectToAction("AMTrainer");
            }
        }

        [Authorize(Roles = SecurityRoles.Admin)]
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

        [Authorize(Roles = SecurityRoles.Admin)]
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

    }
}