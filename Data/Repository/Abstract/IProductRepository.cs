﻿using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product product);
    }
}
