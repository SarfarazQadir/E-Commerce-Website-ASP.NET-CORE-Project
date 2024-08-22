using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Website.Models
{
    public class Product
    {
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }
        public string product_image { get; set; }
        public string product_description { get; set; }

		[ForeignKey("Category")]
		public int category_id { get; set; }
        public Category Category { get; set; }
        
    }
}
