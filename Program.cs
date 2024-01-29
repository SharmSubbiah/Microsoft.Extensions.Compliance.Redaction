using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using DataProtection;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole(option => option.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    });
builder.Logging.EnableRedaction();
builder.Services.AddRedaction(x =>
{
    x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.SensitiveData));
#pragma warning disable EXTEXP0002
    x.SetHmacRedactor(option =>
    {
        option.Key ="BLAHBLAHBLAHBLAHBLAHBLAHBLAHBLAHBLAHBLAHBLAH";
        option.KeyId = 69;
    }, new DataClassificationSet(DataTaxonomy.PiiData));
});

var app = builder.Build();

app.MapGet("/customer", (ILogger<Program> logger) =>
{
    var customer = new Customer("Sharm", "blah@test.com", DateOnly.Parse("1986-07-09"));

    //logger.LogInformation("Customer retrieved: {Customer}", customer);
    logger.LogCustomerCreated(customer);
    return Results.Ok(customer);
});

app.Run();

public record Customer(
   [SensitiveData] string Name,
    [PiiData]string Email,
    DateOnly DateofBirth)
{
    public Guid Id { get; } = Guid.NewGuid();
}