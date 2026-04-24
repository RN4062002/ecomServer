using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ecomServer.DTO.ProductDTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public int StockQuantity { get; set; }
        public int Quentity { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<string>? ImageUrls { get; set; } // Made nullable
        public List<string>? ImageUrlsToDelete { get; set; } // Made nullable
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
