using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpConstruction.Areas.Manage.ViewModels;
using UpConstruction.Models;

namespace UpConstruction.Areas.Manage.Controllers
{
    [Area("manage")]

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser
        //    {
        //        FullName = "Super Admin",
        //        UserName = "SuperAdmin"
        //    };
        //    var result = await _userManager.CreateAsync(admin, "Admin123");
        //    if (!result.Succeeded)
        //    {
        //        return Ok(result.Errors);
        //    }

        //    return Content("Admin was created!");
        //}

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel admin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == admin.UserName);

            if (user==null)
            {
                ModelState.AddModelError("", "UserName or PassWord is not correct");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, admin.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or PassWord is not correct");
                return View();
            }
                
            return RedirectToAction("index","dashboard");

        }
    }
}
