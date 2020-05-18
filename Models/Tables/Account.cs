using HealthcareSystem.Models.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    [Table("Account", Schema = "dbo")]
    public abstract class Account
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
    }

    public class PatientAccount : Account
    {
        public string InsuranceNumber { get; set; }
        public string BillingAddress { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }

    public class EmployeeAccount : Account
    {
        public string SSN { get; set; }
        public double Salary { get; set; }
        public EmployeeRole Role { get; set; }
    }
}