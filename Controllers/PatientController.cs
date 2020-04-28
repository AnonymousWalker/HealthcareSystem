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
            ViewBag.PatientId = patientId;
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

        public ActionResult ServiceInvoice(int invoiceId)
        {
            var model = new StatementInvoiceModel();
            var statement = Db.ServiceStatements.Where(s => s.Id == invoiceId).FirstOrDefault();
            if (statement != null)
            {
                Transaction transaction;
                if (statement.Status) // paid --> get payment info
                {
                    transaction = Db.Transactions.Where(t => t.StatementId == invoiceId).FirstOrDefault();
                    if (transaction != null)
                    {
                        model.Status = true;
                        model.PaymentMethod = transaction.PaymentMethod;
                        model.PaymentNumber = "xxxx" + transaction.PaymentNumber.Substring(transaction.PaymentNumber.Length - 4);
                    }
                }
                var services = Db.ServiceStatementDetails.Where(detail => detail.StatementId == statement.Id)
                                                        .Join(Db.ServiceFees, detail => detail.ServiceId, service => service.ServiceId, (detail, service) =>
                                                            new
                                                            {
                                                                service.ServiceName,
                                                                service.Fee
                                                            })
                                                        .ToList();
                var serviceList = services.Select(s => new KeyValuePair<string, double>(s.ServiceName, s.Fee)).ToList();

                model.Status = statement.Status;
                model.Date = statement.Date;
                model.Prescription = statement.Prescription;
                model.Services = serviceList;
                model.AppointmentId = statement.AppointmentId;
                model.StatementId = statement.Id;
                model.PatientId = statement.PatientId;
                model.DoctorId = statement.DoctorId;
                model.InvoiceId = statement.Id;

                foreach (var item in serviceList)
                {
                    model.TotalAmount += item.Value;
                }
            }
            else
            {
                ViewBag.ErrorMessage = "The doctor has not provided any services!";
                return View("~/Views/Shared/Error.cshtml");
            }

            return View("ServiceInvoice", model);
        }

        //public ActionResult ServiceInvoiceList()
        //{
            
        //}

        public ActionResult Payment(int invoiceId)
        {
            var id = Convert.ToInt32(Session["AccountId"]);
            var statement = Db.ServiceStatements.Find(invoiceId);

            if (statement != null && id != 0)
            {
                var patient = Db.Accounts.OfType<PatientAccount>().Where(p => p.AccountId == id).FirstOrDefault();
                return View(new PaymentModel
                {
                    StatementId = invoiceId,
                    Amount = statement.Amount,
                    BillingAddress = patient.BillingAddress
                });
            }
            else if (statement != null) // no need to login
            {
                return View(new PaymentModel
                {
                    StatementId = invoiceId,
                    Amount = statement.Amount,
                });
            }
            ViewBag.ErrorMessage = "Cannot make a payment at this time. Please try again later!";
            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        public ActionResult Payment(PaymentModel model)
        {
            if (ModelState.IsValid)
            {
                var statement = Db.ServiceStatements.Find(model.StatementId);
                if (statement != null)
                {
                    statement.Status = true;    //paid
                    Db.Transactions.Add(new Transaction
                    {
                        StatementId = model.StatementId,
                        Amount = statement.Amount,
                        BillingAddress = model.BillingAddress,
                        Date = DateTime.Now,
                        PayerName = model.CardHolder,
                        PaymentNumber = model.PaymentNumber,
                        PaymentMethod = PaymentMethod.Card,
                        Status = "Pending"
                    });
                    Db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "This invoice is not available for payment at the moment. Please try again later";
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            return View(model);
        }

        public ActionResult GetInvoiceStatement()
        {
            var id = Convert.ToInt32(Session["AccountId"]);
            
            if (id != 0)
            {
                var model = Db.ServiceStatements.Where(s => s.PatientId == id)
                                .OrderByDescending(s => s.Date)
                                .Select(s => new StatementInvoiceModel {
                                    StatementId = s.Id,
                                    Status = s.Status,
                                    Date = s.Date,
                                    TotalAmount = s.Amount
                                })
                                .ToList();
                return PartialView("_ServiceInvoiceTable", model);
            }
            return PartialView("_ServiceInvoiceTable", new List<StatementInvoiceModel>());
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