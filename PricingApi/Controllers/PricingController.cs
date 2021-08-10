using Microsoft.AspNetCore.Mvc;
using PricingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PricingApi.Controllers
{
    [Route(template: "api/pricing")]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        // GET api/products/{productId}/currencies/{currencyCode}
        [HttpGet(template: "products/{productId}/currencies/{currencyCode}")]
        public async Task<IActionResult> GetPricingProduct([FromRoute] Guid productId, [FromRoute] string currencyCode)
        {
            try
            {
                var pricingDetails = await _pricingService.GetPricingForProductAsync(productId, currencyCode);
                return Ok(pricingDetails);
            }
            catch
            {

                return StatusCode(503);
            }
        }

        
    }
}
