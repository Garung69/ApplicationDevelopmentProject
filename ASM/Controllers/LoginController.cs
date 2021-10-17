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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASM.Controllers
{
    public class LoginController : Controller
    {
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
                        return RedirectToAction("ShowCategory", "Staff");
                    }
                    
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Trainer))
                    {
                        TempData["acb"] = fuser.Id;                  
                        return RedirectToAction( "Index", "Trainer");
                    }
                    if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Trainee))
                    {
                        TempData["xyz"] = fuser.Id;
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

/*        private void SessionLogin(string username)
        {
            if (HttpContext.Session["login"] == null)
            {
                HttpContext.Session["login"] = username;
            }
        }
        public string GetLoginSession()
        {
            return HttpContext.Session["login"] as string;
        }*/

        public async Task<ActionResult> CreateAdmin()
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var email = "thien@gmail.com";
            var password = "Tavip123";
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
                return Content($"Create Admin account Succsess");
            }
            return RedirectToAction("LogIn");
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

    }
}