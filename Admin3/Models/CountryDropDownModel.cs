using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class CountryDropDownModel 
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
}
