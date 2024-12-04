using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockManagement.Data;
using StockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace StockManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly StockDbContext _db;
        public ProductController(StockDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            var products = _db.Products
        .Join(_db.Categories, 
              product => product.CategoryId,
              category => category.CategoryId,
              (product, category) => new ProductViewModel
              {
                  ProductId = product.ProductId,
                  ProductName = product.ProductName,
                  UnitsInStock = product.UnitsInStock,
                  CategoryId = product.CategoryId,
                  CategoryName = category.CategoryName
              })
        .ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {

            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                TempData["success"] = "Product created";
                return RedirectToAction("Index");
            }
            
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        public IActionResult Edit(int? id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _db.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            var viewModel = new ProductEditViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitsInStock = product.UnitsInStock,
                CategoryId = product.CategoryId,
                CategoryName = _db.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId)?.CategoryName,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {

            if (ModelState.IsValid)
            {
                _db.Products.Update(product);
                _db.SaveChanges();
                TempData["success"] = "Product edited";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            var product = _db.Products
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDeleteViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitsInStock = p.UnitsInStock,
                    CategoryName = _db.Categories
                        .Where(c => c.CategoryId == p.CategoryId)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["success"] = "Product deleted";
            return RedirectToAction("Index");



        }
    }
}
