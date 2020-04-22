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

        public ActionResult InputMedicalRecord(int patientId)
        {
            var patient = Db.Accounts.Find(patientId);
            return View(new MedicalRecordModel { PatientId = patientId , PatientName = patient.Firstname + " " + patient.Lastname });
        }

        [HttpPost]
        public ActionResult InputMedicalRecord(MedicalRecordModel record)
        {
            if (ModelState.IsValid)
            {
                Db.MedicalRecords.Add(new MedicalRecord
                {
                    Weight = record.Weight,
                    Height = record.Height,
                    BloodPressure = record.BloodPressure,
                    Pulse = record.Pulse,
                    Description = record.Description,
                    Date = record.Date,
                    LabResult = record.LabResult,
                    PathologyReport = record.PathologyReport,
                    RadiologyReport = record.RadiologyReport,
                    AllegyInformation = record.AllegyInformation,
                    PatientId = record.PatientId
                });
                Db.SaveChanges();
                return RedirectToAction("MedicalRecord", "Patient", new { patientId = record.PatientId });
            }
            //error
            return View(record);
        }

        public ActionResult EditMedicalRecord(MedicalRecordModel model)
        {
            //nurse update patient's medical record after health check
            var date = DateTime.Parse(model.DateString);
            var record = Db.MedicalRecords.Where(rec => rec.PatientId == model.Id && rec.Date == date).FirstOrDefault();
            if (record != null)
            {
                record.Weight = model.Weight;
                record.Height = model.Height;
                record.BloodPressure = model.BloodPressure;
                record.Pulse = model.Pulse;
                record.Description = model.Description;
                record.Date = date;
            }
                Db.SaveChanges();

            return null;
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

        public ActionResult SearchPatient(string firstName, string lastName = "", string phone = "", string address = "", int patientId = 0)
        {
            IList<PatientAccount> patientList;
            patientList = Db.Accounts.OfType<PatientAccount>()
                        .Where(p => p.Firstname.Contains(firstName) || p.Lastname.Contains(lastName))
                        .ToList();
            if (address != "")
            {
                patientList = patientList.Where(p => p.BillingAddress.Contains(address)).ToList();
            }
            if (phone != "")
            {
                patientList = patientList.Where(p => p.Phone.Contains(address)).ToList();
            }

            return PartialView("_PatientList", patientList);
        }

        public ActionResult PatientMedicalRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PatientMedicalRecord(int patientId)
        {
            ViewBag.PatientName = Db.Accounts.Find(patientId);
            var records = getMedicalRecords(patientId);
            return View("~/Views/Patient/MedicalRecord.cshtml", records);
        }

        #region PRIVATE
        private List<MedicalRecordModel> getMedicalRecords(int patientId)
        {
            return Db.MedicalRecords.Where(rec => rec.PatientId == patientId)
                            .Join(Db.Accounts, rec => rec.PatientId, acc => acc.AccountId, (rec, acc)
                            => new MedicalRecordModel
                            {
                                Weight = rec.Weight,
                                Height = rec.Height,
                                BloodPressure = rec.BloodPressure,
                                Pulse = rec.Pulse,
                                Description = rec.Description,
                                Date = rec.Date,
                                LabResult = rec.LabResult,
                                PathologyReport = rec.PathologyReport,
                                RadiologyReport = rec.RadiologyReport,
                                AllegyInformation = rec.AllegyInformation,
                                PatientId = rec.PatientId,
                                PatientName = acc.Firstname + " " + acc.Lastname
                            })
                            .OrderByDescending(rec => rec.Date)
                            .ToList();
        }
        #endregion
    }
}