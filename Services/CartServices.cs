using ecomServer.Models;
using ecomServer.Repositories.Contracts;
using ecomServer.Services.Contracts;
using System.Threading.Tasks;

namespace ecomServer.Services
{
    public class CartServices : ICartServices
    {
        private readonly ICartRepository _cartRepository;

        public CartServices(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<Cart> AddToCartAsync(int userId, int productId, int quantity)
        {
            return await _cartRepository.AddToCartAsync(userId, productId, quantity);
        }

        public async Task<Cart> RemoveFromCartAsync(int userId, int cartItemId)
        {
            return await _cartRepository.RemoveFromCartAsync(userId, cartItemId);
        }

        public async Task<Cart> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity)
        {
            return await _cartRepository.UpdateCartItemQuantityAsync(userId, cartItemId, quantity);
        }
    }
}
