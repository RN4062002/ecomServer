using ecomServer.DTO.ProductDTO;
using ecomServer.Models;
using ecomServer.Repositories;
using ecomServer.Repositories.Contracts;
using ecomServer.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting; // Added for IWebHostEnvironment
using Microsoft.AspNetCore.Http; // Added for IFormFile

namespace ecomServer.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment; // Injected IWebHostEnvironment

        public ProductServices(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment; // Initialize IWebHostEnvironment
        }
        public async Task<ProductDTO> InsertProduct(ProductDTO productDto)
        {
            try
            {
                if (productDto == null)
                {
                    throw new ArgumentNullException(nameof(productDto), "Product cannot be null");
                }
                var product = new Models.Product
                {
                    ProductName = productDto.Name,
                    ProductDesc = productDto.Description,
                    ProductPrice = productDto.Price,
                    ProductStock = productDto.StockQuantity,
                    CategoryId = productDto.CategoryId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                if (productDto.Images != null && productDto.Images.Any())
                {
                    product.ProductImages = new List<ProductImage>();
                    foreach (var imageFile in productDto.Images)
                    {
                        if (imageFile.Length > 0)
                        {
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            product.ProductImages.Add(new ProductImage
                            {
                                ImageUrl = "/images/" + uniqueFileName,
                                IsPrimary = false
                            });
                        }
                    }
                }

                await _productRepository.AddProductAsync(product);
                return productDto;
            }
            catch (Exception ex)
            {
                // Re-throw to be caught by Controller's catch block
                throw new Exception($"Error in InsertProduct Service: {ex.Message}", ex);
            }
        }
        public async Task<ProductDTO> UpdateProduct(ProductDTO productDto)
        {
            var product = await _productRepository.GetProductByIdAsync(productDto.ProductId);
            if (product == null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product not found");
            }
            product.ProductName = productDto.Name; // Changed from ProductName to Name
            product.ProductDesc = productDto.Description; // Changed from ProductDesc to Description
            product.ProductPrice = productDto.Price; // Changed from ProductPrice to Price
            product.ProductStock = productDto.StockQuantity; // Changed from ProductStock to StockQuantity
            product.CategoryId = productDto.CategoryId;
            product.IsActive = productDto.IsActive;

            // Handle image deletion
            if (productDto.ImageUrlsToDelete != null && productDto.ImageUrlsToDelete.Any())
            {
                foreach (var imageUrl in productDto.ImageUrlsToDelete)
                {
                    var imageToDelete = product.ProductImages.FirstOrDefault(pi => pi.ImageUrl == imageUrl);
                    if (imageToDelete != null)
                    {
                        // Delete the file from wwwroot/images
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(imageUrl));
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                        product.ProductImages.Remove(imageToDelete);
                    }
                }
            }

            // Handle new image uploads
            if (productDto.Images != null && productDto.Images.Any())
            {
                foreach (var imageFile in productDto.Images)
                {
                    if (imageFile.Length > 0)
                    {
                        // Ensure the wwwroot/images directory exists
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        product.ProductImages.Add(new ProductImage
                        {
                            ImageUrl = "/images/" + uniqueFileName, // Store relative URL
                            IsPrimary = false // You can add logic here to determine primary image
                        });
                    }
                }
            }
            
            await _productRepository.UpdateProductAsync(product);
            return productDto;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            // Delete image files
            if (product.ProductImages != null && product.ProductImages.Any())
            {
                foreach (var image in product.ProductImages)
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", Path.GetFileName(image.ImageUrl));
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }

            var result = await _productRepository.DeleteProductAsync(productId);
            return result;
        }

        public async Task<ProductPagedResponseDto> GetAllProducts(int? categoryId, int pageNumber, int pageSize, string? search = null)
        {

            var (products, totalCount) = await _productRepository.GetAllProductsAsync(categoryId, pageNumber, pageSize, search);

            var productDtos = products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                Name = p.ProductName,
                Price = p.ProductPrice,
                Description = p.ProductDesc,
                StockQuantity = p.ProductStock,
                ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).ToList()
            }).ToList();

            return new ProductPagedResponseDto
            {
                Products = productDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<ProductDTO> GetProductById(int ProductId)
        {
            var product = await _productRepository.GetProductByIdAsync(ProductId);
            if (product == null)
            {
                return null; // Or throw an exception
            }

            return new ProductDTO
            {
                ProductId = product.ProductId,
                Name = product.ProductName,
                Description = product.ProductDesc,
                Price = product.ProductPrice,
                StockQuantity = product.ProductStock,
                CategoryId = product.CategoryId,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive,
                ImageUrls = product.ProductImages.Select(pi => pi.ImageUrl).ToList() // Populate ImageUrls
            };
        }

    }
}
