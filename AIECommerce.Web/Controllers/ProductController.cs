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
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly HttpClient _httpClient;
        private readonly IReplicateAIService _replicateService;

        public ProductController(IServiceManager manager, HttpClient httpClient, IReplicateAIService replicateService)
        {
            _manager = manager;
            _httpClient = httpClient;
            _replicateService = replicateService;
        }

        public IActionResult Index(ProductRequestParameters p)
        {
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

        public IActionResult Get([FromRoute(Name = "id")] int id)
        {
            var model = _manager.ProductService.GetOneProduct(id, false);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateImage([FromForm] IFormFile image, [FromForm] string productName)
        {
            try
            {
                if (image == null || string.IsNullOrEmpty(productName))
                {
                    return BadRequest(new { message = "Lütfen bir resim ve ürün adı girin." });
                }


                string base64Image;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    var byteArray = memoryStream.ToArray();
                    base64Image = Convert.ToBase64String(byteArray);
                }

                // Base64 formatında URI oluştur
                string dataUri = $"data:image/png;base64,{base64Image}";

                // ReplicateAIService'i kullanarak AI görüntü oluşturma
                string resultImageUrl = await _replicateService.GenerateProductImageAsync(productName, dataUri);

                if (string.IsNullOrEmpty(resultImageUrl))
                {
                    return StatusCode(500, new { message = "AI işlemi başarısız oldu veya boş sonuç döndü." });
                }

                return Ok(new { image_url = resultImageUrl });
            }
            catch (System.Exception ex)
            {
                string errorMsg = ex.InnerException != null ?
                    $"{ex.Message} - {ex.InnerException.Message}" : ex.Message;

                return StatusCode(500, new { message = $"Hata: {errorMsg}" });
            }
        }
    }
}