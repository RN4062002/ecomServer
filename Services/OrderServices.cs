using ecomServer.Models;
using ecomServer.Repositories.Contracts;
using ecomServer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ecomServer.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderServices(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order> CreateOrderAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                throw new System.Exception("Cart is empty");
            }

            var order = await _orderRepository.CreateOrderAsync(userId, cart.CartItems);

            // Clear the cart after creating the order
            foreach (var cartItem in cart.CartItems.ToList())
            {
                await _cartRepository.RemoveFromCartAsync(userId, cartItem.CartItemId);
            }

            return order;
        }
    }
}
