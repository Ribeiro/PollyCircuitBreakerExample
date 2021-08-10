using Core.Models;
using System;
using System.Threading.Tasks;

namespace PricingApi.Services
{
    public interface IPricingService
    {
        Task<PricingDetails> GetPricingForProductAsync(Guid productId, string currencyCode);
    }

}
