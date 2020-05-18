using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthcareSystem.Models
{
    public class PaymentModel
    {
        public int StatementId { get; set; }
        public double Amount { get; set; }
        [Required(ErrorMessage = "Name on card is required")]
        public string CardHolder { get; set; } = "";
        [Required(ErrorMessage = "Card Number is required")]
        public string PaymentNumber { get; set; } = "";
        [Required(ErrorMessage = "Address is required")]
        public string BillingAddress { get; set; } = "";
        [Required(ErrorMessage = "Expiry date is required")]
        public string ExpDate { get; set; } = "";
        [Required(ErrorMessage = "Security code is required")]
        public int CVC { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Card;
    }
}