using ecomServer.DTO.ProductDTO;
using ecomServer.Models;
namespace ecomServer.Services.Contracts
{
    public interface IProductServices
    {
        public Task<ProductDTO> InsertProduct(ProductDTO product);
        public Task<ProductDTO> UpdateProduct(ProductDTO product);
        public Task<ProductPagedResponseDto> GetAllProducts(int? categoryId, int pageNumber, int pageSize, string? search = null);
        public Task<ProductDTO> GetProductById(int productId);
        public Task<bool> DeleteProduct(int productId);

    }
}
