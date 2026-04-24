using ecomServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ecomServer.Services.Contracts
{
    public interface IOrderServices
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(int userId);
    }
}
