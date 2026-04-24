using ecomServer.DTO.ProductDTO;
using ecomServer.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ecomServer.Controllers.ProductController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
      
        public ProductController(IProductServices productServices)
        {
            _productService = productServices;
        }

        [HttpPost]
        [Route("InsertProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> InsertProduct([FromForm]  ProductDTO viewModel)
        {
            try
            {
                return await _productService.InsertProduct(viewModel) != null ? Ok("Product Inserted Successfully") : BadRequest("Failed to Insert Product");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProduct([FromForm] ProductDTO product)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(product);
                if (updatedProduct != null)
                {
                    return Ok(updatedProduct);
                }
                else
                {
                    return NotFound("Product not found or update failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
               bool Results = await _productService.DeleteProduct(id);
                return Results ? Ok("Product Deleted Successfully") : NotFound("Product not found or deletion failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetAllProducts")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductPagedResponseDto>> GetAllProducts([FromQuery] int? categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                var result =  await _productService.GetAllProducts(categoryId, pageNumber, pageSize, search);
                return result != null ? Ok(result) : NotFound("No products found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ProductController>/5
        [HttpGet]
        [Route("GetProduct/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return product != null ? Ok(product) : NotFound("Product not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
