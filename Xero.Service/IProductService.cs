using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model;

namespace Xero.Service
{
    public interface IProductService : IDisposable
    {
        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> SearchProductsByName(string name);

        Product SearchProductByProductId(Guid id);

        Product AddProduct(Product product);

        Product EditProduct(Guid id, Product product);

        void DeleteProduct(Guid productId);
    }
}
