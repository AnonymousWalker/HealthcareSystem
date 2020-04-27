using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class ReportModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = "";
        public double Revenue { get; set; } = 0;
    }
}