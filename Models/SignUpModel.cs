using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class SignUpModel : LoginModel
    {
        [Required]
        [MinLength(6, ErrorMessage = "Password must has at least 6 characters!")]
        public new string Password { get; set; }

        [Required(ErrorMessage = "First name cannot be empty!")]
        [MaxLength(20, ErrorMessage = "First name must not exceed 20 characters!")]
        public string Firstname { get; set; }
        
        [Required(ErrorMessage = "Last name cannot be empty!")]
        [MaxLength(20, ErrorMessage = "Last name must not exceed 20 characters!")]
        public string Lastname { get; set; }

        public string Phone { get; set; }
    }
}