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


        public ActionResult MakeAppointment()
        {
            Dictionary<int, AppointmentModel> appointmentViewModels = getAppointmentsByDate();
            return View(appointmentViewModels);
        }

        [HttpGet]
        public ActionResult MakeAppointment(int patientId, int doctorId, DateTime time)
        {
            createAppointment(patientId, doctorId, time);
            return View("Success");
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

        public ActionResult MedicalRecord(int patientId = 0)
        {
            if (patientId != 0)
            {
                var model = getMedicalRecords(patientId);
                return View(model);
            }
            return View();
        }

        public ActionResult EnterMedicalRecord(MedicalRecordModel model)
        {
            //nurse update patient's medical record after health check
            Db.MedicalRecords.Add(new MedicalRecord
            {
                Weight = model.Weight,
                Height = model.Height,
                BloodPressure = model.BloodPressure,
                Pulse = model.Pulse,
                Date = DateTime.Now,
                Description = model.Description,
                PatientId = model.PatientId
            });

            Db.SaveChanges();
            return null;
        }

        public ActionResult EditMedicalRecord(MedicalRecordModel model)
        {
            //nurse update patient's medical record after health check
            var record = Db.MedicalRecords.Where(rec => rec.PatientId == model.Id && rec.Date == model.Date).FirstOrDefault();
            if (record != null)
            {
                record.Weight = model.Weight;
                record.Height = model.Height;
                record.BloodPressure = model.BloodPressure;
                record.Pulse = model.Pulse;
                record.Description = model.Description;
                Db.SaveChanges();
            }

            return null;
        }
        //doctor create treatment, prescription record => charge amount
        //public ActionResult createStatement





        #region PRIVATE

        private Dictionary<int, AppointmentModel> getAppointmentsByDate(DateTime? date = null)
        {
            if (!date.HasValue)
            {
                date = DateTime.Today;
            }
            var appointmentViewModels = new Dictionary<int, AppointmentModel>();
            var appointments = Db.Appointments
                .Where(ap => ap.Time.Date == date)
                .Join(Db.Accounts, ap => ap.DoctorId, doctor => doctor.AccountId, (ap, doctor) =>
                    new AppointmentModel
                    {
                        AppointmentId = ap.AppointmentId,
                        Time = ap.Time,
                        Doctor = new KeyValuePair<int, string>(doctor.AccountId, doctor.Firstname),
                        PatientId = ap.PatientId
                    }).ToList();

            for (int i = 9; i <= 16; i++)
            {
                foreach (var ap in appointments)
                {
                    //appointment has already been made
                    if (ap.Time.Hour == i)
                    {
                        appointmentViewModels.Add(i, ap);
                    }
                    else
                    {
                        appointmentViewModels.Add(i, new AppointmentModel());
                    }
                }
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
                                      Doctor = new KeyValuePair<int, string>(doctor.AccountId, doctor.Firstname),
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