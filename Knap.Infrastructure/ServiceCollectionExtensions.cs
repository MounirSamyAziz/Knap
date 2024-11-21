using Knap.Application.Contracts.Infrastructure;
using Knap.Application.Contracts.Interfaces;
using Knap.Application.Converter;
using Knap.Infrastructure.ApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace Knap.Infrastructure
{
    /// <summary>
    /// Provides extension methods to register infrastructure services into the dependency injection (DI) container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the <see cref="CoinMarketCapApiWrapper"/> as an implementation of <see cref="ICurrencyApiClient"/> in the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="apiUrl">The base URL for the CoinMarketCap API.</param>
        /// <param name="apiKey">The API key for authenticating with the CoinMarketCap API.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddCoinMarketCapApiWrapper(this IServiceCollection services, string apiUrl, string apiKey)
        {
            // Register a named HttpClient with BaseAddress and DefaultRequestHeaders
            services.AddHttpClient("CoinMarketCapClient", client =>
            {
                client.BaseAddress = new Uri(apiUrl); // Set the base URL
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey); // Add API key header
                client.DefaultRequestHeaders.Add("Accept", "application/json"); // Set default Accept header
            });
            // Register the CoinMarketCapApiWrapper service
            services.AddTransient<ICurrencyApiClient, CoinMarketCapApiWrapper>();
            return services;
        }

        /// <summary>
        /// Registers the <see cref="ExchangeRatesApi"/> as an implementation of <see cref="ICurrencyApiClient"/> in the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="apiUrl">The base URL for the ExchangeRates API.</param>
        /// <param name="apiKey">The API key for authenticating with the ExchangeRates API.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddExchangeRatesApi(this IServiceCollection services, string apiUrl, string accessKey)
        {
            // Register a named HttpClient with BaseAddress and DefaultRequestHeaders
            services.AddHttpClient("ExchangeRatesClient", client =>
            {
                client.BaseAddress = new Uri(apiUrl); // Set the base URL
            });
            // Register the ExchangeRatesApi service
            services.AddTransient<ICurrencyApiClient>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>(); // Resolve IHttpClientFactory
                var changeBaseRate = sp.GetRequiredService<IChangeBaseRate>(); // Resolve IChangeBaseRate
                return new ExchangeRatesApi(httpClient,  accessKey: accessKey, changeBaseRate);
            });

            return services;
        }

        /// <summary>
        /// Registers the <see cref="ChangeBaseRate"/> implementation of <see cref="IChangeBaseRate"/> in the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddChangeBaseRate(this IServiceCollection services)
        {
            // Register the ChangeBaseRate service
            services.AddTransient<IChangeBaseRate, ChangeBaseRate>();

            return services;
        }
    }
}
