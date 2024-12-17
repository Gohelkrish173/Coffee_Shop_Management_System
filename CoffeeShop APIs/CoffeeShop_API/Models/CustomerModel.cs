using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_API.Models
{
    public class CustomerModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string MobileNO { get; set; }
        public string GST_NO { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public decimal NetAmount { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
}
