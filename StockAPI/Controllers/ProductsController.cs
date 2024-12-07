using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;

namespace StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StockDbContext _db;

        public ProductsController(StockDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _db.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            _db.Products.Add(product);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
   
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        
            var existingProduct = _db.Products.Find(id);

           
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.UnitsInStock = product.UnitsInStock;
            existingProduct.CategoryId = product.CategoryId;
            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            _db.SaveChanges();
            return NoContent();
        }
    }

}
