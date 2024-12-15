namespace CoffeeShop_API.Models
{
    public class StateModel
    {
        public int StateID { get; set; }
        public String StateName { get; set; }
        public String StateCode { get; set; }
        public int CountryID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
