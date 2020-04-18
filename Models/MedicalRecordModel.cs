using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class MedicalRecordModel
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string BloodPressure { get; set; }
        public short Pulse { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public string LabResult { get; set; }
        public string RadiologyReport { get; set; }
        public string PathologyReport { get; set; }
        public string AllegyInformation { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; }
    }
}