using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model;

namespace Xero.Service
{
    public interface IProductOptionService : IDisposable
    {
        IEnumerable<ProductOption> GetAllProductOptions(Guid productId);

        ProductOption GetProductOption(Guid productId, Guid productOptionId);

        ProductOption AddProductOption(Guid productId, ProductOption productOption);

        ProductOption UpdateProductOption(Guid productId, Guid id, ProductOption productOption);

        void DeleteProductOption(Guid productId, Guid productOptionId);
    }
}
