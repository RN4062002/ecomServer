using ecomServer.Models;
using ecomServer.Repositories.Contracts;
using ecomServer.Services.Contracts;
using System.Threading.Tasks;

namespace ecomServer.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServices(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _paymentRepository.GetPaymentByIdAsync(paymentId);
        }

        public async Task<Payment> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod)
        {
            return await _paymentRepository.CreatePaymentAsync(orderId, amount, paymentMethod);
        }
    }
}
