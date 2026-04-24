using System;

namespace ecomServer.DTO.PaymentDTO
{
    public class PaymentRequestDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // "Credit Card", "PayPal", etc.
        public string CardNumber { get; set; } // Mock: Ends in 1 for success
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
    }
}
