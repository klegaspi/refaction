using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model;
using Xero.Repository;

namespace Xero.Service
{
    public class ProductService : IProductService
    {
        #region Member Variables
        
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public ProductService()
        {
        }
        
        public ProductService(IUnitOfWork unitOfWork)
        {            
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods

        public IEnumerable<Product> GetAllProducts()
        {            
            _unitOfWork.ProductRepository.GetAll();

            return _unitOfWork.ProductRepository.GetAll();
        }

        public IEnumerable<Product> SearchProductsByName(string name)
        {            
            return _unitOfWork.ProductRepository.GetBy(product => product.Name == name);
        }

        public Product SearchProductByProductId(Guid id)
        {            
            return _unitOfWork.ProductRepository.FindById(id);
        }

        public Product AddProduct(Product product)
        {            
            //Add business rules here
            var item = _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Save();

            return item;
        }

        public Product EditProduct(Guid id, Product product)
        { 
            if (id != product.Id) throw new Exception("Product Ids do not match. please check your JSON and URL.");

            var editProduct = _unitOfWork.ProductRepository.FindById(id);

            if (editProduct == null) throw new Exception("Product does not exist");

            editProduct.Id = product.Id;
            editProduct.Name = product.Name;
            editProduct.Description = product.Description;
            editProduct.Price = product.Price;
            editProduct.DeliveryPrice = product.DeliveryPrice;

            //Add business rules here

            var item = _unitOfWork.ProductRepository.Edit(editProduct);
            _unitOfWork.Save();

            return editProduct;
        }

        public void DeleteProduct(Guid productId)
        {            
            var product = _unitOfWork.ProductRepository.FindById(productId);
            
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();            
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        #endregion
    }
}
