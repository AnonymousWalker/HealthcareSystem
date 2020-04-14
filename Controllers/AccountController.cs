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


        //Profile
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["AccountId"]);
            if (!IsLoggedIn || id == 0) {
                RedirectToAction("Login");
            }

            // Authorization
            var sAccount = Db.Accounts.OfType<EmployeeAccount>().FirstOrDefault(acc => acc.AccountId == id);
            try
            {
                //Staff Account
                if (sAccount != null)
                {
                    var model = new ProfileModel
                    {
                        Email = sAccount.Email,
                        Password = sAccount.Password,
                        Firstname = sAccount.Firstname,
                        Lastname = sAccount.Lastname,
                        Salary = sAccount.Salary,
                        SSN = sAccount.SSN,
                        Role = sAccount.Role,
                        AccountType = AccountType.Employee
                    };
                    return View("StaffProfile",model);
                }
                else
                {
                    PatientAccount pAccount = Db.Accounts.OfType<PatientAccount>().FirstOrDefault(acc => acc.AccountId == id);
                    var model = new ProfileModel
                    {
                        Email = pAccount.Email,
                        Password = pAccount.Password,
                        Firstname = pAccount.Firstname,
                        Lastname = pAccount.Lastname,
                        BillingAddress = pAccount.BillingAddress,
                        InsuranceNumber = pAccount.InsuranceNumber,
                        AccountType = AccountType.Patient
                    };
                    return View("PatientProfile", model);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            //validate input + decrypt?
            if (!ModelState.IsValid)
            {
                return View("SignUp", model);
            }

            bool isNewAccount = addPatientAccount(model);
            if (isNewAccount)
            {
                IsLoggedIn = true;
                var account = verifyExistingAccount(model.Email, model.Password);
                Session["AccountId"] = account.AccountId;
                return View("Index", "Home");
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
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }
            //validate + decrypt?
            var account = verifyExistingAccount(model.Email, model.Password);
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

        private Account verifyExistingAccount(string email, string password)
        {
            var account = Db.Accounts.FirstOrDefault(acc => acc.Email == email);
            if (account != null && account.Password == password)
                return account;
            return null;
        }

        #endregion
    }
}