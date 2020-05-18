using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models.Tables
{
    [Table("ServiceFee", Schema ="dbo")]
    public class ServiceFee
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public double Fee { get; set; }
    }
}