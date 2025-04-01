using Microsoft.AspNetCore.Mvc;
using Northwind.EntityModels;
using Northwind.Mvc.Models;

namespace NorthwindMVC.Controllers
{
    [Route("Category")]
    public class CategoryController : Controller
    {
        private readonly NorthwindDatabaseContext db;

        public CategoryController(NorthwindDatabaseContext injectedContext)
        {
            db = injectedContext;
        }

        [Route("{id:int}")]
        public IActionResult Details(int id)
        {
            Category? category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound($"Kunde inte hitta kategori.");
            }
            return View(category);
        }
    }
}
