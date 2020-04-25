using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class ServiceTreatmentModel
    {
        public ServiceTreatmentModel()
        {
            Date = DateTime.Today;   
        }
        public int StatementId { get; set; }
        public bool BloodTest { get; set; } = false;
        public bool Xray { get; set; } = false;
        public bool MRI { get; set; } = false;
        public bool Radiology { get; set; } = false;
        public bool LabTest { get; set; } = false;
        public string Prescription { get; set; } = "";
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
    }
}