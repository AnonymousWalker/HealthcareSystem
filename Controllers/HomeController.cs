using HealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HealthcareSystem.Controllers
{
    public class HomeController : Controller
    {
        private HealthcareSystemContext Db;

        public HomeController()
        {
            Db = new HealthcareSystemContext();
        }

        public ActionResult Index()
        {
            var model = new IndexModel();
            if (AccountController.IsLoggedIn && Session["AccountId"] != null)
            {
                var id = Convert.ToInt32(Session["AccountId"]);
                var account = Db.Accounts.Find(id);
                model.AccountId = id;
                model.Name = account.Firstname + " " + account.Lastname;
                if (account is EmployeeAccount) model.Role = ((EmployeeAccount)account).Role;
            }
            return View(model);
        }

        public ActionResult NavigationBar()
        {
            var model = new IndexModel();
            if (AccountController.IsLoggedIn && Session["AccountId"] != null)
            {
                var id = Convert.ToInt32(Session["AccountId"]);
                var account = Db.Accounts.Find(id);
                if (account is EmployeeAccount) model.Role = ((EmployeeAccount)account).Role;
            }

            return PartialView("~/Views/Shared/_NavigationBar.cshtml", model);
        }

        #region PRIVATE

       
        #endregion
    }
}