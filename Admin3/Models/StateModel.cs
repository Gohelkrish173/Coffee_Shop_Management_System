using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Admin3.Models
{
    public class StateModel
    {
        public int StateID { get; set; }
        [Required]
        public String StateName { get; set; }
        [Required]
        public String StateCode { get; set; }
        [Required]
        public int CountryID { get; set; }
    }
}
