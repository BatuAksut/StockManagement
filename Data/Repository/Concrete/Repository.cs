using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly StockDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(StockDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            _db.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }


        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return dbSet.ToList();
        }
    }
}
