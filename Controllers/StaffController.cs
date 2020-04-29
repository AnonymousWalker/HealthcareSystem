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
        private const string ACCESS_DENIED_MSG = "Access Denied. You do not have permission to this content!";
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
            DateTime today = DateTime.Today;
            DateTime nextDay = today.AddDays(1);

            var dailyReports = database.ServiceStatements.Where(st => st.Date >= today && st.Date < nextDay)
                        .GroupBy(st => st.DoctorId).Select(group => new
                        {
                            DoctorId = group.Key,
                            Revenue = group.Sum(st => st.Amount)
                        })
                        .Join(database.Accounts, st => st.DoctorId, acc => acc.AccountId, (st, acc) => new ReportModel
                        {
                            DoctorId = st.DoctorId,
                            Revenue = st.Revenue,
                            DoctorName = acc.Firstname + " " + acc.Lastname
                        }).ToList();
            // Add doctor without any statement generated
            var doctorList = database.Accounts.OfType<EmployeeAccount>().Where(acc => acc.Role == EmployeeRole.Doctor);
            foreach (var doctor in doctorList)
            {
                if (!dailyReports.Any(rp => rp.DoctorId == doctor.AccountId))
                {
                    dailyReports.Add(new ReportModel
                    {
                        DoctorId = doctor.AccountId,
                        DoctorName = doctor.Firstname + " " + doctor.Lastname,
                        Revenue = 0
                    });
                }
            }
            var yesterday = DateTime.Now.AddDays(-1);
            database.DailyReports.AddRange(dailyReports.Select(x => new DailyReport { DoctorId = x.DoctorId, Revenue = x.Revenue, Date = yesterday }));
            database.SaveChanges();
        }

        public static void GenerateMonthlyReport()
        {
            var database = new HealthcareSystemContext();
            DateTime thisMonth = DateTime.Today;
            thisMonth = thisMonth.AddDays(-thisMonth.Day + 1);  //return the first day of month
            DateTime nextMonth = thisMonth.AddMonths(1);

            var monthlyReports = database.ServiceStatements.Where(st => st.Date >= thisMonth && st.Date < nextMonth)
                        .GroupBy(st => st.DoctorId).Select(group => new
                        {
                            DoctorId = group.Key,
                            Revenue = group.Sum(st => st.Amount)
                        })
                        .Join(database.Accounts, st => st.DoctorId, acc => acc.AccountId, (st, acc) => new ReportModel
                        {
                            DoctorId = st.DoctorId,
                            Revenue = st.Revenue,
                            DoctorName = acc.Firstname + " " + acc.Lastname
                        }).ToList();
            // Add doctor without any statement generated
            var doctorList = database.Accounts.OfType<EmployeeAccount>().Where(acc => acc.Role == EmployeeRole.Doctor);
            foreach (var doctor in doctorList)
            {
                if (!monthlyReports.Any(rp => rp.DoctorId == doctor.AccountId))
                {
                    monthlyReports.Add(new ReportModel
                    {
                        DoctorId = doctor.AccountId,
                        DoctorName = doctor.Firstname + " " + doctor.Lastname,
                        Revenue = 0
                    });
                }
            }
            database.MonthlyReports.AddRange(monthlyReports.Select(x => new MonthlyReport { DoctorId = x.DoctorId, Revenue = x.Revenue, Date = DateTime.Now }));
            database.SaveChanges();
        }

        public ActionResult ViewDailyReport()
        {
            //StaffController.GenerateDailyReport(); // test 
            // Authorize Access
            if (!isAuthorizedAccess(EmployeeRole.CEO))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);
            
            var dailyReports = Db.DailyReports.Where(report => report.Date >= yesterday && report.Date < today)
                                        .Join(Db.Accounts, report => report.DoctorId, acc => acc.AccountId, (report, acc) => new ReportModel
                                        {
                                            DoctorId = report.DoctorId,
                                            DoctorName = acc.Firstname + " " + acc.Lastname,
                                            Revenue = report.Revenue
                                        })
                                        .OrderByDescending(report => report.Revenue)
                                        .ToList();
            ViewBag.ReportType = "Daily";
            return View("DailyMonthlyReport", dailyReports);
        }

        public ActionResult ViewMonthlyReport()
        {
            //StaffController.GenerateMonthlyReport(); // test 
            if (!isAuthorizedAccess(EmployeeRole.CEO))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }
            DateTime thisMonth = DateTime.Today;
            thisMonth = thisMonth.AddDays(-thisMonth.Day + 1);  //return the first day of month
            DateTime nextMonth = thisMonth.AddMonths(1);
            var monthlyReports = Db.MonthlyReports.Where(report => report.Date >= thisMonth && report.Date < nextMonth)
                                        .Join(Db.Accounts, report => report.DoctorId, acc => acc.AccountId, (report, acc) => new ReportModel
                                        {
                                            DoctorId = report.DoctorId,
                                            DoctorName = acc.Firstname + " " + acc.Lastname,
                                            Revenue = report.Revenue
                                        })
                                        .OrderByDescending(report => report.Revenue)
                                        .ToList();
            ViewBag.ReportType = "Monthly";
            return View("DailyMonthlyReport", monthlyReports);
        }

        public ActionResult InputMedicalRecord(int patientId)
        {
            if (patientId <= 0)
            {
                ViewBag.ErrorMessage = "The record is not found.";
                return View("~/Views/Shared/Error.cshtml");
            }
            if (!isAuthorizedAccess(EmployeeRole.CEO, EmployeeRole.Nurse))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }

            var patient = getPatientAccount(patientId);
            if (patient != null)
            {
                return View(new MedicalRecordModel { PatientId = patientId, PatientName = patient.Firstname + " " + patient.Lastname });
            }
            return View(new MedicalRecordModel());
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
            // model error
            return View(record);
        }

        [HttpGet]   // Doctor/nurse 
        public ActionResult EditMedicalRecord(int recordId)
        {
            if (!isAuthorizedAccess(EmployeeRole.Doctor, EmployeeRole.Nurse))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }
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
            if (!isAuthorizedAccess(EmployeeRole.CEO, EmployeeRole.Doctor, EmployeeRole.Nurse, EmployeeRole.Staff))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }

            int accountId = Convert.ToInt32(Session["AccountId"]);
            var employee = getEmployeeAccount(accountId);
            var model = new SearchPatientModel();
            // "input" or "view" medical record, "make-apt" for making appointment (returning patient)
            model.Action = actionType;
            model.Role = employee.Role;
            model.EmployeeId = employee.AccountId;
            return View(model);
        }

        //AJAX
        [HttpPost]
        public ActionResult SearchPatient(string firstName = "", string phone = "", string lastName = "")
        {
            IList<PatientAccount> patientList = new List<PatientAccount>();
            IQueryable<PatientAccount> resultByFirstName = null;
            IQueryable<PatientAccount> resultByLastName = null;
            IQueryable<PatientAccount> resultByPhone = null;

            if (firstName != "")
            {
                resultByFirstName = Db.Accounts.OfType<PatientAccount>()
                            .Where(p => p.Firstname.Contains(firstName));
            }
            if (lastName != "")
            {
                resultByLastName = Db.Accounts.OfType<PatientAccount>()
                            .Where(p => p.Lastname.Contains(lastName));
            }
            if (phone != "")
            {
                resultByPhone = Db.Accounts.OfType<PatientAccount>()
                            .Where(p => p.Phone.Contains(phone));
                // add result to list if not exist in the list
            }

            if (resultByFirstName != null)
            {
                var result = resultByFirstName.ToList();
                patientList = patientList.Union(result).ToList();
                //patientList.Concat(result.TakeWhile(p => !patientList.Any(list => list.AccountId == p.AccountId)));
            }
            if (resultByLastName != null)
            {
                var result = resultByLastName.ToList();
                patientList = patientList.Union(result).ToList();
                //patientList.Concat(result.TakeWhile(p => !patientList.Any(list => list.AccountId == p.AccountId)));
            }
            if (resultByPhone != null)
            {
                var result = resultByPhone.ToList();
                patientList = patientList.Union(result).ToList();
                //patientList.Concat(result.TakeWhile(p => !patientList.Any(list => list.AccountId == p.AccountId)));
            }

            // Filter result by role: Doctor can only see his/her patient's record
            if (isAuthorizedAccess(EmployeeRole.Doctor))
            {
                int userid = Convert.ToInt32(Session["AccountId"]);
                var patientIds = Db.Appointments.Where(s => s.DoctorId == userid)
                                    .Select(s => s.PatientId);

                patientList = patientList.TakeWhile(p => patientIds.Any(x => x == p.AccountId)).ToList();
            }

            return PartialView("_PatientList", patientList);
        }

        public ActionResult PatientMedicalRecords(int patientId)
        {
            if (!isAuthorizedAccess(EmployeeRole.Doctor, EmployeeRole.Nurse, EmployeeRole.CEO))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }

            var patient = getPatientAccount(patientId);
            if (patient != null)
            {
                var records = getMedicalRecords(patientId);

                return View(new PatientMedicalRecordModel
                {
                    PatientId = patient.AccountId,
                    PatientName = patient.Firstname + " " + patient.Lastname,
                    Records = records
                });
            }
            ViewBag.ErrorMessage = "Patient record not found";
            return View("~/Views/Shared/Error.cshtml");
        }

        //Doctor Appointments
        public ActionResult AppointmentList()
        {
            if (!isAuthorizedAccess(EmployeeRole.Doctor))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }
            var id = Convert.ToInt32(Session["AccountId"]);
            var model = getAppointments(id);
            return View("DoctorAppointmentList", model);
        }

        [HttpGet]
        public ActionResult UpdateServiceTreatment(int appointmentId)
        {
            if (!isAuthorizedAccess(EmployeeRole.Doctor))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }
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

            ViewBag.ErrorMessage = "Appointment not found";
            return View("~/Views/Shared/Error.cshtml");
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

        //AJAX
        public ActionResult EditSalary(int doctorId)
        {
            if (!isAuthorizedAccess(EmployeeRole.CEO))
            {
                ViewBag.ErrorMessage = ACCESS_DENIED_MSG;
                return View("~/Views/Shared/Error.cshtml");
            }
            var doctor = Db.Accounts.OfType<EmployeeAccount>().Where(dr => dr.AccountId == doctorId).FirstOrDefault();
            if (doctor != null)
            {
                return PartialView("_EditSalary", new DoctorSalary
                {
                    DoctorId = doctorId,
                    Salary = doctor.Salary,
                    DoctorName = doctor.Firstname + " " + doctor.Lastname
                });
            }
            return PartialView("_EditSalary", new DoctorSalary());
        }

        //AJAX
        [HttpPost]
        public bool EditSalary(DoctorSalary model)
        {
            if (ModelState.IsValid)
            {
                var doctor = Db.Accounts.OfType<EmployeeAccount>().Where(dr => dr.AccountId == model.DoctorId).FirstOrDefault();
                if (doctor != null && isAuthorizedAccess(EmployeeRole.CEO))
                {
                    doctor.Salary = model.Salary;
                    Db.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        #region PRIVATE
        private bool isAuthorizedAccess(params EmployeeRole[] roles)
        {
            var id = Convert.ToInt32(Session["AccountId"]);
            if (!AccountController.IsLoggedIn || id == 0)
            {
                return false;
            }

            var account = Db.Accounts.OfType<EmployeeAccount>().FirstOrDefault(acc => acc.AccountId == id);
            return roles.Any(r => r == account.Role);
        }

        private PatientAccount getPatientAccount(int patientId)
        {
            return Db.Accounts.OfType<PatientAccount>().FirstOrDefault(acc => acc.AccountId == patientId);
        }

        private EmployeeAccount getEmployeeAccount(int id)
        {
            return Db.Accounts.OfType<EmployeeAccount>().FirstOrDefault(acc => acc.AccountId == id);
        }

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
                    newStatement.Amount += (fee != null) ? fee.Fee : 0;
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