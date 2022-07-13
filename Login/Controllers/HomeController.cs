using Login.Models;
using Login.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Login.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register registerDetails)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new demoEntities())
                {

                    Signup reglog = new Signup();


                    reglog.FirstName = registerDetails.FirstName;
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.Password = registerDetails.Password;


                    databaseContext.Signups.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {

                return View("Register", registerDetails);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var isValidUser = IsValidUser(model);

                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        public Signup IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new demoEntities())
            {
                Signup user = dataContext.Signups.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                if (user == null)
                    return null;
                else
                    return user;
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); 
            return RedirectToAction("Index");
        }




    }
}