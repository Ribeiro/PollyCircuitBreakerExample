using System;


namespace ProductApi.Dto
{
    public class ProductResponse
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public string CurrencyCode { get; internal set; }
        public decimal Price { get; internal set; }
    }
}
