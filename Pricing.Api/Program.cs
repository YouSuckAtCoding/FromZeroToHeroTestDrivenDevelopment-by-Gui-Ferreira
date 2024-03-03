using Pricing.Api.TicketPrice;
using Pricing.Core;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain.Exceptions;
using Pricing.Core.TicketPrice;
using Pricing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new NpgsqlConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")!));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddScoped<IPricingStore, PostgresPricingStore>();
builder.Services.AddScoped<IPricingManager, PricingManager>();
builder.Services.AddScoped<ITicketPriceService, TicketPriceService>();
builder.Services.AddScoped<IPriceCalculator, PriceCalculator>();
builder.Services.AddScoped<IReadPricingStore, PostgresReadPricingStore>();

var app = builder.Build();

app.MapGet("/", () => "Hello Modafoca");

app.MapPut("/PricingTable", async (IPricingManager pricingManager, ApplyPricingRequest request, CancellationToken token) =>
{ 

    try
    {
        var result = await pricingManager.HandleAsync(request, token);
        return result ? Results.Ok() : Results.BadRequest();
    }
    catch (InvalidPricingTierException)
    {
        return Results.Problem();
    }

});

app.MapGet("/TicketPrice", TicketPriceEndpoint.HandleAsync);


await InitializeDatabase(app);

app.Run();
return;
Task InitializeDatabase(WebApplication app) =>
    app.Services.GetService<DatabaseInitializer>()?.InitializeAsync() ?? Task.CompletedTask;





