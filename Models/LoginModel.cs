using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; } = "";
    }
}