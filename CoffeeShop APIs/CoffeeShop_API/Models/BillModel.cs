using System.ComponentModel.DataAnnotations;

namespace CoffeeShop_API.Models
{
    public class BillModel
    {
        public int BillID { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public int OrderID { get; set; }

        public string? OrderNO { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
}
