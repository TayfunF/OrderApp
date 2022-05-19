using Microsoft.AspNetCore.Mvc;
using OrderApp.Data.Repositories.IRepositories;
using OrderApp.Models;

namespace OrderApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> CategoriesList = _unitOfWork.Category.GetAll(null);

            return View(CategoriesList);
        }
    }
}
