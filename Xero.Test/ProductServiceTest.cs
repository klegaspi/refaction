using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Xero.Model;
using Xero.Repository;
using Moq;
using Xero.Service;
using System.Linq;

namespace Xero.Test
{
    [TestClass]
    public class ProductServiceTest
    {
        [TestMethod]
        public void Can_Get_All_Product_Service()
        {
            //Arrange
            List<Product> products = new List<Product>()
                                         {
                                            new Product(){ Id=Guid.NewGuid(), Name="Nike", Description="Nike", Price=34, DeliveryPrice=1 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Adiddas", Description="Adiddas", Price=55, DeliveryPrice=2 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Under Armour", Description="Under Armour", Price=53, DeliveryPrice=4 },
                                            new Product(){ Id=Guid.NewGuid(), Name="Reebok", Description="Reebok", Price=46, DeliveryPrice=5 },
                                         };
            Mock<IRepository<Product>> mock1 = new Mock<IRepository<Product>>();

            //Here we are going to mock repository GetAll method 
            mock1.Setup(m => m.GetAll()).Returns(products);


            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();

            //Here we are going to inject our repository to the property 
            mock.Setup(m => m.ProductRepository).Returns(mock1.Object);

            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductService productService = new ProductService(mock.Object);

            //Act
            var result = productService.GetAllProducts();

            //Assert
            Assert.AreEqual(products, result);
        }

        [TestMethod]
        public void Can_Add_Product_Service()
        {
            //Arrange
            List<Product> products = new List<Product>()
                                         {
                                             new Product(){ Id=Guid.NewGuid(), Name="Nike", Description="Nike", Price=34, DeliveryPrice=1 },
                                             new Product(){ Id=Guid.NewGuid(), Name="Adiddas", Description="Adiddas", Price=55, DeliveryPrice=2 },
                                             new Product(){ Id=Guid.NewGuid(), Name="Under Armour", Description="Under Armour", Price=53, DeliveryPrice=4 },
                                             new Product(){ Id=Guid.NewGuid(), Name="Reebok", Description="Reebok", Price=46, DeliveryPrice=5 },
                                         };
            Mock<IRepository<Product>> mock1 = new Mock<IRepository<Product>>();

            //Here we are going to mock repository GetAll method 
            mock1.Setup(m => m.GetAll()).Returns(products);

            //Here we are going to mock repository Add method
            mock1.Setup(m => m.Add(It.IsAny<Product>())).Returns((Product target) =>
            {
                var original = products.FirstOrDefault(
                    q => q.Id == target.Id);

                products.Add(target);

                return target;
            });

            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();


            //Here we are going to inject our repository to the property 
            //mock.SetupProperty(m => m.ProductRepository).SetReturnsDefault(mock1.Object);
            mock.Setup(m => m.ProductRepository).Returns(mock1.Object);

            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductService productService = new ProductService(mock.Object);


            Guid newProductId = Guid.NewGuid();

            //Act
            productService.AddProduct(new Product
            {
                Id = newProductId,
                Name = "Crocs",
                Description = "Fully molded dual density extra-comfy Croslite material construction offers profound lightweight cushioning.",
                DeliveryPrice = 1,
                Price = 47
            });
            var result = productService.GetAllProducts();
            var newProduct = result.FirstOrDefault(t => t.Id == newProductId);

            //Assert
            Assert.AreEqual(products.Count(), result.Count());
            Assert.AreEqual("Crocs", newProduct.Name);
            Assert.AreEqual("Fully molded dual density extra-comfy Croslite material construction offers profound lightweight cushioning.", newProduct.Description);
        }

        [TestMethod]
        public void Can_Update_Product_Service()
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

            mock1.Setup(m => m.FindById(It.IsAny<Guid>())).Returns((Guid i) => products.Single(x => x.Id == i));

            //Here we are going to mock repository Edit method
            mock1.Setup(m => m.Edit(It.IsAny<Product>())).Returns((Product target) =>
            {
                var original = products.FirstOrDefault(
                    q => q.Id == target.Id);

                original.Id = target.Id;
                original.Name = target.Name;
                original.Description = target.Description;
                original.Price = target.Price;
                original.DeliveryPrice = target.DeliveryPrice;

                return original;
            });


            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();

            //Here we are going to inject our repository to the property 
            mock.Setup(m => m.ProductRepository).Returns(mock1.Object);


            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductService productService = new ProductService(mock.Object);

            //Act
            var updatedProduct = new Product
            {
                Id = productId,
                Name = "Crocs",
                Description = "Fully molded dual density extra-comfy Croslite material construction offers profound lightweight cushioning.",
                DeliveryPrice = 1,
                Price = 47
            };
            productService.EditProduct(productId, updatedProduct);
            var actualProduct = products.FirstOrDefault(t => t.Id == productId);

            //Assert
            Assert.AreEqual(actualProduct.Name, updatedProduct.Name);
            Assert.AreEqual(actualProduct.Description, updatedProduct.Description);
            Assert.AreEqual(actualProduct.Price, updatedProduct.Price);
            Assert.AreEqual(actualProduct.DeliveryPrice, updatedProduct.DeliveryPrice);
        }

        [TestMethod]
        public void Can_Delete_Product_Service()
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

            //Here we are going to mock repository Delete method
            mock1.Setup(m => m.Delete(It.IsAny<Product>())).Callback((Product target) =>
            {
                var original = products.FirstOrDefault(
                    q => q.Id == target.Id);

                products.Remove(target);
            });   
                
            //Now we have our repository ready for property injection
            //Here we are going to mock our IUnitOfWork
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();

            //Here we are going to inject our repository to the property 
            mock.Setup(m => m.ProductRepository).Returns(mock1.Object);

            //Now our UnitOfWork is ready to be injected to the service
            //Here we inject UnitOfWork to constractor of our service
            ProductService productService = new ProductService(mock.Object);

            //Act
            productService.DeleteProduct(productId);
            var result = products.FirstOrDefault(t => t.Id == productId);

            //Assert
            Assert.IsNull(result);
            Assert.IsTrue(products.Count == 3);
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
