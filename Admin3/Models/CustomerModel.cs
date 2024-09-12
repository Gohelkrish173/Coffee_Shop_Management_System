using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class CustomerModel
    {
        public int CustomerID   { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string HomeAddress { get; set; }
        [Required]
        [EmailAddress]
        public string Email {  get; set; }
        [Required]
        [Phone]
        public string MobileNO { get; set; }
        [Required]
        public string GST_NO { get; set; }
        [Required]
        public string CityName  { get; set; }
        [Required]
        public string PinCode { get; set; }
        [Required]
        public decimal NetAmount    { get; set; }
        [Required]
        public int UserID { get; set; }
    }
}
