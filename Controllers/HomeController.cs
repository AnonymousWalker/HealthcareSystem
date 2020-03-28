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
        private HealthcareSystemContext Db;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Account model)
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
            bool isCorrect = loginAccount(model.Username, model.Password);
            if (!isCorrect)
            {
                //error message: invalid username or password 
                model.ErrorMessage = "Invalid username or password";
                return View("Login",model);
            }
            return RedirectToAction("Index");
        }


        #region PRIVATE

        private bool addPatientAccount(Account account)
        {
            return true;
        }

        private bool loginAccount(string username, string password)
        {
            var account = Db.Accounts.FirstOrDefault(acc => acc.Username == username);
            if (account!=null && account.Password == password) return true;
            return false;
        }

        #endregion
    }
}