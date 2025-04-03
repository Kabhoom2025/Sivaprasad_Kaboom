using Kaboom.Interfaces;
using Kaboom.Models.product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kaboom.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var get = _productService.GetAllProducts();
            return Ok(get);
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not Found" });
            }
            return Ok(product);
        }
       // [Authorize(Roles = "Admin")]
        [HttpPost("Addproduct")]
        public IActionResult AddProduct([FromBody] Products products)
        {
            var createProduct = _productService.AddProduct(products);
            return CreatedAtAction(nameof(GetProductById), new { id = createProduct.Id }, createProduct);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Products products)
        {
            var update = _productService.UpdateProduct(id, products);
            if (update == null)
                return NotFound(new { Message = "Product not Found" });
            return Ok(update);
        }
      //  [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var isDeleted = _productService.DeleteProduct(id);
            if (!isDeleted)
                return NotFound(new { Message = "Product not Found" });
            return Ok(new { Message = $"{isDeleted} Product Deleted Successfully" });
        }
    }
}
