using Microsoft.AspNetCore.Mvc;

namespace AIECommerce.Web.Components
{
	public class ProductFilterMenuViewComponent: ViewComponent
	{
		public IViewComponentResult Invoke() {
			return View(); 
		}
	}
}
