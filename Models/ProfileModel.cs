﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class ProfileModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public AccountType AccountType { get; set; }
        //Patient
        public string InsuranceNumber { get; set; } = "";
        public string BillingAddress { get; set; } = "";
        public DateTime? DayOfBirth { get; set; }
        
        //Employee
        public string SSN { get; set; } = "";
        public double? Salary { get; set; }
        public EmployeeRole? Role { get; set; }
    }
}