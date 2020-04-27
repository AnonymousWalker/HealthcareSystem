using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models.Tables
{
    [Table("Transaction", Schema = "dbo")]
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int StatementId { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentNumber { get; set; } = "";
        public string PayerName { get; set; } = "";
        public string BillingAddress { get; set; } = "";
    }
}