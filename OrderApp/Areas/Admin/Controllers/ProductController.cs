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
    }
}
