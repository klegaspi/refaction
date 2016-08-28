using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xero.Model;
using Xero.Service;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionController : ApiController
    {
        #region Member Variables

        private readonly IProductOptionService _productOptionService;

        #endregion

        #region Constructors

        public ProductOptionController()
        {

        }

        public ProductOptionController(IProductOptionService productOptionService)
        {
            _productOptionService = productOptionService;
        }

        #endregion

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            var productOptions = _productOptionService.GetAllProductOptions(productId);

            if (!productOptions.Any())
            {
                return NotFound();
            }

            return Ok(productOptions);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            var productOption = _productOptionService.GetProductOption(productId, id);

            if (productOption == null)
            {
                return NotFound();
            }

            return Ok(productOption);
        }

        [Route("{productId}/options")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            if (option == null || !ModelState.IsValid)
                return BadRequest("Product Option model is invalid.");

            try
            {
                _productOptionService.AddProductOption(productId, option);
                return Created("", option);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        [Authorize]
        public IHttpActionResult UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            if (option == null || !ModelState.IsValid)
                return BadRequest("Product Option model is invalid.");

            try
            {
                return Ok(_productOptionService.UpdateProductOption(productId, id, option));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteOption(Guid productId, Guid id)
        {
            try
            {
                _productOptionService.DeleteProductOption(productId, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}