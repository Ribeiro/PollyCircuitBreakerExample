using Core.Models;
using System;
using System.Threading.Tasks;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Task<Product> GetProductDetails(Guid productId);
    }
}
