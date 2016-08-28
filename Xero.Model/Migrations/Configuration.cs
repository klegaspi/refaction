namespace Xero.Model.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Xero.Model.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Xero.Model.DatabaseContext context)
        {
            var products = new List<Product>()
            {
                new Product()
                {
                    Name = "Nike Kobe 11 EM Low",
                    Description = "Nike Kobe 11 EM Low",
                    Price = 150,
                    DeliveryPrice = 1,
                },
                new Product()
                {
                    Name = "Nike Kyrie 2",
                    Description = "Nike Kyrie 2",
                    Price = 150,
                    DeliveryPrice = 1,
                }
            };
            products.ForEach(s => context.Set<Product>().AddOrUpdate(s));
            context.SaveChanges();

            var productOptions = new List<ProductOption>()
            {
                new ProductOption()
                {
                    Name = "White",
                    Product = products[0],
                    Description = "White Kobe 11 EM Low",                    
                },
                new ProductOption()
                {
                    Name = "Black",
                    Product = products[0],
                    Description = "Black Kobe 11 EM Low",                    
                },
                new ProductOption()
                {
                    Name = "Black",
                    Product = products[1],
                    Description = "Black Nike Kyrie 2",                    
                },
                new ProductOption()
                {
                    Name = "White",
                    Product = products[1],
                    Description = "White Nike Kyrie 2",                    
                }
            };
            
            productOptions.ForEach(s => context.Set<ProductOption>().AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
