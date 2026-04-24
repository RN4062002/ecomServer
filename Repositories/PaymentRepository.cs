using ecomServer.Data;
using ecomServer.Models;
using ecomServer.Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace ecomServer.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EcomDbContext _context;

        public PaymentRepository(EcomDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments.FindAsync(paymentId);
        }

        public async Task<Payment> CreatePaymentAsync(int orderId, decimal amount, string paymentMethod)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                PaidAmount = amount,
                PaymentMode = paymentMethod,
                PaidDate = DateTime.UtcNow,
                PaymentStatus = "Completed",
                TransactionId = Guid.NewGuid().ToString()
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
