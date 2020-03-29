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
        public static bool IsLoggedIn = false;

        public AccountController()
        {
            Db = new HealthcareSystemContext();
        }

        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["AccountId"]);
            if (id != 0)
            {
                PatientAccount pAccount = Db.Accounts.OfType<PatientAccount>().FirstOrDefault(acc => acc.AccountId == id);
                var model = new ProfileModel
                {
                    Email = pAccount.Email,
                    Password = pAccount.Password,
                    Firstname = pAccount.Firstname,
                    Lastname = pAccount.Lastname,
                    BillingAddress = pAccount.BillingAddress,
                    InsuranceNumber = pAccount.InsuranceNumber
                };
                return View(model);
            }
            return RedirectToAction("Login");
        }

        public ActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            bool isNewAccount = addPatientAccount(model);
            if (isNewAccount)
            {
                return View("Login", new LoginModel { Email = model.Email });
            }

            model.ErrorMessage = "Account has already existed!";
            return View("SignUp", model);
        }

        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var account = verifyAccount(model.Email, model.Password);
            if (account != null)
            {
                IsLoggedIn = true;
                Session["AccountId"] = account.AccountId;
                return RedirectToAction("Index", "Home");
            }
            //error message: invalid username or password 
            model.ErrorMessage = "Invalid username or password";
            return View("Login", model);

        }



        #region PRIVATE

        private bool addPatientAccount(SignUpModel account)
        {
            bool checkAccount = Db.Accounts.Any(acc => acc.Email == account.Email); //false;
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