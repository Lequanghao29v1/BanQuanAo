using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ontap_Net104_319.Models;

namespace Ontap_Net104_319.Controllers
{
    public class ProductController : Controller
    {
        AppDbContext _context;
        public ProductController()
        {
            _context = new AppDbContext();
        }
        public IActionResult Index()
        {
            var data = _context.Products.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(Guid id)
        {
            var editItem = _context.Products.Find(id);
            return View(editItem);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var editItem = _context.Products.Find(product.Id);  
            editItem.Name = product.Name;
            editItem.Description = product.Description;
            _context.Products.Update(editItem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            var editItem = _context.Products.Find(id);
            _context.Remove(editItem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult AddtoCart(Guid id, int quantity)
        {
            //Kiểm tra dữ liệu đăng nhập
            var check = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(check))
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var product = _context.Products.Find(id);
                //Kiểm tra xem giỏ hàng của user đã có sản phẩm hay chưa?
                var cartItem = _context.CartDetails.FirstOrDefault(p => p.ProductId == id && p.Username == check);
                if (cartItem == null)
                {
                    CartDetails cartDetails = new CartDetails()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = id,
                        Quantity = quantity,
                        Status = 1,
                        Username = check
                    };
                    _context.CartDetails.Add(cartDetails);
                    product.Amount -= quantity;
                    _context.SaveChanges();
                }
                else //Nếu có rồi thì sẽ cộng dồn(chưa check)
                {

                    if (quantity <= product.Amount - cartItem.Quantity)
                    {
                        
                        cartItem.Quantity = cartItem.Quantity + quantity; //Cập nhật số lượng
                        _context.CartDetails.Update(cartItem);
                        product.Amount -= quantity;
                        _context.SaveChanges();
                    }
                    else
                    {
                        // Trả về thông báo lỗi nếu số lượng thêm vào giỏ hàng lớn hơn số lượng tồn kho còn lại
                        ModelState.AddModelError("quantity", "Số lượng thêm vào giỏ hàng vượt quá số lượng tồn kho.");
                        return View(); // Trả về view hiện tại để hiển thị thông báo lỗi
                    }
                }
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
