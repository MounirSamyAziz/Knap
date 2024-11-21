using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.Configuration;
using Knap.Infrastructure;

namespace Knap.Tests.Integration;

public class KnapMvcServerFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddChangeBaseRate()
    .AddCoinMarketCapApiWrapper(context.Configuration.GetValue<string>("CoinMarketCapApi:ApiUrl"), context.Configuration.GetValue<string>("CoinMarketCapApi:ApiKey"))
    .AddExchangeRatesApi(context.Configuration.GetValue<string>("ExchangeRatesApi:ApiUrl"), context.Configuration.GetValue<string>("ExchangeRatesApi:ApiKey"));



        });
    }
}
