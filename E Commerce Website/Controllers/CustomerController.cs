using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Website.Controllers
{
    public class CustomerController : Controller
    {
        private maniContext _maniContext;
        private IWebHostEnvironment _env; 
        public CustomerController(maniContext maniContext, IWebHostEnvironment env)
        {
            _maniContext = maniContext;
            _env = env;
        }
        public IActionResult Index()
        {
                List<Category> category = _maniContext.tbl_category.ToList();
                ViewData["category"] = category;

                List<Product> products = _maniContext.tbl_product.ToList();
                ViewData["products"] = products;

            ViewBag.checkSession = HttpContext.Session.GetString("customerSession");
                return View();
        }
        [HttpGet]
        public IActionResult Index(string SearchProduct)
        {
			List<Category> category = _maniContext.tbl_category.ToList();
			ViewData["category"] = category;

			List<Product> prod = new List<Product>();
            if (string.IsNullOrEmpty(SearchProduct))
            {
				List<Product> products = _maniContext.tbl_product.ToList();
				ViewData["products"] = products;
				ViewBag.checkSession = HttpContext.Session.GetString("customerSession");
			}
			else
			{
				prod = _maniContext.tbl_product.FromSqlInterpolated($" SELECT * FROM tbl_product WHERE product_name ={SearchProduct}").ToList();
            }
            return View(prod);
		}
		public IActionResult CustomerLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CustomerLogin(string Customer_Email, string Customer_Password)
        {
          var customer =  _maniContext.tbl_customer.FirstOrDefault(c => c.customer_email == Customer_Email);
            if (customer != null && customer.customer_password == Customer_Password)
            {
                HttpContext.Session.SetString("customerSession",    
                    customer.customer_id.ToString());
                return RedirectToAction("Index");
            }
            else 
            {
                ViewBag.message = "Incorrect Username Or Password";
                return View();
            }

        }
        public IActionResult CustomerRegistration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerRegistration(Customer customer)
        {   
            _maniContext.tbl_customer.Add(customer);
            _maniContext.SaveChanges();
            return RedirectToAction("CustomerLogin");
        }  
        public IActionResult CustomerLogout()
        {
            HttpContext.Session.Remove("customerSession");
            return RedirectToAction("Index");
        }

        public IActionResult CustomerProfile()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("customerSession")))
            {
               return RedirectToAction("CustomerLogin");
            }
            else
            {
                List<Category> customer = _maniContext.tbl_category.ToList();
                ViewData["customer"] = customer;
                var customerId = HttpContext.Session.GetString("customerSession");
                var data = _maniContext.tbl_customer.Where(c => c.customer_id== int.Parse(customerId)).ToList();
                return View(data);
            }
        }

      
        [HttpPost]
        public IActionResult UpdateCustomerProfile(IFormFile customer_image, Customer customer)
        {

            string ImagePath = Path.Combine(_env.WebRootPath,"CustomerImages", customer_image.FileName);
            using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
            {
                customer_image.CopyTo(fs);
            }
            customer.customer_image = customer_image.FileName;
            _maniContext.tbl_customer.Update(customer);
            _maniContext.SaveChanges();
            return RedirectToAction("CustomerProfile");
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer cust)
        {
            _maniContext.tbl_customer.Update(cust);
            _maniContext.SaveChanges();
            return RedirectToAction("CustomerProfile");
        }
        public IActionResult ContactPage()
		{
            List<Category> category = _maniContext.tbl_category.ToList();
            ViewData["category"] = category;
			return View();
		}
		[HttpPost]
		public IActionResult ContactPage(Feedback feedback)
		{
            TempData["message"] = "Thank You For Your Feedback";
			_maniContext.tbl_feedback.Add(feedback);
			_maniContext.SaveChanges();
			return RedirectToAction("ContactPage");
		}
		public IActionResult BlogPage()
		{
            List<Category> category = _maniContext.tbl_category.ToList();
            ViewData["BlogCategory"] = category;
            return View();
		}
		public IActionResult BlogDetail()
		{
			return View();
		}
		public IActionResult AboutPage()
		{
			return View();
		}
		public IActionResult FeaturesPage()
		{
			string CustomerId = HttpContext.Session.GetString("customerSession");
			if (CustomerId != null)
			{
				var cart = _maniContext.tbl_cart.Where(c => c.customer_id == int.Parse(CustomerId)).Include(c => c.products).ToList();
				return View(cart);
			}
			else
			{
				return RedirectToAction("CustomerLogin");
			}
		}
		public IActionResult ShopPage()
		{
            List<Product> product = _maniContext.tbl_product.ToList();
            ViewData["product"] = product;
            return View();
		}
		public IActionResult ProductDetails(int id)
		{
            List<Category> category = _maniContext.tbl_category.ToList();
            ViewData["category"] = category;
			List<Product> product = _maniContext.tbl_product.ToList();
			ViewData["product"] = product;
			var products = _maniContext.tbl_product.Where(p => p.product_id == id).ToList();
			return View(products);
		}
        [HttpPost]
        public IActionResult AddToCart(int product_id, Cart cart)
        {
            string IsLogin = HttpContext.Session.GetString("customerSession");
            if (IsLogin != null)
            {
                cart.product_id = product_id;
                cart.customer_id = int.Parse(IsLogin);
                cart.product_quantity = 1;
                cart.cart_status = 0;
                _maniContext.tbl_cart.Add(cart);
                _maniContext.SaveChanges();
                TempData["message"] = "Product Successfully Added In Cart";
                return RedirectToAction("ShopPage");
            }
            return RedirectToAction("CustomerLogin");
        }
        public IActionResult DeleteCart(int id)
        {
          var cart = _maniContext.tbl_cart.Find(id);
            _maniContext.tbl_cart.Remove(cart);
            _maniContext.SaveChanges();
            return RedirectToAction("ProductDetails");
        }
        public IActionResult ViewCartCustomer()
        {
            string CustomerId = HttpContext.Session.GetString("customerSession");
            if (CustomerId != null)
            {
            var cart = _maniContext.tbl_cart.Where(c => c.customer_id == int.Parse(CustomerId)).Include(c=>c.products).ToList();
            return View(cart);
            }
            else
            {
                return RedirectToAction("CustomerLogin");
            }
        }
        public IActionResult RemoveProduct(int id)
        {
         var cart = _maniContext.tbl_cart.Find(id);
            _maniContext.tbl_cart.Remove(cart);
            _maniContext.SaveChanges();
            return RedirectToAction("ViewCartCustomer");
        }
        public IActionResult CheckoutProduct()
        {
            return View();
        }
    }
}
