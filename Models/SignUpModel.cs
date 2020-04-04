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
        [MaxLength(20)]
        public string Firstname { get; set; }
        
        [MaxLength(20)]
        public string Lastname { get; set; }
        public string Phone { get; set; }
    }
}