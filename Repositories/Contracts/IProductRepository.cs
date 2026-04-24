using ecomServer.Models;

namespace ecomServer.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetAllProductsAsync(int? categoryId, int pageNumber, int pageSize, string? search = null);
    }
}
