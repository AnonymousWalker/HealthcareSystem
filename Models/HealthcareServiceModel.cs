using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class HealthcareServiceModel
    {
        public int PatientId { get; set; }
        public string Examination { get; set; } = "";
        public string Scan { get; set; } = "";
        public string Treatment { get; set; } = "";
        public string Prescription { get; set; } = "";
        public DateTime Date { get; set; }
        public double Amount { get; set; } = 0;
        public string Note { get; set; } = "";
    }
}