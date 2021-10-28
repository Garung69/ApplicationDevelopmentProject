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
            if (!string.IsNullOrEmpty(user.Email))
            {
                if (!user.Email.Contains("@") || (user.Email.Split('@')[0] == "") || (user.Email.Split('@')[1] == "") || user.Email.Split('@')[1] != "gmail.com")
                {
                    ModelState.AddModelError("Email", "Please input a valid Email (abc@gmail.com)");
                }
            }
            if (!string.IsNullOrEmpty(user.Email) && (user.Email.Length >= 21))
            {
                ModelState.AddModelError("Email", "This email is not valid!");
            }
            
        }
        public ActionResult Logout() // function to Logout
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie); //Signout authentication cookie  
                ViewData.Clear(); // Clear All ViewData
                Session.RemoveAll(); // Clear All Session
            }
            return RedirectToAction("LogIn", "Login"); // Redirect user to login page
        }

        // GET: Login
        [HttpGet]
        public ActionResult LogIn()
        {

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> LogIn(UserInfor user) //function use to Login to system
        {
            CustomValidation(user); //check validation
            if (!ModelState.IsValid)
            {
                return View(user); //if data not pass validation user need login again
            }
            else //if login data pass validation data login will be verify
            {
                var context = new CMSContext();                     //
                var store = new UserStore<UserInfor>(context);      //create a connection with the database
                var manager = new UserManager<UserInfor>(store);    //Create data manager 
                var signInManager = new SignInManager<UserInfor, string>(manager, HttpContext.GetOwinContext().Authentication); //create authentication cookie          
                var result = await signInManager.PasswordSignInAsync(userName: user.Email.Split('@')[0], password: user.PasswordHash, isPersistent: false, shouldLockout: false); //signin/verify user
                if (result == SignInStatus.Success)// if user signin success redirect to pages by role
                {
                    var fuser = await manager.FindByEmailAsync(user.Email); //find user in database
                    var userStore = new UserStore<UserInfor>(context);      //
                    var userManager = new UserManager<UserInfor>(userStore);///Create user manager 
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Admin))// if user has role admin -> do action
                    {
                        TempData["UN"] = fuser.UserName; // Mark who is logged in
                        return RedirectToAction("Index", "Admin"); // redirect to Admin page
                    }
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Staff))
                    {
                        TempData["UN"] = fuser.UserName;
                        return RedirectToAction("SearchCategory", "Staff");
                    }  // if user has role staff -> do action
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Trainer))
                    {
                        TempData["username"] = fuser.UserName;
                        return RedirectToAction("Index", "Trainer");
                    }  // if user has role trainer -> do action
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Trainee))
                    {
                        TempData["username"] = fuser.UserName;
                        return RedirectToAction("Index", "Trainee");
                    }    // if user has role trainee -> do action
                    else return Content($"Comming Soon!!!");
                }
                else //if login data not pass validation 
                {
                    ModelState.AddModelError("PasswordHash", "User Name or Password incorrect!"); //create notifications for users
                    return View(); // User login again
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

        private readonly Random _random = new Random();

        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }


        public async Task<ActionResult> CreateTrainer()
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var email = "trainer";
            var password = "123qwe123";
            var phone = "09";
            for (int i = 1; i <= 20; i++)
            {
                var user = await manager.FindByEmailAsync(email + i.ToString() + "@gmail.com");

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = email + i.ToString(),
                        Email = email + i.ToString() + "@gmail.com",
                        PhoneNumber = phone + RandomNumber(100000000, 999999999).ToString(),
                        Age = RandomNumber(10, 100),
                        Name = email + i.ToString(),
                        WorkingPlace = "Ha Noi",
                        Type = "Introvert"
                    };
                    var res = await manager.CreateAsync(user, password);
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
            var phone = "09";
            for (int i = 1; i <= 100; i++)
            {
                var user = await manager.FindByEmailAsync(email + i.ToString() + "@gmail.com");

                if (user == null)
                {
                    user = new UserInfor
                    {
                        UserName = email + i.ToString(),
                        Email = email + i.ToString() + "@gmail.com",
                        PhoneNumber = phone + RandomNumber(100000000, 999999999).ToString(),
                        Age = 18,
                        Name = email + i.ToString(),
                        Education = "High School",
                        ProgrammingLanguage = "C#",
                        Toeic = RandomNumber(100,500).ToString(),
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
                        Age = RandomNumber(10, 100),
                        Name = email + i.ToString(),
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
                    if (i % 2 == 0)
                    {
                        course.CategoryId = 1;
                    }
                    else course.CategoryId = 2;
                    abc.Courses.Add(course);
                    abc.SaveChanges();
                }

            }

            return Content($"Done!");
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
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}