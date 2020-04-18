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
        public ActionResult MakeAppointment(string dateString = "")
        {
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

        [HttpPost]
        public ActionResult MakeAppointment(int patientId, int doctorId, string time)
        {
            var apTime = DateTime.Parse(time);
            createAppointment(patientId, doctorId, apTime);
            Dictionary<int, List<AppointmentModel>> appointmentViewModels = getAvailableAppointments(apTime.Date);
            return PartialView("_AppointmentTable", appointmentViewModels);
        }

        [HttpGet]
        public IList<AppointmentModel> PatientAppointments(int patientId)
        {
            return getPatientAppointments(patientId);
        }

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
            var records = Db.MedicalRecords.Where(rec => rec.PatientId == patientId)
                            .OrderByDescending(rec => rec.Date).ToList();
            return View(new MedicalRecordModel { 
                
            });
        }




        #region PRIVATE

        private Dictionary<int, List<AppointmentModel>> getAvailableAppointments(DateTime date)
        {
            date = date.Date;
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
                        DoctorName = doctor.Firstname + " " + doctor.Lastname,
                        PatientId = ap.PatientId
                    }).ToList();

            var doctorList = Db.Accounts.OfType<EmployeeAccount>().Where(acc => acc.Role == EmployeeRole.Doctor).ToList();
            if (doctorList.Count == 0)
            {
                return appointmentViewModels;
            }
            List<AppointmentModel> apEachHour;
            for (int i = 9; i <= 16; i++)   //time of each appointment
            {
                apEachHour = new List<AppointmentModel>();
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
                                  }).ToList();
        }

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