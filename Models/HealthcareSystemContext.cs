using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HealthcareSystem.Models.Tables;

namespace HealthcareSystem.Models
{
    public class HealthcareSystemContext : DbContext
    {
        public HealthcareSystemContext() : base()
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Map<PatientAccount>(m => m.Requires("Type").HasValue("PatientAccount"))
                .Map<EmployeeAccount>(m => m.Requires("Type").HasValue("EmployeeAccount"));
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }
    }
}