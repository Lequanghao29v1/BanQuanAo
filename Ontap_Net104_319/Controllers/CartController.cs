using Microsoft.AspNetCore.Mvc;
using Ontap_Net104_319.Models;

namespace Ontap_Net104_319.Controllers
{
    public class CartController: Controller
    {
        AppDbContext _context;
        public CartController()
        {
            _context = new AppDbContext();
        }
        public IActionResult Index() // Hiển thị tất cả danh sách các sản phẩm có trong giỏ hàng của 1 user
        {
            //Chekc xem đăng nhập chưa 
            var check = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(check)) // chưa đăng nhập =>  bắt đăng nhập
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var data = _context.CartDetails.ToList();
                foreach (var item in data)
                {
                    var product = _context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        item.Product.Name = product.Name; // Lấy tên sản phẩm từ bảng Products và gán vào ProductName trong CartDetails
                    }
                }
                return View(data);
            }
        }
    }
}
