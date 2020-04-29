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
            var id = Convert.ToInt32(Session["AccountId"]);
            if (!IsLoggedIn || id == 0) {
                RedirectToAction("Login");
            }

            // Authorization
            var staff = Db.Accounts.OfType<EmployeeAccount>().FirstOrDefault(acc => acc.AccountId == id);
            try
            {
                //Staff Account
                if (staff != null)
                {
                    var model = new ProfileModel
                    {
                        AccountId = staff.AccountId,
                        Email = staff.Email,
                        Password = staff.Password,
                        Firstname = staff.Firstname,
                        Lastname = staff.Lastname,
                        Phone = staff.Phone,
                        Salary = staff.Salary,
                        SSN = staff.SSN,
                        Role = staff.Role,
                        AccountType = AccountType.Employee
                    };
                    return View("StaffProfile",model);
                }
                else
                {
                    PatientAccount patient = Db.Accounts.OfType<PatientAccount>().FirstOrDefault(acc => acc.AccountId == id);
                    var model = new ProfileModel
                    {
                        AccountId = patient.AccountId,
                        Email = patient.Email,
                        Password = patient.Password,
                        Firstname = patient.Firstname,
                        Lastname = patient.Lastname,
                        Phone = patient.Phone,
                        BillingAddress = patient.BillingAddress,
                        InsuranceNumber = patient.InsuranceNumber,
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
                return RedirectToAction("Index", "Home");
            }

            model.ErrorMessage = "Account has already existed!";
            return View("SignUp", model);
        }

        public ActionResult Login()
        {
            if (IsLoggedIn && Session["AccountId"] != null) return RedirectToAction("Index");
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }
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

        public ActionResult SignOut()
        {
            if (IsLoggedIn || Session["AccountId"] != null)
            {
                IsLoggedIn = false;
                Session["AccountId"] = null;
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditPatientProfile(ProfileModel model)
        {
            if (!isPatientAccess())
            {
                ViewBag.ErrorMessage = "You don't have permission to edit the profile of patient";
                return View("~/Views/Shared/Error.cshtml");
            }
            if (ModelState.IsValid)
            {
                var account = Db.Accounts.OfType<PatientAccount>().FirstOrDefault(acc => acc.AccountId == model.AccountId);
                if (account != null)
                {
                    account.Password = model.Password;
                    account.Firstname = model.Firstname;
                    account.Lastname = model.Lastname;
                    account.Phone = model.Phone;
                    account.BillingAddress = model.BillingAddress;
                    account.InsuranceNumber = model.InsuranceNumber;
                    Db.SaveChanges();
                }
            }
            return PartialView("_EditProfile", model);
        }

        #region PRIVATE
        private bool isPatientAccess()
        {
            var id = Convert.ToInt32(Session["AccountId"]);
            if (!IsLoggedIn || id == 0)
            {
                return false;
            }
            var account = Db.Accounts.OfType<PatientAccount>().FirstOrDefault(acc => acc.AccountId == id);
            return (account!=null);
        }

        private bool addPatientAccount(SignUpModel account)
        {
            bool checkAccount = Db.Accounts.Any(acc => acc.Email == account.Email); 
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