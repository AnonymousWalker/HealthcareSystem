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
            if (AccountController.IsLoggedIn)
            {
                var id = Convert.ToInt32(Session["AccountId"]);
                var account = Db.Accounts.Find(id);
                ViewBag.FirstName = account.Firstname;
                ViewBag.LastName = account.Lastname;
            }
            return View();
        }


        #region PRIVATE

       
        #endregion
    }
}