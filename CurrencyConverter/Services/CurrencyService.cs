using System.Text.Json;
using CurrencyConverter.Models;
using DotNetEnv;

namespace CurrencyConverter.Services;

public class CurrencyService
{
    public async Task<decimal> ConvertCurrency(string from, string to, decimal amount)
    {
        Env.Load();
        string token = Env.GetString("TOKEN");
        string apiUrl = $"https://v6.exchangerate-api.com/v6/{token}/latest/{from}";
        using HttpClient client = new HttpClient();

        try
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string json = await response.Content.ReadAsStringAsync();

            // Логируем полученный JSON для диагностики
            Console.WriteLine($"DEBUG: API Response: {json}");

            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<ExchangeRateResponse>(json);

            if (result?.conversion_rates == null || result.conversion_rates.Count == 0)
            {
                Console.WriteLine("Ошибка: API вернуло пустые курсы валют");
                return -1;
            }
            
    
            if (!result.conversion_rates.ContainsKey(to))
            {
                Console.WriteLine(
                    $"Ошибка: валюта '{to}' не найдена. Доступные валюты: {string.Join(", ", result.conversion_rates.Keys)}");
                return -1;
            }

            return result.conversion_rates[to] * amount;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка при запросе к API: {ex.Message}");
            return -1;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка при десериализации JSON: {ex.Message}");
            return -1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            return -1;
        }
    }
}