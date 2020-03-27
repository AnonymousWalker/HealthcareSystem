using HealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthcareSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(AccountModel model)
        {
            bool isNewAccount = addPatientAccount(model);
            if (isNewAccount)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("SignUp");  // account already exists, reprompt with message
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            bool isAuth = loginAccount(model.Username, model.Password);
            return RedirectToAction("Index");
        }


        #region PRIVATE

        private bool addPatientAccount(AccountModel account)
        {
            return true;
        }

        private bool loginAccount(string username, string password)
        {
            return true;
        }

        #endregion
    }
}