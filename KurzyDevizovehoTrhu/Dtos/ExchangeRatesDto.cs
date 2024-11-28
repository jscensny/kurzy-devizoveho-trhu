using System.Text.Json.Serialization;

namespace Kdt.Api.Dtos;

public class ExchangeRatesDto<T>
{
  [JsonPropertyName("Datum")]
  public string Date { get; set; }
  [JsonPropertyName("Tag")]
  public string Tag { get; set; }
  [JsonPropertyName("Směnné kurzy")]
  public List<T> ExchangeRates { get; set; }
}
