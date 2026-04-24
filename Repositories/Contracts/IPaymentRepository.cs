using ecomServer.Models;
using System.Threading.Tasks;

namespace ecomServer.Repositories.Contracts
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<Payment> CreatePaymentAsync(int orderId, decimal amount, string paymentMethod);
    }
}
