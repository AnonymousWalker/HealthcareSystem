using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models.Tables
{
    [Table("ServiceStatement", Schema = "dbo")]
    public class ServiceStatement
    {
        public int Id { get; set; }
        public string Examination { get; set; } = "";
        public string Scan { get; set; } = "";
        public string Treatment { get; set; } = "";
        public string Prescription { get; set; } = "";
        public DateTime Date { get; set; }
        public double Amount { get; set; }

        public int PatientId { get; set; }
        public virtual PatientAccount PatientAccount { get; set; }
    }
}