using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using Kdt.Api;
using Kdt.Api.Helpers;
using Assert = Xunit.Assert;

namespace Kdt.Api.UnitTests;

public class GetExchangeRatesTests_ConvertTxtToJson
{
  [Fact]
  public void ConvertTxtToJson_ValidInput_ReturnsExpectedResult()
  {
    // Arrange
    string txtResponse =
      @"01.11.2023 #211
země|měna|množství|kód|kurz
USA|dolar|1|USD|23,443
EMU|euro|1|EUR|24,685";

    string dateFilter = "01.11.2023";
    string? currencyCodeFilter = null;

    // Act
    var result = ExchangeRatesHelper.ConvertTxtToJson(txtResponse, dateFilter, currencyCodeFilter);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("01.11.2023", result.Date);
    Assert.Equal("#211", result.Tag);
    Assert.NotNull(result.ExchangeRates);
    Assert.Equal(2, result.ExchangeRates.Count);

    Assert.Equal("USA", result.ExchangeRates[0].Country);
    Assert.Equal("dolar", result.ExchangeRates[0].Currency);
    Assert.Equal(1, result.ExchangeRates[0].Amount);
    Assert.Equal("USD", result.ExchangeRates[0].Code);
    Assert.Equal(23.443, result.ExchangeRates[0].ExchangeRate);

    Assert.Equal("EMU", result.ExchangeRates[1].Country);
    Assert.Equal("euro", result.ExchangeRates[1].Currency);
    Assert.Equal(1, result.ExchangeRates[1].Amount);
    Assert.Equal("EUR", result.ExchangeRates[1].Code);
    Assert.Equal(24.685, result.ExchangeRates[1].ExchangeRate);
  }

  [Fact]
  public void ConvertTxtToJson_InvalidFormat_ThrowsFormatException()
  {
    // Arrange
    string txtResponse =
      @"01.11.2023 #211
      InvalidHeaderLine
      USA|Dolar|1|USD|24.50";

    string dateFilter = "01.11.2023";
    string? currencyCodeFilter = null;

    // Act & Assert
    Assert.Throws<FormatException>(() => ExchangeRatesHelper.ConvertTxtToJson(txtResponse, dateFilter, currencyCodeFilter));
  }

  [Fact]
  public void ConvertTxtToJson_FilteredByCurrencyCode_ReturnsFilteredResult()
  {
    // Arrange
    string txtResponse =
      @"01.11.2023 ValidData
země|měna|množství|kód|kurz
USA|dolar|1|USD|24.50
Eurozóna|Euro|1|EUR|26.30";

    string dateFilter = "01.11.2023";
    string currencyCodeFilter = "USD";

    // Act
    var result = ExchangeRatesHelper.ConvertTxtToJson(txtResponse, dateFilter, currencyCodeFilter);

    // Assert
    Assert.NotNull(result);
    Assert.Single(result.ExchangeRates); // Only USD should remain
    Assert.Equal("USA", result.ExchangeRates[0].Country);
    Assert.Equal("dolar", result.ExchangeRates[0].Currency);
    Assert.Equal("USD", result.ExchangeRates[0].Code);
  }

  [Fact]
  public void ConvertTxtToJson_EmptyInput_ThrowsFormatException()
  {
    // Arrange
    string txtResponse = "";
    string? dateFilter = null;
    string? currencyCodeFilter = null;

    // Act & Assert
    Assert.Throws<FormatException>(() => ExchangeRatesHelper.ConvertTxtToJson(txtResponse, dateFilter, currencyCodeFilter));
  }
}
