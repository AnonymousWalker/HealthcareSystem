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

        public static void GenerateDailyReport()
        {
            var database = new HealthcareSystemContext();
            var dailyReports = database.ServiceStatements.Where(st => st.Date.Date == DateTime.Today)
                        .GroupBy(st => st.DoctorId).Select(group => new
                        {
                            DoctorId = group.Key,
                            Revenue = group.Sum(st => st.DoctorId)
                        })
                        .Join(database.Accounts, st => st.DoctorId, acc => acc.AccountId, (st, acc) => new ReportModel
                        {
                            DoctorId = st.DoctorId,
                            Revenue = st.Revenue,
                            DoctorName = acc.Firstname + " " + acc.Lastname
                        }).ToList();
            
            database.DailyReports.AddRange(dailyReports.Select(x => new DailyReport { DoctorId = x.DoctorId, Revenue = x.Revenue, Date = DateTime.Now }));
            database.SaveChanges();
        }

        public static void GenerateMonthlyReport()
        {
            var database = new HealthcareSystemContext();
            var monthlyReports = database.ServiceStatements.Where(st => st.Date.Month == DateTime.Today.Month)
                        .GroupBy(st => st.DoctorId).Select(group => new
                        {
                            DoctorId = group.Key,
                            Revenue = group.Sum(st => st.DoctorId)
                        })
                        .Join(database.Accounts, st => st.DoctorId, acc => acc.AccountId, (st, acc) => new ReportModel
                        {
                            DoctorId = st.DoctorId,
                            Revenue = st.Revenue,
                            DoctorName = acc.Firstname + " " + acc.Lastname
                        }).ToList();
            database.MonthlyReports.AddRange(monthlyReports.Select(x => new MonthlyReport { DoctorId = x.DoctorId, Revenue = x.Revenue, Date = DateTime.Now }));
        }

        public ActionResult InputMedicalRecord(int patientId)
        {
            var patient = Db.Accounts.Find(patientId);
            return View(new MedicalRecordModel { PatientId = patientId, PatientName = patient.Firstname + " " + patient.Lastname });
        }

        public ActionResult ViewDailyReport()
        {
            DateTime today = DateTime.Today;
            DateTime nextDay = today.AddDays(1);
            var dailyReports = Db.DailyReports.Where(report => report.Date >= today && report.Date < nextDay).OrderByDescending(report => report.Revenue);
            return View("DailyMonthlyReport", dailyReports);
        }

        public ActionResult ViewMonthlyReport()
        {
            DateTime thisMonth = DateTime.Today;
            thisMonth = thisMonth.AddDays(-thisMonth.Day + 1);  //return the first day of month
            DateTime nextMonth = thisMonth.AddMonths(1);
            var monthlyReports = Db.MonthlyReports.Where(report => report.Date >= thisMonth && report.Date < nextMonth).OrderByDescending(report => report.Revenue);
            return View("DailyMonthlyReport", monthlyReports);
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
                    if (medicalRecord != null)
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
            }
            else if (lastName != "")
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
            return View(new PatientMedicalRecordModel
            {
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
                return View("DoctorAppointmentList", model);
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
                return View("ServiceTreatment", new ServiceTreatmentModel()
                {
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    AppointmentId = appointmentId
                });
            }
            // Update
            else if (statement.Status == false && appointment != null)
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
            if (ModelState.IsValid)
            {
                updateServiceTreatment(model);
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

        private void updateServiceTreatment(ServiceTreatmentModel model)
        {
            if (model.StatementId == 0)  // CREATE NEW STATEMENT
            {
                var newStatement = Db.ServiceStatements.Add(new ServiceStatement
                {
                    Date = DateTime.Now,
                    Prescription = model.Prescription,
                    Amount = 0,
                    AppointmentId = model.AppointmentId,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                });

                //update service with fee
                var serviceFees = Db.ServiceFees.Select(x => new { x.ServiceId, x.Fee });
                List<int> serviceIds = new List<int>();
                if (model.BloodTest)
                {
                    serviceIds.Add(1);
                }
                if (model.Xray)
                {
                    serviceIds.Add(2);
                }
                if (model.MRI)
                {
                    serviceIds.Add(3);
                }
                if (model.Radiology)
                {
                    serviceIds.Add(4);
                }
                if (model.LabTest)
                {
                    serviceIds.Add(5);
                }
                
                // create a list for AddRange
                List<ServiceStatementDetail> serviceDetails = new List<ServiceStatementDetail>();
                
                // Add appointment fee as default
                var appointmentFee = serviceFees.FirstOrDefault(s => s.ServiceId == 6);
                newStatement.Amount += (appointmentFee != null) ? appointmentFee.Fee : 0;
                serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = 6 });

                // Add optional services + fee
                foreach (var serviceId in serviceIds)
                {
                    var fee = serviceFees.FirstOrDefault(s => s.ServiceId == serviceId);
                    newStatement.Amount += (fee != null)? fee.Fee : 0;
                    serviceDetails.Add(new ServiceStatementDetail { StatementId = newStatement.Id, ServiceId = serviceId });
                }

                Db.ServiceStatementDetails.AddRange(serviceDetails);
            }
            else // UPDATE OLD STATEMENT
            {
                var oldStatement = Db.ServiceStatements.Where(s => s.Id == model.StatementId).FirstOrDefault();
                if (oldStatement != null)
                {
                    oldStatement.Prescription = model.Prescription;
                    oldStatement.Date = DateTime.Now;
                    oldStatement.Amount = 0;
                    
                    // remove old statement details
                    var details = Db.ServiceStatementDetails.Where(detail => detail.StatementId == model.StatementId);
                    Db.ServiceStatementDetails.RemoveRange(details);

                    //update new services with fee
                    var serviceFees = Db.ServiceFees.Select(x => new { x.ServiceId, x.Fee });
                    List<int> serviceIds = new List<int>();
                    if (model.BloodTest)
                    {
                        serviceIds.Add(1);
                    }
                    if (model.Xray)
                    {
                        serviceIds.Add(2);
                    }
                    if (model.MRI)
                    {
                        serviceIds.Add(3);
                    }
                    if (model.Radiology)
                    {
                        serviceIds.Add(4);
                    }
                    if (model.LabTest)
                    {
                        serviceIds.Add(5);
                    }
                    // create a list for AddRange
                    List<ServiceStatementDetail> serviceDetails = new List<ServiceStatementDetail>();

                    // add appointment fee 
                    var appointmentFee = serviceFees.FirstOrDefault(s => s.ServiceId == 6);
                    oldStatement.Amount += (appointmentFee != null) ? appointmentFee.Fee : 0;
                    serviceDetails.Add(new ServiceStatementDetail { StatementId = oldStatement.Id, ServiceId = 6 });
                    // add optional services + fee
                    foreach (var serviceId in serviceIds)
                    {
                        var fee = serviceFees.FirstOrDefault(s => s.ServiceId == serviceId);
                        oldStatement.Amount += (fee != null) ? fee.Fee : 0;
                        serviceDetails.Add(new ServiceStatementDetail { StatementId = oldStatement.Id, ServiceId = serviceId });
                    }

                    // then update with new details
                    Db.ServiceStatementDetails.AddRange(serviceDetails);
                }
            }
            Db.SaveChanges();
        }
        #endregion
    }
}