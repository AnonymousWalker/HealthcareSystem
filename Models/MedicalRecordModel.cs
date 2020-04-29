using HealthcareSystem.Models.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class MedicalRecordModel
    {
        public int RecordId { get; set; }
        [Required]
        [Range(0, 1000)]
        public double Weight { get; set; }
        [Required]
        [Range(0, 100)]
        public double Height { get; set; }
        [MaxLength(7)]  //(111/111)
        [Required(ErrorMessage = "Blood pressure is required")]
        public string BloodPressure { get; set; }
        [Required]
        [MaxLength(3)] //(111)
        public short Pulse { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public string DateString { get; set; } = "";
        public DateTime Date { get; set; }

        public string LabResult { get; set; }
        public string RadiologyReport { get; set; }
        public string PathologyReport { get; set; }
        public string AllegyInformation { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; } = "";
    }
}