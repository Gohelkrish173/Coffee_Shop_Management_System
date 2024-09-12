using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class ProductModel
    {
        public int? ProductID {  get; set; }
        [Required]
        [StringLength(100,ErrorMessage="Enter A valid Name")]
        public string ProductName   { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        [StringLength(3,ErrorMessage ="Code length is must be 3")]
        public string ProductCode { get; set; }
        [Required]
        public string Description   { get; set; }
        [Required]
        public int UserID   { get; set; }
    }
}
