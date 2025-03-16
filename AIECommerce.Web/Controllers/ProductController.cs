using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using Entities.RequestParameters;
using AIECommerce.Web.Model;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Diagnostics;

namespace AIECommerce.Web.Controllers
{
    public class ProductController:Controller
    {
        private readonly IServiceManager _manager;
        private readonly HttpClient _httpClient;

        public ProductController(IServiceManager manager, HttpClient httpClient)
        {
            _manager = manager;
            _httpClient = httpClient;
        }

        public IActionResult Index(ProductRequestParameters p){

            var products = _manager.ProductService.GetAllProductsWithDetails(p);
            var pagination = new Pagination()
            {
                CurrenPage = p.PageNumber,
                ItemsPerPage = p.PageSize,
                TotalItems = _manager.ProductService.GetAllProducts(false).Count()
            };
            return View(new ProductListViewModel()
            {
                Products = products,
                Pagination = pagination
            });
        }
        public IActionResult Get([FromRoute(Name = "id")]int id)
        {
            var model = _manager.ProductService.GetOneProduct(id, false);
            return View(model);
        }
        // AI ile kıyafet deneme için fotoğraf yükleme ve API çağrısı
        [HttpPost]
        public async Task<IActionResult> GenerateImage([FromForm] IFormFile image, [FromForm] string productName)
        {try
        {
            Console.WriteLine("✅GenerateImage metodu başladı.");
            if (image == null || string.IsNullOrEmpty(productName))
            {
                Console.WriteLine("✅Hata: Eksik parametreler - Resim veya ürün adı boş.");
                return BadRequest(new { message = "Lütfen bir resim ve ürün adı girin." });
            }

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                var byteArray = memoryStream.ToArray();
                Console.WriteLine($"✅Resim boyutu: {byteArray.Length} byte");

                var requestContent = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(byteArray);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                requestContent.Add(imageContent, "image", image.FileName);
                requestContent.Add(new StringContent(productName), "productName");

                // AI API'nizin URL'sini buraya ekleyin
                string aiApiUrl = "https://api.replicate.com/v1/predictions"; 
                Console.WriteLine($"✅AI API çağrısı yapılıyor: {aiApiUrl}");

                var response = await _httpClient.PostAsync(aiApiUrl, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅AI API hatası: {response.StatusCode}");
                    return StatusCode(500, new { message = "AI hizmetine bağlanırken bir hata oluştu." });
                }

                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"✅AI API yanıtı: {responseData}");
                return Ok(new { image_url = responseData });
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"✅GenerateImage metodu hata verdi: {ex.Message}");
        return StatusCode(500, new { message = "Bilinmeyen bir hata oluştu." });
        }
            
        }
    }
}