using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Website.Models
{
	public class Order
	{
		[Key]
		public int order_id { get; set; }
		public int cust_id { get; set; }
		public int prod_id { get; set; }

		public int quantity { get; set; }
		public int total_price { get; set; }
		public string order_status { get; set; }  // Pending, Shipped, Delivered, etc.
		public string payment_status { get; set; } // Pending, Completed, Failed
		public DateTime order_date { get; set; }

	}
}
