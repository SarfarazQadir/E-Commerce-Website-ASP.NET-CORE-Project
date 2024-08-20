using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace E_Commerce_Website.Controllers
{
    public class AdminController : Controller
    {
        private maniContext _manicontext ;
        private IWebHostEnvironment _env ;
        public AdminController(maniContext manicontext, IWebHostEnvironment env)
        {
            _manicontext = manicontext;
            _env= env;
        }
        public IActionResult Index()
        {
            var admin = HttpContext.Session.GetString("admin_session");
            if(admin != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }
            
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string adminEmail, string adminPassword)
        {
            var row = _manicontext.tbl_admin.FirstOrDefault(a => a.admin_email == adminEmail);
            if (row != null && row.admin_password == adminPassword)
            {
                HttpContext.Session.SetString("admin_session", row.admin_id.ToString());
                return View("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Username Or Password";
                return View();
            }
            
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("login");
        }
        public IActionResult Profile()
        {
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
				var adm = HttpContext.Session.GetString("admin_session");
				var data = _manicontext.tbl_admin.Where(a => a.admin_id == int.Parse(adm)).ToList();
				return View(data);
			}
			else
			{
				return RedirectToAction("login");
			}

		}
        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            _manicontext.tbl_admin.Update(admin);
            _manicontext.SaveChanges();
            return RedirectToAction("profile");
        }
        [HttpPost]
        public IActionResult ChangeProfileImage(IFormFile admin_image, Admin admin)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "AdminImages", admin_image.FileName);
			using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
			{
				admin_image.CopyTo(fs);
			}
			admin.admin_image = admin_image.FileName;
            _manicontext.tbl_admin.Update(admin);
            _manicontext.SaveChanges();
            return RedirectToAction("profile");
        }
        public IActionResult FetchCustomer()
        {
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
			return View(_manicontext.tbl_customer.ToList());
			
			}
			else
			{
				return RedirectToAction("login");
			}
        }

        public IActionResult DetailCustomer(int id)
        {
            return View(_manicontext.tbl_customer.FirstOrDefault(c => c.customer_id == id));
        }
        public IActionResult UpdateCustomer(int id)
        {
            return View(_manicontext.tbl_customer.Find(id));
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer,IFormFile customer_image)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "CustomerImages", customer_image.FileName);
			using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
			{
				customer_image.CopyTo(fs);
			}
			customer.customer_image = customer_image.FileName;
            _manicontext.tbl_customer.Update(customer);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchCustomer");
        }

        public IActionResult DeleteCustomer(int id)
        {
           var customer = _manicontext.tbl_customer.Find(id);
            _manicontext.tbl_customer.Remove(customer);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchCustomer");
        }

        public IActionResult FetchCategory()
        {
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
			return View(_manicontext.tbl_category.ToList());
			}
			else
			{
				return RedirectToAction("login");
			}
        }

        public IActionResult AddCategory()
        {
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
			    return View();
			}
			else
			{
				return RedirectToAction("login");
			}
        }
        [HttpPost]

        public IActionResult AddCategory(Category category)
        {
            _manicontext.tbl_category.Add(category);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchCategory");

        }

        public IActionResult UpdateCategory(int id) 
        {
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
			    return View(_manicontext.tbl_category.Find(id));
			}
			else
			{
				return RedirectToAction("login");
			}
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category cat) 
        {
            _manicontext.tbl_category.Update(cat);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchCategory");
        }

        public IActionResult DeleteCategory(int id) 
        {
          var category = _manicontext.tbl_category.Find(id);
            _manicontext.tbl_category.Remove(category);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchCategory");
        }

        public IActionResult FetchProduct()
        {
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
			return View(_manicontext.tbl_product.ToList());
			}
			else
			{
				return RedirectToAction("login");
			}
        }
        public IActionResult DetailProduct(int id)
        {
            return View(_manicontext.tbl_product.Include(p => p.Category).FirstOrDefault
                (p=>p.product_id == id));
        }

        private IActionResult View<TProperty>(Func<Expression<Func<Product, TProperty>>, IIncludableQueryable<Product, TProperty>> include)
        {
            throw new NotImplementedException();
        }

        public IActionResult AddProduct()
        {
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
				List<Category> categories = _manicontext.tbl_category.ToList();
				ViewData["category"] = categories;
				return View();
			}
			else
			{
				return RedirectToAction("login");
			}
		}
        [HttpPost]
        public IActionResult AddProduct(Product prd,IFormFile product_image)
        {
            string ImageName = Path.GetFileName(product_image.FileName);
            string ImagePath = Path.Combine(_env.WebRootPath, "ProductImages", ImageName);
            using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
			{
				product_image.CopyTo(fs);
			}
            prd.product_image = ImageName;
            _manicontext.tbl_product.Add(prd);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchProduct");
        }

        public IActionResult UpdateProduct(int id)
        {
            List<Category> categories = _manicontext.tbl_category.ToList();
            ViewData["category"] = categories;

            var product = _manicontext.tbl_product.Find(id);
            ViewBag.SelectedCategoryId = product.category_id;
            return View(product);
        }
        [HttpPost]
		public IActionResult UpdateProduct(IFormFile product_image, Product prd)
		{
			string ImagePath = Path.Combine(_env.WebRootPath, "ProductImages", product_image.FileName);

			using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
			{
				product_image.CopyTo(fs);
			}

			prd.product_image = product_image.FileName;
                
			_manicontext.tbl_product.Update(prd);
			_manicontext.SaveChanges();

			return RedirectToAction("FetchProduct");
		}
		/*public IActionResult UpdateProduct(IFormFile product_image, Product prd)
		{
			string ImagePath = Path.Combine(_env.WebRootPath, "ProductImages", product_image.FileName);
			FileStream fs = new FileStream(ImagePath, FileMode.Create);
			product_image.CopyTo(fs);

			prd.product_image = product_image.FileName;

			_manicontext.tbl_product.Update(prd);
			_manicontext.SaveChanges();

			return RedirectToAction("FetchProduct");
		}*/
		public IActionResult DeleteProduct(int id) 
        {
           var product = _manicontext.tbl_product.Find(id);
            _manicontext.tbl_product.Remove(product);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchProduct");
        }
        public IActionResult FetchFeedback()
        {
            var admin = HttpContext.Session.GetString("admin_session");
            if (admin != null)
            {
                return View(_manicontext.tbl_feedback.ToList());
            }
            else
            {
                return RedirectToAction("login");
            }
        }
        public IActionResult DeleteFeedback(int id)
        {
            var feedback = _manicontext.tbl_feedback.Find(id);
            _manicontext.tbl_feedback.Remove(feedback);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchFeedback");
        }
		public IActionResult FetchCart()
		{
			var admin = HttpContext.Session.GetString("admin_session");
			if (admin != null)
			{
              var cart = _manicontext.tbl_cart.Include(c => c.products).Include(c => c.customers).ToList();
				return View(cart);
			}
			else
			{
				return RedirectToAction("login");
			}
		}
        public IActionResult DeleteCart(int id)
        {
          var cart = _manicontext.tbl_cart.Find(id);
            _manicontext.tbl_cart.Remove(cart);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchCart");
        }
        public IActionResult UpdateCart(int id)
        { 
            return View(_manicontext.tbl_cart.Find(id));
        }
        [HttpPost]
        public IActionResult UpdateCart(int cart_status, Cart cart)
        {
            cart.cart_status = cart_status;
            _manicontext.tbl_cart.Update(cart);
            _manicontext.SaveChanges();
            return RedirectToAction("FetchCart");
        }

    }
}
