using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class PatientMedicalRecordModel
    {
        public PatientMedicalRecordModel()
        {
            Records = new List<MedicalRecordModel>();
        }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public IList<MedicalRecordModel> Records { get; set; }
    }
}