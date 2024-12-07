using Data.Repository.Abstract;
using Data.Repository.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using System.Text.Json;

namespace StockManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly StockDbContext _db;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IUnitOfWork unitOfWork, StockDbContext db, ILogger<ProductController> logger)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _logger = logger;
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
            _logger.LogInformation("Product create page opened");
            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state: {Errors}", JsonSerializer.Serialize(ModelState));
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Save();
                _logger.LogInformation("Product created: {ProductName}", product.ProductName);
                TempData["success"] = "Product created";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                _logger.LogWarning("Invalid Id for edit: {Id}", id);
                return NotFound();
            }
            
            var product = _unitOfWork.ProductRepository.Get(c => c.ProductId == id);
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
            _logger.LogInformation("Edit page opened: {ProductName}", product.ProductName);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state: {Errors}", JsonSerializer.Serialize(ModelState));
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                _logger.LogInformation("Product edited: {ProductName}", product.ProductName);
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
                _logger.LogWarning("Product not found for delete: {ProductId}", id);
                return NotFound();
            }
            _logger.LogInformation("Product delete page opened: {ProductName}", product.ProductName);
            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product product = _unitOfWork.ProductRepository.Get(c => c.ProductId == id);
            if (product == null)
            {
                _logger.LogError("Invalid Id for delete: {ProductId}", id);
                return NotFound();
            }

            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();
            _logger.LogInformation("Product deleted: {ProductName}", product.ProductName);
            TempData["success"] = "Product deleted";
            return RedirectToAction("Index");



        }

        public IActionResult StockChart()
        {
          
            var productSalesData = _db.Products
                .Select(p => new
                {
                    p.ProductName,
                    p.UnitsInStock
                })
                .ToList();

      
            var categorySalesData = _db.Products
                .GroupBy(p => p.CategoryId)
                .Select(g => new
                {
                    CategoryName = _db.Categories.FirstOrDefault(c => c.CategoryId == g.Key).CategoryName,
                    TotalUnitsInStock = g.Sum(p => p.UnitsInStock)
                })
                .ToList();

       
            ViewBag.ProductStockData = JsonSerializer.Serialize(productSalesData);
            ViewBag.CategoryStockData = JsonSerializer.Serialize(categorySalesData);

            return View();
        }


        public IActionResult LowStockAlert()
        {
            var lowStockProducts = _db.Products
                .Where(p => p.UnitsInStock <= 10)
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

            return View(lowStockProducts);
        }

    }
}
