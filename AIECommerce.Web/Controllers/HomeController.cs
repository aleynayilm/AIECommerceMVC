using Microsoft.AspNetCore.Mvc;

namespace AIECommerce.Web.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index(){
        return View();
        }
    }
}