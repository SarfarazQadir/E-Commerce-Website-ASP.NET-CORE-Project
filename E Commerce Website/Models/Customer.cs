using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Customer
    {
        [Key]
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string? customer_phone { get; set; }
        public string customer_email { get; set; }
        public string customer_password { get; set; }
        public string? customer_gender { get; set; }
        public string? customer_country { get; set; }
        public string? customer_city { get; set; }
        public string? customer_address { get; set; }
        public string? customer_image { get; set; }
    }
}
