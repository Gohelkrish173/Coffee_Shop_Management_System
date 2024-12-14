using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShop_API.DATA
{
    public class BillRepository
    {
        private readonly IConfiguration _configuration;

        public BillRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region GetAllBill
        public IEnumerable<BillModel> GetAllBill()
        {
            List<BillModel> bills = new List<BillModel>();
            using(SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectAll_Bill", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    bills.Add(new BillModel
                    {
                        BillID = Convert.ToInt32(reader["BillID"]),
                        BillNumber = reader["BillNumber"].ToString(),
                        BillDate = Convert.ToDateTime(reader["BillDate"]),
                        OrderNO = reader["OrderNO"].ToString(),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Discount = Convert.ToDecimal(reader["Discount"]),
                        NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                        UserName = reader["UserName"].ToString(),
                    });
                }
                return bills;
            }
        }
        #endregion

        #region DeleteBill
        public bool DeleteBill(int BillID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Delete_Bill", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("BillID", BillID);
                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region InsertBill
        public bool InsertBill(BillModel billModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Insert_Bill", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("BillNumber", billModel.BillNumber);
                cmd.Parameters.AddWithValue("BillDate", billModel.BillDate);
                cmd.Parameters.AddWithValue("OrderID", billModel.OrderID);
                cmd.Parameters.AddWithValue("TotalAmount", billModel.TotalAmount);
                cmd.Parameters.AddWithValue("Discount",billModel.Discount);
                cmd.Parameters.AddWithValue("NetAmount", billModel.NetAmount);
                cmd.Parameters.AddWithValue("UserID", billModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region UpdateBill
        public bool UpdateBill(BillModel billModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Update_Bill", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("BillID", billModel.BillID);
                cmd.Parameters.AddWithValue("BillNumber", billModel.BillNumber);
                cmd.Parameters.AddWithValue("BillDate", billModel.BillDate);
                cmd.Parameters.AddWithValue("OrderID", billModel.OrderID);
                cmd.Parameters.AddWithValue("TotalAmount", billModel.TotalAmount);
                cmd.Parameters.AddWithValue("Discount", billModel.Discount);
                cmd.Parameters.AddWithValue("NetAmount", billModel.NetAmount);
                cmd.Parameters.AddWithValue("UserID", billModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region SelectByPkBill
        public BillModel GetBillByPK(int BillID)
        {
            BillModel bmodel = new BillModel();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectByPK_Bill", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("BillID", BillID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    bmodel.BillID = Convert.ToInt32(reader["BillID"]);
                    bmodel.BillDate = Convert.ToDateTime(reader["BillDate"]);
                    bmodel.BillNumber = reader["BillNumber"].ToString();
                    bmodel.OrderID = Convert.ToInt32(reader["OrderID"]);
                    bmodel.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    bmodel.Discount = Convert.ToDecimal(reader["Discount"]);
                    bmodel.NetAmount = Convert.ToDecimal(reader["NetAmount"]);
                    bmodel.UserID = Convert.ToInt32(reader["UserID"]);
                }

                return bmodel;
            }
        }
        #endregion
    }
}
