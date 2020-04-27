using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class StatementInvoiceModel : ServiceTreatmentModel
    {
        public StatementInvoiceModel()
        {

        }
        public int InvoiceId { get; set; }
        public IList<KeyValuePair<string, double>> Services { get; set; }
        public double TotalAmount { get; set; } = 0;
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentNumber { get; set; } = "";
    }
}