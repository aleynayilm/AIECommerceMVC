﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIECommerce.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles ="Admin")]
	public class CategoryController:Controller
	{
		public IActionResult Index() { 
		return View();
		}
	}
}
