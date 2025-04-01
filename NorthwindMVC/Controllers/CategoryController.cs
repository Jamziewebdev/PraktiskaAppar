using Microsoft.AspNetCore.Mvc;
using Northwind.EntityModels;

namespace NorthwindMVC.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
