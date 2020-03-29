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
            if (AccountController.IsLoggedIn)
            {

            }
            return View("~/Views/Account/Login.cshtml");
        }

 
    }
}