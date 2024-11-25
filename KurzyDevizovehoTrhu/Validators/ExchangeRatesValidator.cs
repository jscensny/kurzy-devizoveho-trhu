using System.Globalization;

namespace Kdt.Api.Validators;

public static class ExchangeRatesValidator
{
  public static bool IsDateValid(string date)
  {
    // Ensure the format is DD.MM.YYYY
    var format = "dd.MM.yyyy";
    return DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
  }

  public static bool IsCurrencyCodeValid(string code)
  {
    // Ensure code is 3 letters, e.g. CZK, USD, GBP
    return code.Length == 3 && code.All(char.IsLetter);
  }
}
