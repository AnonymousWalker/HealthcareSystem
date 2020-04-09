﻿using HealthcareSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HealthcareSystem.Controllers
{
    public class HomeController : Controller
    {
        private HealthcareSystemContext Db;

        public HomeController()
        {
            Db = new HealthcareSystemContext();
        }

        public ActionResult Index()
        {
            if (AccountController.IsLoggedIn)
            {
                var id = Convert.ToInt32(Session["AccountId"]);
            }
            return View("~/Views/Account/Login.cshtml");
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

        public IList<AppointmentModel> PatientAppointments(int patientId)
        {
            return getPatientAppointments(patientId);
        }



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
        #endregion
    }
}