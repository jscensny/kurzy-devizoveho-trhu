using System.Text.Json.Serialization;

namespace Kdt.Api.Dtos;

public class CountryExchangeRateDto
{
    [JsonPropertyName("Země")]
    public string Country { get; set; } = string.Empty;

  [JsonPropertyName("Měna")]
    public string Currency { get; set; } = string.Empty;

  [JsonPropertyName("Množství")]
    public int Amount { get; set; } 

    [JsonPropertyName("Kód")]
    public string Code { get; set; } = string.Empty;

  [JsonPropertyName("Kurz")]
    public double ExchangeRate { get; set; }
}

