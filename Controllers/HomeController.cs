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

        public HomeController()
        {
            Db = new HealthcareSystemContext();
        }

        public ActionResult Index()
        {
            if (AccountController.IsLoggedIn)
            {

            }
            return View("~/Views/Account/Login.cshtml");
        }

        public ActionResult Apointment()
        {
            var appointments = Db.Appointments.Where(ap => ap.Time.Date == DateTime.Today);
            TimeSpan t;
            for (int i = 9; i <= 16; i++)
            {
                t = new TimeSpan(i, 0, 0);   
                
            }
            return View();
        }
 
    }
}