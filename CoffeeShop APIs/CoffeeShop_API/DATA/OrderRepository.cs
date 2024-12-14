using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShop_API.DATA
{
    public class OrderRepository
    {
        private readonly IConfiguration _configuration;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region GetAllOrder
        public IEnumerable<OrderModel> GetAllOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectAll_Orders", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderModel
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        OrderNO = reader["OrderNO"].ToString(),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        CustomerName = reader["CustomerName"].ToString(),
                        PaymentMode = reader["PaymentMode"].ToString(),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        ShippingAddress = reader["ShippingAddress"].ToString(),
                        UserName = reader["UserName"].ToString()
                    });
                }

                return orders;
            }
        }
        #endregion

        #region DeleteOrder
        public bool DeleteOrder(int OrderID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Delete_Order", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("OrderID", OrderID);
                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region InsertOrder
        public bool InsertOrder(OrderModel orderModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Insert_Orders", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("OrderNO", orderModel.OrderNO);
                cmd.Parameters.AddWithValue("OrderDate", orderModel.OrderDate);
                cmd.Parameters.AddWithValue("CustomerID", orderModel.CustomerID);
                cmd.Parameters.AddWithValue("PaymentMode", orderModel.PaymentMode);
                cmd.Parameters.AddWithValue("TotalAmount", orderModel.TotalAmount);
                cmd.Parameters.AddWithValue("ShippingAddress", orderModel.ShippingAddress);
                cmd.Parameters.AddWithValue("UserID", orderModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region UpdateOrder
        public bool UpdateOrder(OrderModel orderModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Update_Orders", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("OrderID", orderModel.OrderID);
                cmd.Parameters.AddWithValue("OrderNO", orderModel.OrderNO);
                cmd.Parameters.AddWithValue("OrderDate", orderModel.OrderDate);
                cmd.Parameters.AddWithValue("CustomerID", orderModel.CustomerID);
                cmd.Parameters.AddWithValue("PaymentMode", orderModel.PaymentMode);
                cmd.Parameters.AddWithValue("TotalAmount", orderModel.TotalAmount);
                cmd.Parameters.AddWithValue("ShippingAddress", orderModel.ShippingAddress);
                cmd.Parameters.AddWithValue("UserID", orderModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region GetOrderByPK
        public OrderModel GetOrderByPK(int orderID)
        {
            OrderModel orderModel = new OrderModel();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectByPK_Order", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("OrderID", orderID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orderModel.OrderID = Convert.ToInt32(reader["OrderID"]);
                    orderModel.OrderNO = reader["OrderNO"].ToString();
                    orderModel.OrderDate = Convert.ToDateTime(reader["OrderDate"]);
                    orderModel.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                    orderModel.PaymentMode = reader["PaymentMode"].ToString();
                    orderModel.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    orderModel.ShippingAddress = reader["ShippingAddress"].ToString();
                    orderModel.UserID = Convert.ToInt32(reader["UserID"]);
                }
                
                return orderModel;
            }
        }
        #endregion
    }
}
