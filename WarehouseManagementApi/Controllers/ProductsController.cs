using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Data;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _productContext;
        public ProductsController(ProductContext productContext)
        {
            _productContext = productContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            return await _productContext.Products
                .Select(product => ProductToDTO(product))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            var product = await _productContext.Products.FindAsync(id);
            
            if (product == null)
                return NotFound();
            
            return ProductToDTO(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDTO)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.QuantityInStock
            };

            _productContext.Add(product);
            await _productContext.SaveChangesAsync();
            return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct (Guid id, ProductDTO updatedProduct)
        {
            if (id != updatedProduct.Id)
                return BadRequest();

            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _productContext.Entry(product).CurrentValues.SetValues(updatedProduct);
            try
            {
                await _productContext.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) when (!ProductExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _productContext.Products.FindAsync(id);
            
            if (product == null)
                return NotFound();

            _productContext.Products.Remove(product);
            await _productContext.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(Guid id) =>
            _productContext.Products.Any(e => e.Id == id);

        private static ProductDTO ProductToDTO(Product product) =>
            new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                QuantityInStock = product.StockQuantity
            };
    }
}
