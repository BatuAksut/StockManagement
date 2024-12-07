using Data.Repository.Abstract;
using StockManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private StockDbContext _db;
        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(StockDbContext db)
        {
            _db = db;
            ProductRepository = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
