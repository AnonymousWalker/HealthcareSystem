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
                // New Record
                if (record.RecordId == 0)
                {
                    var date = DateTime.Parse(record.DateString);
                    Db.MedicalRecords.Add(new MedicalRecord
                    {
                        Weight = record.Weight,
                        Height = record.Height,
                        BloodPressure = record.BloodPressure,
                        Pulse = record.Pulse,
                        Description = record.Description,
                        Date = date,
                        LabResult = record.LabResult,
                        PathologyReport = record.PathologyReport,
                        RadiologyReport = record.RadiologyReport,
                        AllegyInformation = record.AllegyInformation,
                        PatientId = record.PatientId
                    });
                }
                else // Update record
                {
                    var medicalRecord = Db.MedicalRecords.Where(rec => rec.Id == record.RecordId).FirstOrDefault();
                    if (medicalRecord!=null)
                    {
                        var date = DateTime.Parse(record.DateString);
                        medicalRecord.Date = date;
                        medicalRecord.Weight = record.Weight;
                        medicalRecord.Height = record.Height;
                        medicalRecord.BloodPressure = record.BloodPressure;
                        medicalRecord.Pulse = record.Pulse;
                        medicalRecord.Description = record.Description;
                        medicalRecord.LabResult = record.LabResult;
                        medicalRecord.PathologyReport = record.PathologyReport;
                        medicalRecord.RadiologyReport = record.RadiologyReport;
                        medicalRecord.AllegyInformation = record.AllegyInformation;
                        medicalRecord.PatientId = record.PatientId;
                    }
                }
                Db.SaveChanges();
                return RedirectToAction("PatientMedicalRecords", "Staff", new { patientId = record.PatientId });
            }
            //error
            return View(record);
        }

        [HttpGet]
        public ActionResult EditMedicalRecord(int recordId)
        {
            // Doctor/nurse 
            var record = Db.MedicalRecords.Find(recordId);
            if (record != null)
            {
                var patient = Db.Accounts.Find(record.PatientId);
                var model = new MedicalRecordModel
                {
                    RecordId = recordId,
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
                };
                model.PatientName = (patient != null) ? patient.Firstname + " " + patient.Lastname : "Unknown Patient";
                model.DateString = string.Format("{0}-{1:00}-{2:00}", record.Date.Year, record.Date.Month, record.Date.Day);
                return View("InputMedicalRecord", model);
            }

            return RedirectToAction("SearchPatient", new { actionType = "view" });
        }

        public ActionResult SearchPatient(string actionType = "")
        {
            //action from index: "input" or "view" medical record
            if (actionType != "")
            {
                ViewBag.Action = actionType;
            }
            return View();
        }

        //AJAX
        [HttpPost]
        public ActionResult SearchPatient(string firstName, string phone = "", string lastName = "")
        {
            IList<PatientAccount> patientList;
            if (phone != "")
            {
                patientList = Db.Accounts.OfType<PatientAccount>()
                            .Where(p => p.Firstname.Contains(firstName) || p.Phone.Contains(phone))
                            .ToList();
            } else if (lastName != "")
            {
                patientList = Db.Accounts.OfType<PatientAccount>()
                            .Where(p => p.Firstname.Contains(firstName) || p.Lastname.Contains(lastName))
                            .ToList();
            }
            patientList = Db.Accounts.OfType<PatientAccount>()
                            .Where(p => p.Firstname.Contains(firstName))
                            .ToList();


            return PartialView("_PatientList", patientList);
        }

        public ActionResult PatientMedicalRecords(int patientId)
        {
            var patient = Db.Accounts.Find(patientId);
            var patientName = (patient != null) ? patient.Firstname + " " + patient.Lastname : "Unknown Patient";
            var records = getMedicalRecords(patientId);
            return View(new PatientMedicalRecordModel {
                PatientId = patient.AccountId,
                PatientName = patientName,
                Records = records
            });
        }

        //Doctor Appointments
        public ActionResult AppointmentList()
        {
            if (AccountController.IsLoggedIn)
            {
                var id = Convert.ToInt32(Session["AccountId"]);
                var model = getAppointments(id);
                return View("DoctorAppointmentList",model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult UpdateServiceTreatment(int appointmentId)
        {
            var statement = Db.ServiceStatements.Where(s => s.AppointmentId == appointmentId).FirstOrDefault();
            var appointment = Db.Appointments.Find(appointmentId);
            // New Statement 
            if (statement == null && appointment != null)
            {
                return View("ServiceTreatment", new ServiceTreatmentModel() {
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    AppointmentId = appointmentId
                });
            }
            // Update
            else if (statement.Status == false && appointment!=null) 
            {
                var serviceList = Db.ServiceStatementDetails.Where(s => s.StatementId == statement.Id).ToList();
                var model = new ServiceTreatmentModel();
                model.StatementId = statement.Id;
                model.Prescription = statement.Prescription;
                model.AppointmentId = appointmentId;
                model.Status = statement.Status;

                // load checkboxes values
                foreach (var service in serviceList)
                {
                    switch (service.ServiceId)
                    {
                        case 1:
                            model.BloodTest = true;
                            break;
                        case 2:
                            model.Xray = true;
                            break;
                        case 3:
                            model.MRI = true;
                            break;
                        case 4:
                            model.Radiology = true;
                            break;
                        case 5:
                            model.LabTest = true;
                            break;
                        default: break;
                    }
                }
                
                return View("ServiceTreatment", model);
            }

            return RedirectToAction("AppointmentList");
        }

        [HttpPost]
        public ActionResult UpdateServiceTreatment(ServiceTreatmentModel model)
        {
            if (ModelState.IsValid) {
                if (model.StatementId == 0)  // create new statement
                {
                    var newStatement = Db.ServiceStatements.Add(new ServiceStatement {
                        Date = DateTime.Today,
                        Prescription = model.Prescription,
                        AppointmentId = model.AppointmentId,
                        DoctorId = model.DoctorId,
                        PatientId = model.PatientId,
                    });

                    List<ServiceStatementDetail> serviceDetails = new List<ServiceStatementDetail>();
                    if (model.BloodTest) serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = 1 });
                    if (model.Xray) serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = 2 });
                    if (model.MRI) serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = 3 });
                    if (model.Radiology) serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = 4 });
                    if (model.LabTest) serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = 5 });
                    serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = 6 });

                    Db.ServiceStatementDetails.AddRange(serviceDetails);
                }
                else // update statement
                {
                    var statement = Db.ServiceStatements.Where(s => s.Id == model.StatementId).FirstOrDefault();
                    if (statement != null)
                    {
                        statement.Prescription = model.Prescription;

                        List<ServiceStatementDetail> serviceDetails = new List<ServiceStatementDetail>();
                        if (model.BloodTest) serviceDetails.Add(new ServiceStatementDetail { StatementId = model.StatementId, ServiceId = 1 });
                        if (model.Xray) serviceDetails.Add(new ServiceStatementDetail { StatementId = model.StatementId, ServiceId = 2 });
                        if (model.MRI) serviceDetails.Add(new ServiceStatementDetail { StatementId = model.StatementId, ServiceId = 3 });
                        if (model.Radiology) serviceDetails.Add(new ServiceStatementDetail { StatementId = model.StatementId, ServiceId = 4 });
                        if (model.LabTest) serviceDetails.Add(new ServiceStatementDetail { StatementId = model.StatementId, ServiceId = 5 });
                        serviceDetails.Add(new ServiceStatementDetail { StatementId = statement.Id, ServiceId = 6 });

                        // remove old statement details
                        var details = Db.ServiceStatementDetails.Where(detail => detail.StatementId == model.StatementId);
                        Db.ServiceStatementDetails.RemoveRange(details);
                        // then update with new details
                        Db.ServiceStatementDetails.AddRange(serviceDetails);
                    }
                    else
                    {
                        return View("ServiceTreatment", new ServiceTreatmentModel()
                        {
                            PatientId = model.PatientId,
                            DoctorId = model.DoctorId,
                            AppointmentId = model.AppointmentId
                        });
                    }

                }
                Db.SaveChanges();
                return RedirectToAction("AppointmentList");
            }
            return View("ServiceTreatment", model);
        }


        #region PRIVATE
        private List<MedicalRecordModel> getMedicalRecords(int patientId)
        {
            return Db.MedicalRecords.Where(rec => rec.PatientId == patientId)
                            .Join(Db.Accounts, rec => rec.PatientId, acc => acc.AccountId, (rec, acc)
                            => new MedicalRecordModel
                            {
                                RecordId = rec.Id,
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

        private IList<AppointmentModel> getAppointments(int doctorId)
        {
            return Db.Appointments.Where(ap => ap.DoctorId == doctorId)
                                .Join(Db.Accounts, ap => ap.PatientId, pt => pt.AccountId, (ap, pt) =>
                                    new AppointmentModel
                                    {
                                        AppointmentId = ap.AppointmentId,
                                        Time = ap.Time,
                                        PatientId = ap.PatientId,
                                        PatientName = pt.Firstname + " " + pt.Lastname
                                    })
                                    .OrderByDescending(ap => ap.Time)
                                    .ToList();
        }
        #endregion
    }
}