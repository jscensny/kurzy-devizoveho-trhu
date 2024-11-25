using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json.Serialization;

using Kdt.Api.Dtos;
using Kdt.Api.Validators;

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
    var jsonResponse = ConvertTxtToJson(response, date, currencyCode );

    return Results.Json(jsonResponse);
  }
  catch (Exception ex)
  {
    return Results.Problem($"Error occurred: {ex.Message}");
  }
})
.WithName("GetExchangeRates")
.WithOpenApi();


static object ConvertTxtToJson(string txtResponse, string? dateFilter, string? currencyCodeFilter)
{
  var lines = txtResponse.Split('\n', StringSplitOptions.RemoveEmptyEntries);

  // Validate the input format
  if (lines.Length < 2 || !lines[1].StartsWith("zemì|mìna|množství|kód|kurz"))
  {
    throw new FormatException("Unexpected format of the input TXT response.");
  }

  // Extract the header line (first line) for Date and Tag
  var headerParts = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
  if (headerParts.Length < 2)
  {
    throw new FormatException("Unexpected format of the header line.");
  }


  var date = headerParts[0];
  var tag = headerParts[1];

  var exchangeRates = new List<CountryExchangeRateDto>();

  // Parse the data rows (starting from the third line)
  for (int i = 2; i < lines.Length; i++)
  {
    var columns = lines[i].Split('|');

    if (columns.Length != 5)
    {
      continue; // Skip malformed lines
    }

    var currencyCode = columns[3].Trim();
    if (!string.IsNullOrEmpty(currencyCodeFilter) && !currencyCode.Equals(currencyCodeFilter, StringComparison.OrdinalIgnoreCase))
    {
      continue; // Skip rows that don't match the currency code filter
    }

    exchangeRates.Add(new CountryExchangeRateDto
    {
      Country = columns[0].Trim(),
      Currency = columns[1].Trim(),
      Amount = int.TryParse(columns[2].Trim(), out var amount) ? amount : 0,
      Code = columns[3].Trim(),
      ExchangeRate = double.TryParse(columns[4].Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var rate) ? rate : 0.0
    });
  }

  return new ExchangeRatesDto
  {
    Date = date,
    Tag = tag,
    ExchangeRates = exchangeRates
  };
}


app.Run();
