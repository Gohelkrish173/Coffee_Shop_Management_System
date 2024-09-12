using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class BillsModel
    {
        public int BillID { get; set; }
        [Required]
        public string BillNumber { get; set; }
        [Required]
        public DateTime BillDate    { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public decimal Discount {  get; set; }
        [Required]
        public decimal NetAmount { get; set; }
        [Required]
        public int UserID   { get; set; }
    }
}
