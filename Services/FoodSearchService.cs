using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FitnessTracker.Services
{
    public class FoodProduct
    {
        public string Name { get; set; } = "";
        public int Kalorien { get; set; }
        public decimal Protein { get; set; }
        public decimal Kohlenhydrate { get; set; }
        public decimal Fett { get; set; }
        public string Marke { get; set; } = "";
    }

    public class FoodSearchService
    {
        private static readonly HttpClient _client = new()
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        public async Task<List<FoodProduct>> SearchAsync(string query)
        {
            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    var url = $"https://world.openfoodfacts.org/cgi/search.pl" +
                              $"?search_terms={Uri.EscapeDataString(query)}" +
                              $"&search_simple=1&action=process&json=1&page_size=8&lc=de&cc=de";

                    var response = await _client.GetStringAsync(url);
                    return ParseProducts(response);
                }
                catch (HttpRequestException ex) when (ex.Message.Contains("503") || ex.Message.Contains("502"))
                {
                    if (attempt < 2)
                    {
                        Console.WriteLine($"FoodSearch: Server nicht verfuegbar, Versuch {attempt + 2}/3...");
                        await Task.Delay(2000 * (attempt + 1));
                    }
                    else
                    {
                        Console.WriteLine("FoodSearch: Server dauerhaft nicht verfuegbar");
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("FoodSearch: Timeout - Server antwortet nicht");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FoodSearch error: {ex.Message}");
                    break;
                }
            }
            return new List<FoodProduct>();
        }

        private List<FoodProduct> ParseProducts(string response)
        {
            var result = new List<FoodProduct>();
            try
            {
                var json = JsonDocument.Parse(response);
                var products = json.RootElement.GetProperty("products");

                foreach (var p in products.EnumerateArray())
                {
                    try
                    {
                        var name = p.TryGetProperty("product_name_de", out var nd) && nd.GetString() != ""
                            ? nd.GetString()!
                            : p.TryGetProperty("product_name", out var n) ? n.GetString() ?? "" : "";

                        if (string.IsNullOrWhiteSpace(name)) continue;

                        var marke = p.TryGetProperty("brands", out var b) ? b.GetString() ?? "" : "";
                        var nutriments = p.TryGetProperty("nutriments", out var nut) ? nut : default;

                        var kcal = nutriments.ValueKind != JsonValueKind.Undefined &&
                                   nutriments.TryGetProperty("energy-kcal_100g", out var k)
                            ? (int)k.GetDouble() : 0;
                        var protein = nutriments.ValueKind != JsonValueKind.Undefined &&
                                      nutriments.TryGetProperty("proteins_100g", out var pr)
                            ? (decimal)pr.GetDouble() : 0;
                        var carbs = nutriments.ValueKind != JsonValueKind.Undefined &&
                                    nutriments.TryGetProperty("carbohydrates_100g", out var c)
                            ? (decimal)c.GetDouble() : 0;
                        var fat = nutriments.ValueKind != JsonValueKind.Undefined &&
                                  nutriments.TryGetProperty("fat_100g", out var f)
                            ? (decimal)f.GetDouble() : 0;

                        result.Add(new FoodProduct
                        {
                            Name = name,
                            Marke = marke,
                            Kalorien = kcal,
                            Protein = protein,
                            Kohlenhydrate = carbs,
                            Fett = fat
                        });
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FoodSearch parse error: {ex.Message}");
            }
            return result;
        }
    }
}