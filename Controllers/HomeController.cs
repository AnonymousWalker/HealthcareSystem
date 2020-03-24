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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            addPatientAccount(model);
            return RedirectToAction("Index");
        }



        #region PRIVATE

        private void addPatientAccount(SignUpModel account)
        {

        }
        #endregion
    }
}