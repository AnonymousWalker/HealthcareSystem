using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models.Tables
{
    [Table("ServiceStatementDetail", Schema = "dbo")]
    public class ServiceStatementDetail
    {
        [Key]
        public int Id { get; set; }
        public int StatementId { get; set; }
        public int ServiceId { get; set; }

        public virtual ServiceFee Service { get; set; }
        public virtual ServiceStatement Statement { get; set; } 
    }
}