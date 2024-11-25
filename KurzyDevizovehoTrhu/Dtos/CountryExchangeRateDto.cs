using System.Text.Json.Serialization;

namespace Kdt.Api.Dtos;

public class CountryExchangeRateDto
{
    [JsonPropertyName("Země")]
    public string Country { get; set; }

    [JsonPropertyName("Měna")]
    public string Currency { get; set; }

    [JsonPropertyName("Množství")]
    public int Amount { get; set; }

    [JsonPropertyName("Kód")]
    public string Code { get; set; }

    [JsonPropertyName("Kurz")]
    public double ExchangeRate { get; set; }
}

