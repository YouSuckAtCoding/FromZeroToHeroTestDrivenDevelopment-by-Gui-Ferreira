using Pricing.Core;
using Pricing.Core.Domain.Exceptions;
using Pricing.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPricingManager, PricingManager>();

var app = builder.Build();

app.MapGet("/", () => "Hello Modafoca");

app.MapPut("PricingTable", async (IPricingManager pricingManager, ApplyPricingRequest request, CancellationToken token) =>
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

}
);

app.Run();


