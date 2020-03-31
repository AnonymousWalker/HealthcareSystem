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
        public HealthcareSystemContext() : base("HealthcareSystem")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<HealthcareSystemContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
            .Map<PatientAccount>(m => m.Requires("AccountType").HasValue(1))
            .Map<EmployeeAccount>(m => m.Requires("AccountType").HasValue(2));
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }
    }
}