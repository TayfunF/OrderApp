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
        public IActionResult CreateOrUpdate(ProductCategoryListVM ProductCategoryListVM, IFormFile UploadedImage)
        {
            string WwwRootPath = _webHostEnvironment.WebRootPath;

            if (UploadedImage != null)
            {
                string FileName = Guid.NewGuid().ToString(); //Benzersiz Resim Adi Icin
                var UploadRoot = Path.Combine(WwwRootPath, @"img\products");
                var Extension = Path.GetExtension(UploadedImage.FileName);

                if (ProductCategoryListVM.Product.Picture != null)
                {
                    var OldPicPath = Path.Combine(WwwRootPath, ProductCategoryListVM.Product.Picture);
                    if (System.IO.File.Exists(OldPicPath))
                    {
                        System.IO.File.Delete(OldPicPath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(UploadRoot, FileName + Extension), FileMode.Create))
                {
                    UploadedImage.CopyTo(fileStream);
                }

                ProductCategoryListVM.Product.Picture = @"\img\products\" + FileName + Extension;
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
