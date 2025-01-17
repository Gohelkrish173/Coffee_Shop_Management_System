namespace Admin3.Models
{
    public class DashboardCounts
    {
        public string Metric { get; set; }
        public int Value { get; set; }
    }

    public class RecentOrder
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class RecentProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }

        public decimal ProductPrice { get; set; }
    }

    public class TopCustomer
    {
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public string Email { get; set; }
    }

    public class QuickLinks
    {
        public string ActionMethodName { get; set; }
        public string ControllerName { get; set; }
        public string LinkName { get; set; }
    }

    public class Dashboard
    {
        public List<DashboardCounts> Counts { get; set; }
        public List<RecentOrder> RecentOrders { get; set; }
        public List<RecentProduct> RecentProducts { get; set; }
        public List<TopCustomer> TopCustomers { get; set; }
        public List<QuickLinks> NavigationLinks { get; set; }
    }
}
