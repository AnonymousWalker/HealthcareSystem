using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }
        public DateTime Time { get; set; }
        public KeyValuePair<int,string> Doctor { get; set; }
        public int PatientId { get; set; }
    }
}