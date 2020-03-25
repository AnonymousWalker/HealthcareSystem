using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; } 
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }
}