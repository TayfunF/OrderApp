using Microsoft.AspNetCore.Mvc;
using OrderApp.Data.Repositories.IRepositories;
using OrderApp.Models;

namespace OrderApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var ProductList = _unitOfWork.Product.GetAll();

            return View(ProductList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _unitOfWork.Product.Add(product);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost]
        public IActionResult Edit(Product Product)
        {
            _unitOfWork.Product.Update(Product);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(Product);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
