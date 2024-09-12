using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class OrderDropDownModel 
    {
        public int OrderID { get; set; }
        [Required]
        public string OrderNO { get; set; }
    }
}
