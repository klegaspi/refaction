using System;
using System.Net;
using System.Web.Http;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xero.Service;
using Xero.Model;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ApiController
    {
        #region Member Variables

        private readonly IProductService _productService;

        #endregion

        #region Constructors

        public ProductController()
        {
        }

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Methods

        [Route]
        [HttpGet]
        public IHttpActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();

            if (!products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

        [Route]
        [HttpGet]
        public IHttpActionResult SearchProductsByName(string name)
        {
            var products = _productService.SearchProductsByName(name);

            if (!products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProductByProductId(Guid id)
        {
            var product = _productService.SearchProductByProductId(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Route]
        [HttpPost]
        [Authorize]
        public IHttpActionResult CreateProduct(Product product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest("Product model is invalid");

            try
            {
                return Ok(_productService.AddProduct(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        [HttpPut]
        [Authorize]
        public IHttpActionResult UpdateProduct(Guid id, Product product)
        {
            if (product == null || !ModelState.IsValid)
                return BadRequest("Product model is invalid");

            try
            {
                return Ok(_productService.EditProduct(id, product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteProduct(Guid id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
