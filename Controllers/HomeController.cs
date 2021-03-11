using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Take_home_test.Models;
using Take_home_test.Service;

namespace Take_home_test.Controllers
{
	public class HomeController : Controller
	{
		private readonly IStringMatchService _stringMatchService;

		public HomeController(IStringMatchService stringMatchService)
		{
			_stringMatchService = stringMatchService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public ActionResult InsertData()
		{
			var reuslt = _stringMatchService.InsertData();
			return Json(reuslt);
		}

		[HttpPost]
		public ActionResult Search(RequestSearch requestSearch)
		{
			var result = _stringMatchService.Search(requestSearch.SearchValue, requestSearch.PageIndex, requestSearch.PageSize);
			return PartialView("PagingStringMatch", result);
		}
	}
}
