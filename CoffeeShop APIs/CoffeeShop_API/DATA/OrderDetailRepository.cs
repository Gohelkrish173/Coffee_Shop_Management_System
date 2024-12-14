using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShop_API.DATA
{
    public class OrderDetailRepository
    {
        private readonly IConfiguration _configuration;

        public OrderDetailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region GetAllOrderDetail
        public IEnumerable<OrderDetailModel> GetAllOrderDetail()
        {
            List<OrderDetailModel> od = new List<OrderDetailModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectAll_OrderDetail", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    od.Add(new OrderDetailModel
                    {
                        OrderDetailID = Convert.ToInt32(reader["OrderDetailID"]),
                        OrderNO = reader["OrderNO"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductName = reader["ProductName"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        UserName = reader["UserName"].ToString()
                    });
                }

                return od;
            }
        }
        #endregion

        #region DeleteOrderDetail
        public bool DeleteOrderDetail(int OrderDetailID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Delete_OrderDetail", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("OrderDetailID", OrderDetailID);
                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region InsertOrderDetail
        public bool InsertOrderDetail(OrderDetailModel orderDetailModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Insert_OrderDetail", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("OrderID", orderDetailModel.OrderID);
                cmd.Parameters.AddWithValue("ProductID", orderDetailModel.ProductID);
                cmd.Parameters.AddWithValue("Quantity", orderDetailModel.Quantity);
                cmd.Parameters.AddWithValue("TotalAmount", orderDetailModel.TotalAmount);
                cmd.Parameters.AddWithValue("Amount", orderDetailModel.Amount);
                cmd.Parameters.AddWithValue("UserID", orderDetailModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region UpdateOrderDetail
        public bool UpdateOrderDetail(OrderDetailModel orderDetailModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Update_OrderDetail", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("OrderDetailID", orderDetailModel.OrderDetailID);
                cmd.Parameters.AddWithValue("OrderID", orderDetailModel.OrderID);
                cmd.Parameters.AddWithValue("ProductID", orderDetailModel.ProductID);
                cmd.Parameters.AddWithValue("Quantity", orderDetailModel.Quantity);
                cmd.Parameters.AddWithValue("TotalAmount", orderDetailModel.TotalAmount);
                cmd.Parameters.AddWithValue("Amount", orderDetailModel.Amount);
                cmd.Parameters.AddWithValue("UserID", orderDetailModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region GetODByPk
        public OrderDetailModel GetOrderDetailByPK(int OrderDetailID)
        {
            OrderDetailModel ODModel = new OrderDetailModel();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectByPK_OrderDetail", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("OrderDetailID", OrderDetailID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ODModel.OrderDetailID = Convert.ToInt32(reader["OrderDetailID"]);
                    ODModel.OrderID = Convert.ToInt32(reader["OrderID"]);
                    ODModel.Quantity = Convert.ToInt32(reader["Quantity"]);
                    ODModel.ProductID = Convert.ToInt32(reader["ProductID"]);
                    ODModel.Amount = Convert.ToDecimal(reader["Amount"]);
                    ODModel.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    ODModel.UserID = Convert.ToInt32(reader["UserID"]);
                }

                return ODModel;
            }
        }
        #endregion
    }
}
