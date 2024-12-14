using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShop_API.DATA
{
    public class CustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region GetAllCustomers
        public IEnumerable<CustomerModel> GetAllCustomer()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            using(SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectAll_Customer", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    customers.Add(new CustomerModel
                    {
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        CustomerName = reader["CustomerName"].ToString(),
                        HomeAddress = reader["HomeAddress"].ToString(),
                        Email = reader["Email"].ToString(),
                        MobileNO = reader["MobileNO"].ToString(),
                        GST_NO = reader["GST_NO"].ToString(),
                        CityName = reader["CityName"].ToString(),
                        PinCode = reader["PinCode"].ToString(),
                        NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                        UserName = reader["UserName"].ToString()
                    });
                }

                return customers;
            }
        }
        #endregion

        #region DeleteCustomer
        public bool DeleteCustomer(int CustomerID) 
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Delete_Customer", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("CustomerID", CustomerID);
                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region InsertCustomer
        public bool InsertCustomer(CustomerModel customerModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Insert_Customer", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("CustomerName", customerModel.CustomerName);
                cmd.Parameters.AddWithValue("HomeAddress", customerModel.HomeAddress);
                cmd.Parameters.AddWithValue("Email", customerModel.Email);
                cmd.Parameters.AddWithValue("MobileNO", customerModel.MobileNO);
                cmd.Parameters.AddWithValue("GST_NO", customerModel.GST_NO);
                cmd.Parameters.AddWithValue("CityName", customerModel.CityName);
                cmd.Parameters.AddWithValue("PinCode", customerModel.PinCode);
                cmd.Parameters.AddWithValue("NetAmount", customerModel.NetAmount);
                cmd.Parameters.AddWithValue("UserID", customerModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region UpdateCustomer
        public bool UpdateCustomer(CustomerModel customerModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Update_Customer", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CustomerID", customerModel.CustomerID);
                cmd.Parameters.AddWithValue("CustomerName", customerModel.CustomerName);
                cmd.Parameters.AddWithValue("HomeAddress", customerModel.HomeAddress);
                cmd.Parameters.AddWithValue("Email", customerModel.Email);
                cmd.Parameters.AddWithValue("MobileNO", customerModel.MobileNO);
                cmd.Parameters.AddWithValue("GST_NO", customerModel.GST_NO);
                cmd.Parameters.AddWithValue("CityName", customerModel.CityName);
                cmd.Parameters.AddWithValue("PinCode", customerModel.PinCode);
                cmd.Parameters.AddWithValue("NetAmount", customerModel.NetAmount);
                cmd.Parameters.AddWithValue("UserID", customerModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region SelectByPkCustomer
        public CustomerModel GetCustomerByPK(int CustomerID)
        {
            CustomerModel customerModel = new CustomerModel();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectByPK_Customer", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CustomerID", CustomerID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customerModel.CustomerID = Convert.ToInt32(reader["CustomerID"]);
                    customerModel.CustomerName = reader["CustomerName"].ToString();
                    customerModel.HomeAddress = reader["HomeAddress"].ToString();
                    customerModel.Email = reader["Email"].ToString();
                    customerModel.MobileNO = reader["MobileNO"].ToString();
                    customerModel.GST_NO = reader["GST_NO"].ToString();
                    customerModel.CityName = reader["CityName"].ToString();
                    customerModel.PinCode = reader["PinCode"].ToString();
                    customerModel.NetAmount = Convert.ToDecimal(reader["NetAmount"]);
                    customerModel.UserID = Convert.ToInt32(reader["UserID"]);
                }

                return customerModel;
            }
        }
        #endregion
    }
}
