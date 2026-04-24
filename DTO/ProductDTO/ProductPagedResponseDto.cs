using System.Collections.Generic;

namespace ecomServer.DTO.ProductDTO
{
    public class ProductPagedResponseDto
    {
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
