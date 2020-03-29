using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class SignUpModel : LoginModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
    }
}