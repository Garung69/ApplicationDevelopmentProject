using ASM.EF;
using ASM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASM.Controllers
{
    public class LoginController : Controller
    {





        /// <summary>
        ///                        // CREATE SEED 
        /// </summary>
        /// <returns></returns>

        //  update-database -ConfigurationTypeName ASM.EF.Migrations.Configuration -TargetMigration:InitialCreate -Verbose
        public async Task<ActionResult> NewSeed()
        {
            await CreateAdmin();
            await CreateTrainer();
            await CreateTrainee();
            await CreateStaff();
            CreateCourseCategory();
            CreateCourse();

            return RedirectToAction("login");
        }








        private void CustomValidation(UserInfor user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("Email", "Please input Email");
            }
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                ModelState.AddModelError("PasswordHash", "Please input PassWord");
            }
            if (!string.IsNullOrEmpty(user.Email) && (user.Email.Split('@')[0] == null) && (user.Email.Split('@')[1] == null)  && (user.Email.Split('@')[1] != "gmail.com"))
            {
                ModelState.AddModelError("Email", "Please a valid Email (abc@gmail.com)");
            }
            if (!string.IsNullOrEmpty(user.Email) && (user.Email.Length >= 21))
            {
                ModelState.AddModelError("Email", "This email is not valid!");
            }
            if (!string.IsNullOrEmpty(user.PasswordHash) && user.PasswordHash.Length <= 7)
            {
                ModelState.AddModelError("PasswordHash", "PassWord need more than 7 characters");
            }
        }
        public  ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                ViewData.Clear();
                Session.RemoveAll();
            }

            return RedirectToAction("LogIn", "Login");
        }

        // GET: Login
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
   

        [HttpPost]
        public async Task<ActionResult> LogIn(UserInfor user)
        {
            CustomValidation(user);

            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                var context = new CMSContext();
                var store = new UserStore<UserInfor>(context);
                var manager = new UserManager<UserInfor>(store);

                var signInManager
                    = new SignInManager<UserInfor, string>(manager, HttpContext.GetOwinContext().Authentication);

                var fuser = await manager.FindByEmailAsync(user.Email);

                var result = await signInManager.PasswordSignInAsync(userName: user.Email.Split('@')[0], password: user.PasswordHash, isPersistent: false, shouldLockout: false);

                if (result == SignInStatus.Success)
                {
                    var userStore = new UserStore<UserInfor>(context);
                    var userManager = new UserManager<UserInfor>(userStore);
                    

                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Admin))
                    {
                        /*SessionLogin(fuser.UserName);*/
                        TempData["acb"] = fuser.UserName;
                        return RedirectToAction( "Index", "Admin");
                    }
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Staff))
                    {
                        TempData["UN"] = fuser.UserName;
                        return RedirectToAction("SearchCategory", "Staff");
                    }
                    
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Trainer))
                    {
                        TempData["username"] = fuser.UserName;                  
                        return RedirectToAction( "Index", "Trainer");
                    }
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Trainee))
                    {
                        TempData["username"] = fuser.UserName;
                        return RedirectToAction("Index", "Trainee");
                    }
                    else return Content($"Comming Soon!!!");
                }
                else
                {
                    ModelState.AddModelError("PasswordHash", "User Name or Password incorrect!");
                    return View();
                }
            }

        }



        public async Task<ActionResult> CreateAdmin()
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var email = "thien@gmail.com";
            var password = "123qwe123";
            var phone = "0961119526";
            var role = "admin";

            var user = await manager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new UserInfor
                {
                    UserName = email.Split('@')[0],
                    Email = email,
                    PhoneNumber = phone,
                    Name = email.Split('@')[0],
                    Role = role,
                };
                await manager.CreateAsync(user, password);
                await CreateRole(user.Email, "admin");             
            }
            return Content($"On Processing 20%");
        }

        public async Task<ActionResult> CreateTrainer()
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var email = "trainer";
            var password = "123qwe123";
            var phone = "0961119526";
            for(int i =1; i<= 20; i++)
            {
                var user = await manager.FindByEmailAsync(email+ i.ToString() + "@gmail.com");

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = email + i.ToString(),
                        Email = email + i.ToString() + "@gmail.com",
                        PhoneNumber = phone,
                        Age = 18,
                        Name = "Le Minh Thien",
                        WorkingPlace = "Ha Noi",
                        Type = "Introvert"
                    };
                    var res =  await manager.CreateAsync(user, password);
                    if (res.Succeeded)
                    {
                        await CreateRole(user.Email, "trainer");
                    }
                                    
                }
            }

            return Content($"On Processing 40%");
        }

        public async Task<ActionResult> CreateTrainee()
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var email = "trainee";
            var password = "123qwe123";
            var phone = "0961119526";
            for (int i = 1; i <= 100; i++)
            {
                var user = await manager.FindByEmailAsync(email + i.ToString() + "@gmail.com");

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = email + i.ToString(),
                        Email = email + i.ToString() + "@gmail.com",
                        PhoneNumber = phone,
                        Age = 18,
                        Name = "Le Minh Thien",
                        Education = "High School",
                        ProgrammingLanguage = "C#",
                        Toeic = "9",
                        Experience = "None!",
                        Department = "GCH0803",
                        Location = "Lao Cai"
                    };
                    var res = await manager.CreateAsync(user, password);
                    if (res.Succeeded)
                    {
                        await CreateRole(user.Email, "trainee");
                    }

                }
            }

            return Content($"On Processing 60%");
        }

        public async Task<ActionResult> CreateStaff()
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var email = "staff";
            var password = "123qwe123";
            for (int i = 1; i <= 2; i++)
            {
                var user = await manager.FindByEmailAsync(email + i.ToString() + "@gmail.com");

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = email + i.ToString(),
                        Email = email + i.ToString() + "@gmail.com",
                        Age = 18,
                        Name = "Le Minh Thien",
                    };
                    var res = await manager.CreateAsync(user, password);
                    if (res.Succeeded)
                    {
                        await CreateRole(user.Email, "staff");
                    }

                }
            }

            return Content($"On Processing 80%");
        }

        public ActionResult CreateCourseCategory()
        {
            using (var abc = new EF.CMSContext())
            {
                for (int i = 1; i <= 5; i++)
                {
                    CourseCategoryEntity course = new CourseCategoryEntity();
                    if (i % 2 == 0)
                    {
                        course.Name = "IT";
                    }
                    else course.Name = "DESIGN";
                    course.Description = "None!";
                    abc.courseCategoryEntities.Add(course);
                    abc.SaveChanges();
                }

            }

            return Content($"On Processing 99$");
        }

        public ActionResult CreateCourse()
        {
            var name = "CourseNo";
            using (var abc = new EF.CMSContext())
            {
                for (int i = 1; i <= 10; i++)
                {
                    CourseEntity course = new CourseEntity();
                    course.Name = name + i.ToString();
                    course.Description = "None!";
                    if(i % 2 == 0)
                    {
                        course.CategoryId = 1;
                    }
                    else course.CategoryId = 2;
                    abc.Courses.Add(course);
                    abc.SaveChanges();
                }

            }

            return  Content($"Done!");
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


        public ActionResult Test()
        {
            return View();
        }
    }
}