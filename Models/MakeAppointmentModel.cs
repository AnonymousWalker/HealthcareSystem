using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class MakeAppointmentModel
    {
        public MakeAppointmentModel()
        {
            //var today = DateTime.Now;
            //DateString = string.Format("{0}-{1}-{2}", today.Year, today.Month, today.Day);
        }
        public Dictionary<int, List<AppointmentModel>> AppointmentSchedule { get; set; }
        //public string DateString { get; set; } 
    }
}