using Knap.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adds support for controllers and views.
builder.Services.AddControllersWithViews();

// Adds support for making HTTP requests via HttpClient.
builder.Services.AddHttpClient();


// Registers application-specific services.
// Includes configuration for ChangeBaseRate, CoinMarketCap API, and Exchange Rates API.
builder.Services.AddChangeBaseRate()
    .AddCoinMarketCapApiWrapper(builder.Configuration.GetValue<string>("CoinMarketCapApi:ApiUrl"), builder.Configuration.GetValue<string>("CoinMarketCapApi:ApiKey"))
    .AddExchangeRatesApi(builder.Configuration.GetValue<string>("ExchangeRatesApi:ApiUrl"),        builder.Configuration.GetValue<string>("ExchangeRatesApi:ApiKey"));

var app = builder.Build();

// Configure the HTTP request pipeline.
// Use exception handler for error pages in non-development environments.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Enforces strict transport security headers (HSTS) for production environments.
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Serve static files (e.g., CSS, JavaScript, images).
app.UseStaticFiles();

// Configure routing for the application.
app.UseRouting();

// Enable authorization middleware (currently no specific policies are configured).
app.UseAuthorization();

// Configure the default route for the application.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the application.
app.Run();
public partial class Program { }
