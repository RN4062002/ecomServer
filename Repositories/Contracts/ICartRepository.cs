using ecomServer.Models;
using System.Threading.Tasks;

namespace ecomServer.Repositories.Contracts
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<Cart> AddToCartAsync(int userId, int productId, int quantity);
        Task<Cart> RemoveFromCartAsync(int userId, int cartItemId);
        Task<Cart> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity);
    }
}
