using Data.Repository.Abstract;
using StockManagement.Data;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Concrete
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private readonly StockDbContext _db;

        public ProductRepository(StockDbContext db):base(db) 
        {
            _db = db;
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
        }

    
    }
}
