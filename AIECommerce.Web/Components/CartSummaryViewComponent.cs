using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIECommerce.Web.Components
{
    public class CartSummaryViewComponent:ViewComponent
    {
        private readonly Cart _cart;
        public CartSummaryViewComponent(Cart carService)
        {
            _cart = carService;
        }

        public string Invoke()
        {
           return _cart.Lines.Count().ToString();
        }
    }
}
