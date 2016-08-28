using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model;
using Xero.Repository;

namespace Xero.Service
{
    public class ProductOptionService : IProductOptionService
    {
        #region Member Variables
        
        private readonly IUnitOfWork _unitOfWork;
        
        #endregion

        #region Constructors

        public ProductOptionService()
        {

        }
        
        public ProductOptionService(IUnitOfWork unitOfWork)
        {            
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods

        public IEnumerable<ProductOption> GetAllProductOptions(Guid productId)
        {            
            return _unitOfWork.ProductOptionRepository.GetBy(productOption => productOption.ProductId == productId);
        }

        public ProductOption GetProductOption(Guid productId, Guid productOptionId)
        {            
            return _unitOfWork.ProductOptionRepository.GetBy(productOption => productOption.ProductId == productId && productOption.Id == productOptionId).SingleOrDefault();
        }

        public ProductOption AddProductOption(Guid productId, ProductOption productOption)
        {
            if (productId != productOption.ProductId) throw new Exception("Product Ids do not match. please check your JSON and URL.");

            var product = _unitOfWork.ProductRepository.FindById(productId);

            if (product == null) throw new Exception("Product does not exist.");

            //Add business rules here

            var item = _unitOfWork.ProductOptionRepository.Add(productOption);
            _unitOfWork.Save();
            
            return item;
        }

        public ProductOption UpdateProductOption(Guid productId, Guid id, ProductOption productOption)
        {
            if (productId != productOption.ProductId) throw new Exception("Product Ids do not match. please check your JSON and URL.");
            if (id != productOption.Id) throw new Exception("Product Option Ids do not match. please check your JSON and URL.");

            var product = _unitOfWork.ProductRepository.FindById(productId);

            if (product == null) throw new Exception("Product does not exist.");

            var editOption = _unitOfWork.ProductOptionRepository.GetBy(option => option.Id == id && option.ProductId == productId).SingleOrDefault();

            if (editOption == null) throw new Exception("Product Option does not exist.");

            editOption.Name = productOption.Name;
            editOption.ProductId = productOption.ProductId;
            editOption.Description = productOption.Description;

            //Add business rules here

            _unitOfWork.ProductOptionRepository.Edit(editOption);
            _unitOfWork.Save();

            return productOption;
        }

        public void DeleteProductOption(Guid productId, Guid productOptionId)
        {            
            ProductOption productOption = _unitOfWork.ProductOptionRepository.GetBy(prodOption => prodOption.ProductId == productId && prodOption.Id == productOptionId).SingleOrDefault();
            _unitOfWork.ProductOptionRepository.Delete(productOption);
            _unitOfWork.Save();            
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        #endregion
    }
}
