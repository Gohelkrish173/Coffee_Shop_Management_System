using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_API.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public string? OrderNO { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
}
