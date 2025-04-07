namespace CurrencyConverter.Models;

public class ExchangeRateResponse
{
    public Dictionary<string, decimal> conversion_rates { get; set; }
    public decimal Amount { get; set; }
    public string Base { get; set; }
    public string Date { get; set; }
}

