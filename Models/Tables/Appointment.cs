using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    [Table("Appointment", Schema = "dbo")]
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Time { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public virtual PatientAccount PatientAccount { get; set; }
        public virtual EmployeeAccount EmployeeAccount { get; set; }
    }
}