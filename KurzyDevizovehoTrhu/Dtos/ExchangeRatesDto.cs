using System.Text.Json.Serialization;

namespace Kdt.Api.Dtos;

public class ExchangeRatesDto<T>
{
  [JsonPropertyName("Datum")]
  public string Date { get; set; } = string.Empty;
  [JsonPropertyName("Tag")]
  public string Tag { get; set; } = string.Empty;
  [JsonPropertyName("Směnné kurzy")]
  public List<T> ExchangeRates { get; set; } = default!;
}
