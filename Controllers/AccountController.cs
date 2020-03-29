using HealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthcareSystem.Controllers
{
    public class AccountController : Controller
    {
        private HealthcareSystemContext Db;
        public static bool isLoggedIn = false;

        public AccountController()
        {
            Db = new HealthcareSystemContext();
        }

        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            bool isNewAccount = addPatientAccount(model);
            if (isNewAccount)
            {
                return View("Login");
            }

            model.ErrorMessage = "Account has already existed!";
            return View("SignUp", model);  
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var account = verifyAccount(model.Email, model.Password);
            if (account != null)
            {
                isLoggedIn = true;
                Session["AccountId"] = account.AccountId;
                return RedirectToAction("Index");
            }
            //error message: invalid username or password 
            model.ErrorMessage = "Invalid username or password";
            return View("Login", model);

        }


        #region PRIVATE

        private bool addPatientAccount(SignUpModel account)
        {
            bool checkAccount = false;// Db.Accounts.Any(acc => acc.Email == account.Email);
            if (checkAccount)
            {
                return false;
            }
            else
            {
                Db.Accounts.Add(new PatientAccount
                {
                    Email = account.Email,
                    Password = account.Password,
                    Firstname = account.Firstname,
                    Lastname = account.Lastname,
                    Phone = account.Phone
                });
                Db.SaveChanges();
            }
            return true;
        }

        private Account verifyAccount(string email, string password)
        {
            var account = Db.Accounts.FirstOrDefault(acc => acc.Email == email);
            if (account != null && account.Password == password)
                return account;
            return null;
        }

        #endregion
    }
}