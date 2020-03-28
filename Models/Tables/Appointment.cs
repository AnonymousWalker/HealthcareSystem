using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Time { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public virtual PatientAccount PatientAccountAccount { get; set; }
    }
}