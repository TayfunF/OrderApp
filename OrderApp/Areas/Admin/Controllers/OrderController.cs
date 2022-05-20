using Microsoft.AspNetCore.Mvc;
using OrderApp.Data.Repositories.IRepositories;
using OrderApp.Models.ViewModels;

namespace OrderApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var OrderList = _unitOfWork.OrderProduct.GetAll(x => x.OrderStatus != "Teslim Edildi");

            return View(OrderList);
        }
    }
}
