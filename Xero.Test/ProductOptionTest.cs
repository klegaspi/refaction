using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Xero.Model;
using Xero.Repository;
using Moq;
using Xero.Service;
using System.Linq;
using System.Linq.Expressions;

namespace Xero.Test
{
    [TestClass]
    public class ProductOptionServiceTest
    {
        private Guid _productId1 = Guid.NewGuid();
        private Guid _productId2 = Guid.NewGuid();
        private Guid _productOptionId1 = Guid.NewGuid();
        [TestMethod]
        public void Can_Get_All_Product_Option_Service()
        {
            //Arrange
            List<ProductOption> productOptions = new List<ProductOption>()
                                         {
                                            new ProductOption(){ Id=_productOptionId1, Name="Option Name 1", Description="Option Desc 1", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 2", Description="Option Desc 2", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 3", Description="Option Desc 3", ProductId=_productId2 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 4", Description="Option Desc 4", ProductId=_productId2 },
                                         };
            Mock<IRepository<ProductOption>> mock1 = new Mock<IRepository<ProductOption>>();

            mock1.Setup(r => r.GetBy(It.IsAny<Expression<Func<ProductOption, bool>>>()))
                .Returns((Expression<Func<ProductOption, bool>> ppp) =>
                productOptions.Where(p=>p.ProductId == _productId1).AsEnumerable<ProductOption>() );
                
            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();

            //Here we are going to inject our repository to the property 
            mock.Setup(m => m.ProductOptionRepository).Returns(mock1.Object);

            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductOptionService productOptionService = new ProductOptionService(mock.Object);

            //Act
            var result = productOptionService.GetAllProductOptions(_productId1);

            var a = productOptions.Where(p => p.ProductId == _productId1).AsEnumerable<ProductOption>();
            //Assert
            Assert.AreEqual(productOptions.Where(p => p.ProductId == _productId1).Count(), result.Count());
        }

        [TestMethod]
        public void Can_Add_Product_Option_Service()
        {
            //Arrange
            List<ProductOption> productOptions = new List<ProductOption>()
                                         {
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 1", Description="Option Desc 1", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 2", Description="Option Desc 2", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 3", Description="Option Desc 3", ProductId=_productId2 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 4", Description="Option Desc 4", ProductId=_productId2 },
                                         };

            List<Product> products = new List<Product>()
                                         {
                                            new Product(){ Id=_productId1, Name="Nike", Description="Nike", Price=34, DeliveryPrice=1 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Adiddas", Description="Adiddas", Price=55, DeliveryPrice=2 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Under Armour", Description="Under Armour", Price=53, DeliveryPrice=4 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Reebok", Description="Reebok", Price=46, DeliveryPrice=5 },
                                         };

            Mock<IRepository<ProductOption>> mock1 = new Mock<IRepository<ProductOption>>();
            Mock<IRepository<Product>> mockProduct = new Mock<IRepository<Product>>();

            //Here we are going to mock repository GetAll method 
            mock1.Setup(r => r.GetBy(It.IsAny<Expression<Func<ProductOption, bool>>>()))
                .Returns((Expression<Func<ProductOption, bool>> ppp) =>
                productOptions.Where(p => p.ProductId == _productId1).AsEnumerable<ProductOption>());


            mockProduct.Setup(m => m.FindById(It.IsAny<Guid>())).Returns((Guid i) => products.Single(x => x.Id == _productId1));

            //Here we are going to mock repository Add method
            mock1.Setup(m => m.Add(It.IsAny<ProductOption>())).Returns((ProductOption target) =>
            {
                var original = productOptions.FirstOrDefault(
                    q => q.Id == target.Id);

                productOptions.Add(target);

                return target;
            });

            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();


            //Here we are going to inject our repository to the property 
            //mock.SetupProperty(m => m.ProductRepository).SetReturnsDefault(mock1.Object);
            mock.Setup(m => m.ProductOptionRepository).Returns(mock1.Object);
            mock.Setup(m => m.ProductRepository).Returns(mockProduct.Object);

            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductOptionService productOptionService = new ProductOptionService(mock.Object);


            Guid newProductOptionId = Guid.NewGuid();

            //Act
            productOptionService.AddProductOption(_productId1, new ProductOption()
            {
                Id = newProductOptionId,
                Name="Option Name 5",
                Description="Option Desc 5",
                ProductId = _productId1
            });
            var result = productOptionService.GetAllProductOptions(_productId1);
            var newProduct = result.FirstOrDefault(t => t.Id == newProductOptionId && t.ProductId == _productId1);

            //Assert
            Assert.AreEqual(productOptions.Where(p=>p.ProductId == _productId1).Count(), result.Count());
            Assert.AreEqual("Option Name 5", newProduct.Name);
            Assert.AreEqual("Option Desc 5", newProduct.Description);
        }

        [TestMethod]
        public void Can_Update_Product_Option_Service()
        {
            //Arrange
            List<ProductOption> productOptions = new List<ProductOption>()
                                         {
                                            new ProductOption(){ Id=_productOptionId1, Name="Option Name 1", Description="Option Desc 1", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 2", Description="Option Desc 2", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 3", Description="Option Desc 3", ProductId=_productId2 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 4", Description="Option Desc 4", ProductId=_productId2 },
                                         };

            List<Product> products = new List<Product>()
                                         {
                                            new Product(){ Id=_productId1, Name="Nike", Description="Nike", Price=34, DeliveryPrice=1 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Adiddas", Description="Adiddas", Price=55, DeliveryPrice=2 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Under Armour", Description="Under Armour", Price=53, DeliveryPrice=4 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Reebok", Description="Reebok", Price=46, DeliveryPrice=5 },
                                         };

            Mock<IRepository<ProductOption>> mock1 = new Mock<IRepository<ProductOption>>();
            Mock<IRepository<Product>> mock2 = new Mock<IRepository<Product>>();

            mock1.Setup(r => r.GetBy(It.IsAny<Expression<Func<ProductOption, bool>>>()))
                .Returns((Expression<Func<ProductOption, bool>> ppp) =>
                productOptions.Where(p => p.ProductId == _productId1 && p.Id == _productOptionId1).AsEnumerable<ProductOption>());


            mock2.Setup(m => m.FindById(It.IsAny<Guid>())).Returns((Guid i) => products.Single(x => x.Id == i));

            //Here we are going to mock repository Edit method
            mock1.Setup(m => m.Edit(It.IsAny<ProductOption>())).Returns((ProductOption target) =>
            {
                var original = productOptions.FirstOrDefault(
                    q => q.Id == target.Id);

                original.Id = target.Id;
                original.ProductId = target.ProductId;
                original.Description = target.Description;
                original.Name = target.Name;

                return original;
            });


            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();

            //Here we are going to inject our repository to the property 
            mock.Setup(m => m.ProductOptionRepository).Returns(mock1.Object);
            mock.Setup(m => m.ProductRepository).Returns(mock2.Object);

            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductOptionService productOptionService = new ProductOptionService(mock.Object);

            //Act
            var updatedProductOption = new ProductOption
            {
                Id = _productOptionId1,
                ProductId = _productId1,
                Name = "Option Name 6",
                Description = "Option Desc 6"
            };
            productOptionService.UpdateProductOption(_productId1, _productOptionId1, updatedProductOption);
            var actualProduct = productOptions.FirstOrDefault(t => t.Id == _productOptionId1 && t.ProductId == _productId1);

            //Assert
            Assert.AreEqual(actualProduct.Name, updatedProductOption.Name);
            Assert.AreEqual(actualProduct.Description, updatedProductOption.Description);
        }

        [TestMethod]
        public void Can_Delete_Product_Option_Service()
        {
            //Arrange
            List<ProductOption> productOptions = new List<ProductOption>()
                                         {
                                            new ProductOption(){ Id=_productOptionId1, Name="Option Name 1", Description="Option Desc 1", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 2", Description="Option Desc 2", ProductId=_productId1 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 3", Description="Option Desc 3", ProductId=_productId2 },
                                            new ProductOption(){ Id=Guid.NewGuid(), Name="Option Name 4", Description="Option Desc 4", ProductId=_productId2 },
                                         };
            Mock<IRepository<ProductOption>> mock1 = new Mock<IRepository<ProductOption>>();

            //Here we are going to mock repository FindById Method
            mock1.Setup(r => r.GetBy(It.IsAny<Expression<Func<ProductOption, bool>>>()))
                .Returns((Expression<Func<ProductOption, bool>> ppp) =>
                productOptions.Where(p => p.ProductId == _productId1 && p.Id == _productOptionId1).AsEnumerable<ProductOption>());

            //Here we are going to mock repository Delete method
            mock1.Setup(m => m.Delete(It.IsAny<ProductOption>())).Callback((ProductOption target) =>
            {
                var original = productOptions.FirstOrDefault(
                    q => q.Id == target.Id);

                productOptions.Remove(target);
            });

            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();

            //Here we are going to inject our repository to the property 
            mock.Setup(m => m.ProductOptionRepository).Returns(mock1.Object);

            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductOptionService productOptionService = new ProductOptionService(mock.Object);

            //Act
            productOptionService.DeleteProductOption(_productOptionId1, _productId1);
            var result = productOptions.FirstOrDefault(t => t.Id == _productOptionId1 && t.ProductId == _productId1);

            //Assert
            Assert.IsNull(result);
            Assert.IsTrue(productOptions.Count == 3);
        }

        [TestMethod]
        public void Can_Find_By_Id()
        {
            Guid productId = Guid.NewGuid();
            //Arrange
            List<Product> products = new List<Product>()
                                         {
                                            new Product(){ Id=productId, Name="Nike", Description="Nike", Price=34, DeliveryPrice=1 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Adiddas", Description="Adiddas", Price=55, DeliveryPrice=2 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Under Armour", Description="Under Armour", Price=53, DeliveryPrice=4 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Reebok", Description="Reebok", Price=46, DeliveryPrice=5 },
                                         };
            Mock<IRepository<Product>> mock1 = new Mock<IRepository<Product>>();

            //Here we are going to mock repository FindById Method
            mock1.Setup(m => m.FindById(It.IsAny<Guid>())).Returns((Guid i) => products.Single(x => x.Id == i));

            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();

            //Here we are going to inject our repository to the property 
            mock.Setup(m => m.ProductRepository).Returns(mock1.Object);


            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductService productService = new ProductService(mock.Object);

            //Act
            var result = productService.SearchProductByProductId(productId);
            var expected = products.Single(t => t.Id == productId);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}
