using Kdt.Api.Dtos;
using System.Globalization;

namespace Kdt.Api.Helpers;

public static class ExchangeRatesHelper
{
  public static ExchangeRatesDto<CountryExchangeRateDto> ConvertTxtToJson(string txtResponse, string? dateFilter, string? currencyCodeFilter)
  {
    // Normalize line endings to '\n'
    txtResponse = txtResponse.Replace("\r\n", "\n").Replace("\r", "\n");

    var lines = txtResponse.Split('\n', StringSplitOptions.RemoveEmptyEntries);

    // Validate the input format
    if (lines.Length < 2 || !lines[1].StartsWith("země|měna|množství|kód|kurz"))
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

    return new ExchangeRatesDto<CountryExchangeRateDto>
    {
      Date = date,
      Tag = tag,
      ExchangeRates = exchangeRates
    };
  }
}
