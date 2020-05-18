using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models.Tables
{
    [Table("ServiceStatement", Schema = "dbo")]
    public class ServiceStatement
    {
        [Key]
        public int Id { get; set; }
        public string Prescription { get; set; } = "";
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }

        public int DoctorId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public virtual PatientAccount PatientAccount { get; set; }
    }
}