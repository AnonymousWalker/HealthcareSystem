using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class DoctorSalary
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        [Required]
        public double Salary { get; set; }
    }
}