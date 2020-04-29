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
            var id = Convert.ToInt32(Session["AccountId"]);
            if (AccountController.IsLoggedIn)
            {
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
            var id = Convert.ToInt32(Session["AccountId"]);
            if (AccountController.IsLoggedIn && id != 0)
            {
                var account = Db.Accounts.Find(id);
                if (account is EmployeeAccount) model.Role = ((EmployeeAccount)account).Role;
            }

            return PartialView("~/Views/Shared/_NavigationBar.cshtml", model);
        }

        #region PRIVATE


        #endregion
    }
}