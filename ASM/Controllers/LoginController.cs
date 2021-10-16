using ASM.EF;
using ASM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASM.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
   

        [HttpPost]
        public async Task<ActionResult> LogIn(UserInfor user)
        {
            var context = new CMSContext();
            var store = new UserStore<UserInfor>(context);
            var manager = new UserManager<UserInfor>(store);

            var signInManager
                = new SignInManager<UserInfor, string>(manager, HttpContext.GetOwinContext().Authentication);

            var fuser = await manager.FindByEmailAsync(user.Email);

            var result = await signInManager.PasswordSignInAsync(userName: user.Email.Split('@')[0], password: user.PasswordHash, isPersistent: false, shouldLockout: false);


            var userStore = new UserStore<UserInfor>(context);
            var userManager = new UserManager<UserInfor>(userStore);


            if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Admin))
            {
                return RedirectToAction("Test");
            }
            if (await userManager.IsInRoleAsync(fuser.Id, SecurityRoles.Staff))
            {
                return Content("dcmm");
            }
            else return RedirectToAction("Test2");


        }

       
    }
}