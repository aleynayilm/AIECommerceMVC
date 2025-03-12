using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
        [Route("api/products")]
        [ApiController]
        public class ProductsController : ControllerBase
        {
            private readonly IServiceManager _manager;

            public ProductsController(IServiceManager manager)
            {
                _manager = manager;
            }

            [HttpGet]
            public IActionResult GetAllProducts()
            {
                return Ok(_manager.ProductService.GetAllProducts(false));
            }
        }
}
