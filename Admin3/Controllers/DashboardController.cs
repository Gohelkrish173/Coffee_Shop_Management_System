using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin3.Bal;

namespace Admin3.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;

        public DashboardController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new Dashboard
            {
                Counts = new List<DashboardCounts>(),
                RecentOrders = new List<RecentOrder>(),
                RecentProducts = new List<RecentProduct>(),
                TopCustomers = new List<TopCustomer>(),
                NavigationLinks = new List<QuickLinks>()
            };

            using (var connection = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("PR_Dashboard", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("UserID", Convert.ToInt64(HttpContext.Session.GetString("UserID")));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            // Fetch counts
                            while (await reader.ReadAsync())
                            {
                                dashboardData.Counts.Add(new DashboardCounts
                                {
                                    Metric = reader["Metric"].ToString(),
                                    Value = Convert.ToInt32(reader["Value"])
                                });
                            }

                            // Fetch recent orders
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.RecentOrders.Add(new RecentOrder
                                    {
                                        OrderID = Convert.ToInt32(reader["OrderID"]),
                                        CustomerName = reader["CustomerName"].ToString(),
                                        OrderDate = Convert.ToDateTime(reader["OrderDate"])
                                    });
                                }
                            }

                            // Fetch recent products
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.RecentProducts.Add(new RecentProduct
                                    {
                                        ProductID = Convert.ToInt32(reader["ProductID"]),
                                        ProductName = reader["ProductName"].ToString(),
                                        ProductCode = reader["ProductCode"].ToString(),
                                        ProductPrice = Convert.ToDecimal(reader["ProductPrice"])
                                    });
                                }
                            }

                            // Fetch top customers
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.TopCustomers.Add(new TopCustomer
                                    {
                                        CustomerName = reader["CustomerName"].ToString(),
                                        TotalOrders = Convert.ToInt32(reader["TotalOrders"]),
                                        Email = reader["Email"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }

            dashboardData.NavigationLinks = new List<QuickLinks> {
        new QuickLinks {ActionMethodName = "ProductTable", ControllerName="Product", LinkName="Product" },
        new QuickLinks {ActionMethodName = "UserTable", ControllerName="User", LinkName="User" },
        new QuickLinks {ActionMethodName = "OrderTable", ControllerName="Order", LinkName="Order" },
        new QuickLinks {ActionMethodName = "OrderDetailTable", ControllerName="OrderDetail", LinkName="Order Detail" },
        new QuickLinks {ActionMethodName = "BillTable", ControllerName="Bills", LinkName="Bill" },
        new QuickLinks {ActionMethodName = "CustomerTable", ControllerName="Customer", LinkName="Customer" },
        new QuickLinks {ActionMethodName = "CountryTable", ControllerName="Country", LinkName="Country" },
        new QuickLinks {ActionMethodName = "StateTable", ControllerName="State", LinkName="State" },
        new QuickLinks {ActionMethodName = "CityTable", ControllerName="City", LinkName="City" }
    };

            var model = new Dashboard
            {
                Counts = dashboardData.Counts,
                RecentOrders = dashboardData.RecentOrders,
                RecentProducts = dashboardData.RecentProducts,
                TopCustomers = dashboardData.TopCustomers,
                NavigationLinks = dashboardData.NavigationLinks
            };

            return View(model);
        }
    }
}
