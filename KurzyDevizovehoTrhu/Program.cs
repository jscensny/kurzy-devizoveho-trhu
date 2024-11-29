using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json.Serialization;

using Kdt.Api.Dtos;
using Kdt.Api.Validators;
using Kdt.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add HttpClient to the dependency injection container
builder.Services.AddHttpClient();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/kurzy-devizoveho-trhu", async (
    [FromServices]HttpClient httpClient, 
    [FromServices] IConfiguration config,
    string? date = null, 
    string? currencyCode = null
) =>
{
  // URL of the public API
  var apiUrl = config["PublicApi:CNB:KurzyDevizovehoTrhuUrl"];
  if (string.IsNullOrEmpty(apiUrl))
  {
    return Results.Problem("Czech National Bank API URL is not configured.");
  }


  // Validate the date filter & eventually append 
  if (!string.IsNullOrEmpty(date) && !ExchangeRatesValidator.IsDateValid(date))
  {
    return Results.Problem("Invalid date format. Please use DD.MM.YYYY.", statusCode: 400);
  }
  else
  {
    apiUrl += $"?date={date}";
  }

  // Validate the currency code filter
  if (!string.IsNullOrEmpty(currencyCode) && !ExchangeRatesValidator.IsCurrencyCodeValid(currencyCode))
  {
    return Results.Problem("Invalid currency code. Please use a three-letter code (e.g., USD, EUR).", statusCode: 400);
  }



  try
  {
    // Call the public API
    var response = await httpClient.GetStringAsync(apiUrl);

    // Transform the TXT response into JSON
    var jsonResponse = ExchangeRatesHelper.ConvertTxtToJson(response, date, currencyCode );

    return Results.Json(jsonResponse);
  }
  catch (Exception ex)
  {
    return Results.Problem($"Error occurred: {ex.Message}");
  }
})
.WithName("GetExchangeRates")
.WithOpenApi();

app.Run();
