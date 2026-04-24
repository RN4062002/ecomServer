using ecomServer.Models;
using System.Threading.Tasks;

namespace ecomServer.Services.Contracts
{
    public interface ICartServices
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<Cart> AddToCartAsync(int userId, int productId, int quantity);
        Task<Cart> RemoveFromCartAsync(int userId, int cartItemId);
        Task<Cart> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity);
    }
}
