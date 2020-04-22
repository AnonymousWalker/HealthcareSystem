using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class IndexModel
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public EmployeeRole? Role { get; set; }

    }
}