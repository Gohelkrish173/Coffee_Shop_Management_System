using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_API.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }
        [Required]
        public int OrderID { get; set; }
        
        public String OrderNO { get; set; }
        [Required]
        public int ProductID { get; set; }
        public String ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public int UserID { get; set; }
        public String UserName { get; set; }
    }
}
