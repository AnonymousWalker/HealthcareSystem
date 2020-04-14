using HealthcareSystem.Models;
using HealthcareSystem.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthcareSystem.Controllers
{
    public class StaffController : Controller
    {
        private HealthcareSystemContext Db;

        public StaffController()
        {
            Db = new HealthcareSystemContext();
        }

        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        //doctor create treatment, prescription record => charge amount
        public ActionResult InputTreatmentService(HealthcareServiceModel model)
        {
            Db.ServiceStatements.Add(new ServiceStatement
            {
                Examination = model.Examination,
                Treatment = model.Treatment,
                Prescription = model.Prescription,
                Date = model.Date,
                Amount = model.Amount,
                PatientId = model.PatientId
            });
            Db.SaveChanges();

            return null;
        }

    }
}