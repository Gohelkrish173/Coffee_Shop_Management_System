using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class CountryModel
    {
        public int? CountryID { get; set; }
        [Required]
        public String CountryName { get; set; }
        [Required]
        public String CountryCode {  get; set; }
        public int? StateCount { get; set; }
    }
}
