using Core.Models;
using System;
using System.Threading.Tasks;

namespace PricingApi.Services
{
    public class PricingService : IPricingService
    {
        private DateTime _recoveryTime = DateTime.Now;
        private static readonly Random Random = new Random();

        public Task<PricingDetails> GetPricingForProductAsync(Guid productId, string currencyCode)
        {
            if(_recoveryTime > DateTime.UtcNow)
            {
                throw new Exception(message: "Something went wrong!");
            }

            if (_recoveryTime < DateTime.UtcNow && Random.Next(1,4) == 1)
            {
                _recoveryTime = DateTime.UtcNow.AddSeconds(30);
            }

            return Task.FromResult(new PricingDetails() 
            {   ProductId = productId, 
                CurrencyCode = currencyCode, 
                Price = 10.99m
            });

        }

    }

}
