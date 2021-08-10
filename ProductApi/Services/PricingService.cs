using Core.Models;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductApi.Services
{
    public class PricingService : IPricingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly Random Jitterer = new Random();
        private static readonly AsyncRetryPolicy<HttpResponseMessage> TransientErrorRetryPolicy =
            Policy.HandleResult<HttpResponseMessage>(
                message => ((int)message.StatusCode) == 429 || (int)message.StatusCode >= 500)
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => 
                 {
                     Console.WriteLine($"Retrying because of transient error. Attempt {retryAttempt}");
                     return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                          + TimeSpan.FromMilliseconds(Jitterer.Next(0, 50));
                 });

        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy =
            Policy.HandleResult<HttpResponseMessage>(message => (int)message.StatusCode == 503)
            //.AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromMinutes(1), 100, TimeSpan.FromMinutes(1));
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        private readonly AsyncPolicyWrap<HttpResponseMessage> _resilientPolicy =
            CircuitBreakerPolicy.WrapAsync(TransientErrorRetryPolicy);

        public PricingService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PricingDetails> GetPricingForProductAsync(Guid productId, string currencyCode)
        {
            if(CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            {
                throw new Exception(message: "Service is currently unavailable!");
            }

            var httpClient = _httpClientFactory.CreateClient();
            var response = await _resilientPolicy.ExecuteAsync(() =>
                                       httpClient.GetAsync(requestUri: $"https://localhost:44308/api/pricing/products/{productId}/currencies/{currencyCode} ")
                                                              );

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception(message: "Service is currently unavailable!");
            }

            var responseText = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PricingDetails>(responseText, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

    }

}
