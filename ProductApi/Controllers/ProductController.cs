using Microsoft.AspNetCore.Mvc;
using ProductApi.Dto;
using ProductApi.Services;
using System;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [Route(template:"api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IPricingService _pricingService;

        public ProductController(IProductService productService, IPricingService pricingService)
        {
            _productService = productService;
            _pricingService = pricingService;
        }

        [HttpGet(template:"products/{productId}/currencies/{currencyCode}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid productId, [FromRoute] string currencyCode)
        {
            var product = await _productService.GetProductDetails(productId);

            if(null == product)
            {
                return NotFound();
            }

            var pricing = await _pricingService.GetPricingForProductAsync(productId, currencyCode);
            return Ok(new ProductResponse 
            {
                Id = productId,
                Name = product.Name,
                Price = pricing.Price,
                CurrencyCode = pricing.CurrencyCode
            });

        }

    }

}
