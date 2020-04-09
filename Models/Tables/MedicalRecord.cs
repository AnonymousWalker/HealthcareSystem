using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models.Tables
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; } 
        public string BloodPressure { get; set; }
        public short Pulse { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public int PatientId { get; set; }
        public virtual PatientAccount PatientAccount { get; set; }
    }
}