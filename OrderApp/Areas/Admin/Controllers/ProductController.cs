using Microsoft.AspNetCore.Mvc;
using OrderApp.Data.Repositories.IRepositories;
using OrderApp.Models;
using OrderApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var ProductList = _unitOfWork.Product.GetAll();

            return View(ProductList);
        }


        //1 Sayfada Hem Create Hem Update Yapabilmek Icin
        [HttpGet]
        public IActionResult CreateOrUpdate(int? id)
        {
            ProductCategoryListVM ProductCategoryListVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null || id <= 0)
            {
                return View(ProductCategoryListVM);
            }

            ProductCategoryListVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            if (ProductCategoryListVM.Product == null)
            {
                return View(ProductCategoryListVM);
            }

            return View(ProductCategoryListVM);
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(ProductCategoryListVM ProductCategoryListVM, IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString(); //Benzersiz Id
                var uploadRoot = Path.Combine(wwwRootPath, @"img\products");
                var extension = Path.GetExtension(file.FileName);

                if (ProductCategoryListVM.Product.Picture != null)
                {
                    var oldPicPath = Path.Combine(wwwRootPath, ProductCategoryListVM.Product.Picture);
                    if (System.IO.File.Exists(oldPicPath))
                    {
                        System.IO.File.Delete(oldPicPath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploadRoot, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                ProductCategoryListVM.Product.Picture = @"img\products\" + fileName + extension;
            }

            if (ProductCategoryListVM.Product.Id <= 0)
            {
                _unitOfWork.Product.Add(ProductCategoryListVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(ProductCategoryListVM.Product);
            }
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
