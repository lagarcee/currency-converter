using CurrencyConverter;
using CurrencyConverter.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

var converter = new CurrencyService();

Console.WriteLine("Конвертер валют");
Console.Write("Из (USD/EUR/RUB): ");
string from = Console.ReadLine()!.ToUpper();

Console.Write("В (USD/EUR/RUB): ");
string to = Console.ReadLine()!.ToUpper();

Console.Write("Сумма: ");
decimal amount = decimal.Parse(Console.ReadLine()!);

decimal result = await converter.ConvertCurrency(from, to, amount);

Console.WriteLine($"Результат: {amount} {from} = {result:0.00} {to}");
Console.ReadLine();