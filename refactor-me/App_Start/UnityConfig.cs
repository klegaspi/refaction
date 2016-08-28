using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using Xero.Service;

namespace refactor_me
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
                        
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IProductOptionService, ProductOptionService>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}