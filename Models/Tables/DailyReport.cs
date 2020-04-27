using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models.Tables
{
    [Table("DailyReport",Schema = "dbo")]
    public class DailyReport
    {
        public int Id { get; set;}
        public int DoctorId { get; set; }
        public double Revenue { get; set; }
        public DateTime Date { get; set; }
    }
}