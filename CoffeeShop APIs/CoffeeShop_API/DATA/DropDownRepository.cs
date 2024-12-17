using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeShop_API.DATA
{
    public class DropDownRepository
    {
        private IConfiguration _configuration;

        public DropDownRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region UserDropDown
        public IEnumerable<UserDropDownModel> LoadUserDropDown()
        {
            var UDD = new List<UserDropDownModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {

                SqlCommand cmd = new SqlCommand("PR_DropDown_Users", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UDD.Add(new UserDropDownModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                    });
                }
            }
            return UDD;
        }
        #endregion

        #region CustomerDropDown
        public IEnumerable<CustomerDropDownModel> LoadCustomerDropDown()
        {
            var CDD = new List<CustomerDropDownModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {

                SqlCommand cmd = new SqlCommand("PR_DropDown_Customer", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CDD.Add(new CustomerDropDownModel
                    {
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        CustomerName = reader["CustomerName"].ToString(),
                    });
                }
            }
            return CDD;
        }
        #endregion

        #region ProductDropDownModel
        public IEnumerable<ProductDropDownModel> LoadProductDropDown()
        {
            var PDD = new List<ProductDropDownModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {

                SqlCommand cmd = new SqlCommand("PR_DropDown_Product", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PDD.Add(new ProductDropDownModel
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                    });
                }
            }
            return PDD;
        }
        #endregion

        #region OrderDropDown
        public IEnumerable<OrderDropDownModel> LoadOrderDropDown()
        {
            var ODD = new List<OrderDropDownModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {

                SqlCommand cmd = new SqlCommand("PR_DropDown_Order", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ODD.Add(new OrderDropDownModel
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        OrderNO = reader["OrderNO"].ToString(),
                    });
                }
            }
            return ODD;
        }
        #endregion

        #region CountryDropDown
        public IEnumerable<CountryDropDownModel> GetCountryDropDown()
        {
            List<CountryDropDownModel> CDDM = new List<CountryDropDownModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_SelectComboBox", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CDDM.Add(new CountryDropDownModel
                    {
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CountryName = reader["CountryName"].ToString()
                    });
                }
            }
            return CDDM;
        }
        #endregion

        #region StateDropDownByCountry
        public IEnumerable<StateDropDownModel> GetStateDropDownByCountry(int CountryID)
        {
            List<StateDropDownModel> SDDM = new List<StateDropDownModel>();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_SelectComboBoxByCountryID", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("CountryID", CountryID);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SDDM.Add(new StateDropDownModel
                    {
                        StateID = Convert.ToInt32(reader["StateID"]),
                        StateName = reader["StateName"].ToString()
                    });
                }
            }
            return SDDM;
        }
        #endregion
    }
}
