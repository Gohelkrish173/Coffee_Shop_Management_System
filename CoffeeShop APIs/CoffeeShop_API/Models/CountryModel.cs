namespace CoffeeShop_API.Models
{
    public class CountryModel
    {
        public int CountryID { get; set; }
        public String CountryName { get; set; }
        public String CountryCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
