using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string BillingAddress { get; set; }
        public string Phone { get; set; }
        public string SSN { get; set; }
        public double? Salary { get; set; }
    }
}