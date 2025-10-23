using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<Product[]>> Get()
        {
            var products = await _productContext.Products.ToArrayAsync();

            if (products.Length == 0)
                return NotFound();

            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _productContext.Products.FindAsync(id);
            
            if (product == null)
                return NotFound();
            
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _productContext.Add(product);
            await _productContext.SaveChangesAsync();
            return CreatedAtAction(nameof(CreateProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct (Guid id, Product updatedProduct)
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
            } catch (DbUpdateConcurrencyException)
            {
                if (!_productContext.Products.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
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
    }
}
