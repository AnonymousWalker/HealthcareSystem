using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class SearchPatientModel
    {
        public int EmployeeId { get; set; }
        public EmployeeRole Role { get; set; }
        public string Action { get; set; } = "";
    }
}