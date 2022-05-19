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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category Category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(Category);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(Category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category Category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(Category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(Category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }

            var Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(Category);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
