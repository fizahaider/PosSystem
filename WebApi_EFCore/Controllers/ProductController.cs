using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSSystem2.DTOs.Products;
using POSSystem2.Services;
using POSSystem2.Filters;
namespace POSSystem2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogActionFilter))]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<ProductDto>> GetAll()
        {
            var products = _productService.GetAll();

            return Ok(products);
        }

        [HttpGet("{sku}")]
        public ActionResult<ProductDto> GetBySku(string sku)
        {
            var product = _productService.GetBySku(sku);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<ProductDto> Create(CreateProductDto productDto)
        {
            var product = _productService.Create(productDto);

            if (product == null)
            {
                return Conflict("A product with this SKU already exists.");
            }

            return CreatedAtAction(
                nameof(GetBySku),
                new { sku = product.Sku },
                product);
        }

        [HttpPut("{sku}")]
        public ActionResult<ProductDto> Update(
            string sku,
            UpdateProductDto productDto)
        {
            var product = _productService.Update(sku, productDto);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpDelete("{sku}")]
        public IActionResult Delete(string sku)
        {
            var deleted = _productService.Delete(sku);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

