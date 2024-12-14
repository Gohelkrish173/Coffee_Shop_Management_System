using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class CityModel
    {
        public int CityID { get; set; }
        //[Required]
        public string CityName { get; set; }
        //[Required]
        public int CountryID { get; set; }
        //[Required]
        public int StateID { get; set; }
        //[Required]
        public string CityCode { get; set; }
    }
}
