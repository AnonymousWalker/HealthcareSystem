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

        //Landing Page for Make appointment
        [HttpGet]
        public ActionResult MakeAppointment(int patientId)
        {
            if (patientId == 0)
            {

                return RedirectToAction("Login", "Account");
            }
            ViewBag.PatientId = Convert.ToInt32(Session["AccountId"]);
            return View();
        }

        //AJAX Pick Date
        [HttpGet]
        public ActionResult GetAvailableAppointmentsByDate(string dateString = "")
        {
            DateTime date;
            if (dateString == "")
            {
                date = DateTime.Today;
            }
            else
            {
                //render appointment table 
                date = DateTime.Parse(dateString);

            }
            Dictionary<int, List<AppointmentModel>> appointmentViewModels = getAvailableAppointments(date);
            return PartialView("_AppointmentTable", appointmentViewModels);
        }

        //AJAX
        [HttpPost]
        public ActionResult MakeAppointment(int patientId, int doctorId, string time)
        {
            var apTime = DateTime.Parse(time);
            createAppointment(patientId, doctorId, apTime);
            Dictionary<int, List<AppointmentModel>> appointmentViewModels = getAvailableAppointments(apTime.Date);
            return PartialView("_AppointmentTable", appointmentViewModels);
        }

        public ActionResult AppointmentList(int patientId)
        {
            if (patientId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            var appointments = getPatientAppointments(patientId);
            return View(appointments);
        }

        [HttpGet]
        public bool CancelAppointment(int appointmentId)
        {
            var ap = Db.Appointments.Find(appointmentId);
            if (ap != null)
            {
                Db.Appointments.Remove(ap);
                Db.SaveChanges();
                return true;
            }
            return false;
        }

        public ActionResult MedicalRecord(int patientId)
        {
            var model = new PatientMedicalRecordModel();
            if (patientId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            var patient = Db.Accounts.FirstOrDefault(acc => acc.AccountId == patientId);
            if (patient != null)
            {
                model.PatientName = patient.Firstname + " " + patient.Lastname;
            }
            else
            {
                model.PatientName = "Unknown Patient";
            }

            model.Records = getMedicalRecords(patientId);
            return View(model);
        }

        public ActionResult ServiceInvoice(int appointmentId)
        {
            var model = new StatementInvoiceModel();
            var statement = Db.ServiceStatements.Where(s => s.AppointmentId == appointmentId).FirstOrDefault();
            if (statement != null)
            {
                var services = Db.ServiceStatementDetails.Where(detail => detail.StatementId == statement.Id)
                                                        .Join(Db.ServiceFees, detail => detail.ServiceId, service => service.ServiceId, (detail, service) =>
                                                            new
                                                            {
                                                                service.ServiceName,
                                                                service.Fee
                                                            })
                                                        .ToList();
                var serviceList = services.Select(s => new KeyValuePair<string, double>(s.ServiceName, s.Fee)).ToList();
                
                model = new StatementInvoiceModel
                {
                    Status = statement.Status,
                    Date = statement.Date,
                    Prescription = statement.Prescription,
                    Services = serviceList,
                    AppointmentId = statement.AppointmentId,
                    PatientId = statement.PatientId,
                    DoctorId = statement.DoctorId,
                    InvoiceId = statement.Id
                };
                foreach (var item in serviceList)
                {
                    model.TotalAmount += item.Value;
                }
            }

            return View("ServiceInvoice", model);
        }


        #region PRIVATE

        private Dictionary<int, List<AppointmentModel>> getAvailableAppointments(DateTime date)
        {
            date = date.Date;   //reset to 12AM
            DateTime nextDay = date.AddDays(1);
            var appointmentViewModels = new Dictionary<int, List<AppointmentModel>>();

            var appointments = Db.Appointments
                .Where(ap => ap.Time >= date && ap.Time < nextDay)
                .Join(Db.Accounts, ap => ap.DoctorId, doctor => doctor.AccountId, (ap, doctor) =>
                    new AppointmentModel
                    {
                        AppointmentId = ap.AppointmentId,
                        Time = ap.Time,
                        DoctorId = ap.DoctorId,
                        DoctorName = doctor.Firstname + " " + doctor.Lastname
                    }).ToList();

            var doctorList = Db.Accounts.OfType<EmployeeAccount>().Where(acc => acc.Role == EmployeeRole.Doctor).ToList();
            var now = DateTime.Now;

            if (doctorList.Count == 0 || date < now.Date)
            {
                return appointmentViewModels;
            }

            List<AppointmentModel> apEachHour;
            for (int i = 9; i <= 16; i++)   //time of each appointment
            {
                apEachHour = new List<AppointmentModel>();
                if (date == now.Date && i <= now.Hour) continue;
                foreach (var doctor in doctorList)
                {
                    //appointment has already existed
                    if (appointments.Any(ap => ap.Time.Hour == i && ap.DoctorId == doctor.AccountId))
                    {
                        apEachHour.Add(new AppointmentModel
                        {
                            AppointmentId = -1,  //not available
                            DoctorId = doctor.AccountId,
                            DoctorName = doctor.Firstname + " " + doctor.Lastname,
                            Time = new DateTime(date.Year, date.Month, date.Day, i, 0, 0)
                        });
                    }
                    else
                    {
                        apEachHour.Add(new AppointmentModel
                        {
                            AppointmentId = 0,   //available
                            DoctorId = doctor.AccountId,
                            DoctorName = doctor.Firstname + " " + doctor.Lastname,
                            Time = new DateTime(date.Year, date.Month, date.Day, i, 0, 0)
                        });
                    }
                }
                appointmentViewModels.Add(i, apEachHour);
            }

            return appointmentViewModels;
        }

        private bool createAppointment(int patientId, int doctorId, DateTime time)
        {
            Db.Appointments.Add(new Appointment
            {
                Time = time,
                DoctorId = doctorId,
                PatientId = patientId
            });
            Db.SaveChanges();
            return true;
        }

        private IList<AppointmentModel> getPatientAppointments(int patientId)
        {
            return Db.Appointments.Where(ap => ap.PatientId == patientId)
                                .Join(Db.Accounts, ap => ap.DoctorId, doctor => doctor.AccountId, (ap, doctor) =>
                                  new AppointmentModel
                                  {
                                      AppointmentId = ap.AppointmentId,
                                      Time = ap.Time,
                                      DoctorId = ap.DoctorId,
                                      DoctorName = doctor.Firstname + " " + doctor.Lastname,
                                      PatientId = ap.PatientId
                                  })
                                  .OrderByDescending(ap => ap.Time)
                                  .ToList();
        }

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