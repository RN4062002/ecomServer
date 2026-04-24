using ecomServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ecomServer.Repositories.Contracts
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(int userId, IEnumerable<CartItem> cartItems);
    }
}
