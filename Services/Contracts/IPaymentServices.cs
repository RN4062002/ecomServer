using ecomServer.Models;
using System.Threading.Tasks;

namespace ecomServer.Services.Contracts
{
    public interface IPaymentServices
    {
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<Payment> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod);
    }
}
