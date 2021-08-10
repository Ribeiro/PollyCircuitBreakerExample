using Core.Models;
using System;
using System.Threading.Tasks;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        public Task<Product> GetProductDetails(Guid productId)
        {

            return Task.FromResult(new Product()
            {
                Id = productId,
                Name = "Apple Macbook Pro M1"
            });

        }

    }

}
