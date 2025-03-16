using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Repositories;
using Services.Contracts;
    public class ReplicateAIService : RepositoryBase<Product>, IReplicateAIService
{
    private readonly HttpClient _httpClient;
    private readonly RepositoryContext _repositoryContext;
    private readonly IConfiguration _configuration;

    public ReplicateAIService(HttpClient httpClient, IConfiguration configuration, RepositoryContext context) : base(context)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.replicate.com/v1/");
        _configuration = configuration;
        _repositoryContext = context;
    }

    public async Task<string> GenerateProductImageAsync(string productName, string userImageUrl)
    {
        Console.WriteLine("✅ GenerateImage metodu başladı.");

        try
        {
            // Ürün görselini veritabanından al
            var productImageUrl = _repositoryContext.Products
                .Where(p => p.ProductName == productName)
                .Select(p => p.ImageUrl)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(productImageUrl))
            {
                throw new Exception($"Ürün görsel URL'si bulunamadı: {productName}");
            }

            Console.WriteLine($"✅ Ürün görseli bulundu: {productImageUrl}");

            // API isteği için JSON verisi
            var requestData = new
            {
                version = "c871bb9b046607b680449ecbae55fd8c6d945e0a1948644bf2361b3d021d3ff4",
                input = new
                {
                    crop = false,
                    seed = 42,
                    steps = 30,
                    category = "upper_body",
                    force_dc = false,
                    garm_img = productImageUrl,
                    human_img = userImageUrl,
                    mask_only = false,
                    garment_des = productName
                }
            };

            // API anahtarını ayarla
            string apiKey = _configuration["Replicate:ApiKey"] ?? throw new Exception("API anahtarı bulunamadı.");
            
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            _httpClient.DefaultRequestHeaders.Add("Prefer", "wait");

            Console.WriteLine("✅ API çağrısı yapılıyor: https://api.replicate.com/v1/predictions");

            // JSON içeriği oluştur
            string jsonContent = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // POST isteğini gönder
            HttpResponseMessage response = await _httpClient.PostAsync("predictions", content);

            // Yanıtı işle
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"📢 API Yanıt Kodu: {(int)response.StatusCode} {response.StatusCode}");
            Console.WriteLine($"📢 API Yanıtı: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("⚠️ Hata oluştu, detaylar: " + responseContent);
                throw new Exception($"Replicate API hatası: {response.StatusCode}, {responseContent}");
            }

            // API yanıtını parse et
            var result = JsonSerializer.Deserialize<ReplicateResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result?.Output != null)
            {
                Console.WriteLine($"✅ İşlem başarılı, görsel URL: {result.Output}");
                return result.Output;
            }
            else if (result?.Status == "processing")
            {
                Console.WriteLine($"✅ İşlem devam ediyor, ID: {result.Id}");
                return await PollForResult(result.Id);
            }

            throw new Exception("Beklenmeyen API yanıtı: Output alanı bulunamadı.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ GenerateProductImageAsync hatası: {ex.Message}");
            throw;
        }
    }

    private async Task<string> PollForResult(string predictionId)
    {
        Console.WriteLine($"🔄 Sonucu almak için bekleniyor... (ID: {predictionId})");

        string url = $"predictions/{predictionId}";

        for (int i = 0; i < 10; i++) // 10 kez dene
        {
            await Task.Delay(2000); // 2 saniye bekle
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ReplicateResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result?.Output != null)
            {
                Console.WriteLine($"✅ Sonuç alındı: {result.Output}");
                return result.Output;
            }
            else if (result?.Status == "failed")
            {
                throw new Exception("❌ AI işlemi başarısız oldu.");
            }
        }

        throw new Exception("⚠️ AI işlemi çok uzun sürdü.");
    }
}

// API yanıt modeli
public class ReplicateResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public string Output { get; set; }
    [JsonPropertyName("error")]
    public object Error { get; set; }
}