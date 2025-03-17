using AIECommerce.Web.Model;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;

namespace AIECommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index([FromQuery] ProductRequestParameters p)
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
        public IActionResult Create()
        {
            ViewBag.Categories = GetCategoriesSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDtoForInsertion productDto, IFormFile? file)
        {
            if (file == null && string.IsNullOrWhiteSpace(productDto.ImageUrl))
            {
                ModelState.AddModelError("File", "Lütfen bir dosya yükleyin veya bir URL girin.");
            }

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Dosyayı kaydet
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Base URL ekleyerek ImageUrl oluştur
                    string baseUrl = $"{Request.Scheme}://{Request.Host}";
                    productDto.ImageUrl = $"{baseUrl}/images/{uniqueFileName}";
                }

                // Ürünü veritabanına kaydet
                _manager.ProductService.CreateProduct(productDto);
                return RedirectToAction("Index");
            }

            return View(productDto);
        }
        private SelectList GetCategoriesSelectList()
        {
            return new SelectList(_manager.CategoryService.GetAllCategories(false), "CategoryId", "CategoryName", "1");
        }

        public IActionResult Update([FromRoute(Name = "id")] int id)
        {
            ViewBag.Categories = GetCategoriesSelectList();
            var model = _manager.ProductService.GetOneProductForUpdate(id, false);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] ProductDtoForUpdate productDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // file operation
                string path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "images", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                productDto.ImageUrl = String.Concat("/images/", file.FileName);
                _manager.ProductService.UpdateOneProduct(productDto);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete([FromRoute(Name = "id")] int id)
        {
            _manager.ProductService.DeleteOneProduct(id);
            return RedirectToAction("Index");
        }
    }
}
