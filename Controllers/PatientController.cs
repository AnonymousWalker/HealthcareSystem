using System;
using HealthcareSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthcareSystem.Models.Tables;

namespace HealthcareSystem.Controllers
{
    public class PatientController : Controller
    {
        private HealthcareSystemContext Db;

        public PatientController()
        {
            Db = new HealthcareSystemContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MedicalRecord(int patientId = 0)
        {
            if (patientId != 0)
            {
                var model = getMedicalRecords(patientId);
                return View(model);
            }
            return View();
        }

        public ActionResult UpdateMedicalRecord()
        {
            //nurse update patient's medical record after health check
            return null;
        }

        //doctor create treatment, prescription 
        //public ActionResult createStatement

        #region PRIVATE

        private IList<MedicalRecord> getMedicalRecords(int patientId)
        {
            var records = Db.MedicalRecords.Where(rec => rec.PatientId == patientId)
                                        .OrderByDescending(rec => rec.Date)
                                        .ToList();
            return records;
        }

        #endregion
    }
}