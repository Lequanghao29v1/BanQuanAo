using Microsoft.AspNetCore.Mvc;
using Ontap_Net104_319.Models;

namespace Ontap_Net104_319.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext context; // Khai báo context và tạo constructor
        public AccountController()
        {
            context = new AppDbContext(); // khỏi tạo context
        }
        public IActionResult Login(string username, string password)
        {
            if (username == null && password == null)
            {
                return View();
            }
            else
            {
                // Kiểm tra xem tài khoản hay mật khẩu có chính xác không?
                var data = context.Accounts.FirstOrDefault(p => p.Username == username && p.Password == password);
                if (data == null) // trong trường hợp không tìm thấy dữ liệu Account tương ứng
                {
                    return Content("Đăng nhập thất bại mời kiểm tra lại");
                }
                else // trong trường hợp thành công sẽ trả về trang chủ
                {
                    HttpContext.Session.SetString("username", username); // Lưu dữ liệu login vào Session
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        public IActionResult SignUp() // Action này đơn thuần để mở View cần thực hiện
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(Account account) // Action này thực hiện việc tạo ra tài khoản mới
        {
            try
            {
                context.Accounts.Add(account);
                // Tạo mới đồng thời 1 giỏ hàng
                Cart cart = new Cart()
                {
                    Username = account.Username,     
                    Status = 1
                };
                context.Carts.Add(cart);
                context.SaveChanges(); // Lưu thay đổi
                TempData["Status"] = "Tạo tài khoản thành công";
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("username"); // Xóa dữ liệu của username đã login
            return RedirectToAction("Index", "Home");
        }
    }
}
