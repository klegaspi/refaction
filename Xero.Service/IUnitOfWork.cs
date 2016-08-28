using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model;
using Xero.Repository;

namespace Xero.Service
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> ProductRepository { get; }

        IRepository<ProductOption> ProductOptionRepository { get; }

        int Save();
    }
}
