using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Abstract
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        void Save();
    }
}
