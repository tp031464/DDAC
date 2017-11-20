using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UIA.Helper;
using UIA.Models;
using UIA.ViewModel;

namespace UIA.Controllers
{
    public class AccountController : Controller
    {
        private UIAEntities db = new UIAEntities();

        // GET: Account
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = (db.Users.Where(u => u.Username == loginViewModel.Username)).FirstOrDefault();

            if (user != null)
            {
                if (PasswordHash.CheckPassword(loginViewModel.Password, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(loginViewModel.Username + "|" + user.Id, false);
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["IncorrectCredentials"] = "IncorrectCredentials";

            return View(loginViewModel);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return View(registerViewModel);

            bool isNewRegistration = true;

            if (db.Users.Any(x => x.Username == registerViewModel.Username))
            {
                ModelState.AddModelError("username", "Username is already taken.");
                isNewRegistration = false;
            }

            if (!isNewRegistration)
            {
                return View(registerViewModel);
            }

            string hashedPassword = PasswordHash.Encrypt(registerViewModel.Password);

            User user = new User()

            {
                Id = registerViewModel.Id,
                Username = registerViewModel.Username,
                Password = hashedPassword,
            };

            db.Users.Add(user);
            db.SaveChanges();


            return RedirectToAction("Login", "Account");
        }
    }
}