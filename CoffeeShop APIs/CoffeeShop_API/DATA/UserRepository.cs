using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;
using System.Net;

namespace CoffeeShop_API.DATA
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region GetAllUser
        public IEnumerable<UserModel> GetAllUser()
        {
            List<UserModel> users = new List<UserModel>();
            using(SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectAll_Users", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UserModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        MobileNo = reader["MobileNo"].ToString(),
                        Address = reader["Address"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }

                return users;
            }
        }
        #endregion

        #region DeleteUser
        public bool DeleteUser(int UserID)
        {
            using(SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Delete_Users", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("UserID", UserID);
                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region InsertUser
        public bool InsertUser(UserModel userModel)
        {
            using(SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Insert_Users", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("UserName",userModel.UserName);
                cmd.Parameters.AddWithValue("Email", userModel.Email);
                cmd.Parameters.AddWithValue("Password", userModel.Password);
                cmd.Parameters.AddWithValue("Address", userModel.Address);
                cmd.Parameters.AddWithValue("MobileNo", userModel.MobileNo);
                cmd.Parameters.AddWithValue("IsActive", userModel.IsActive);
                
                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region UpdateUser
        public bool UpdateUser(UserModel userModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Update_Users", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("UserID", userModel.UserID);
                cmd.Parameters.AddWithValue("UserName", userModel.UserName);
                cmd.Parameters.AddWithValue("Email", userModel.Email);
                cmd.Parameters.AddWithValue("Password", userModel.Password);
                cmd.Parameters.AddWithValue("Address", userModel.Address);
                cmd.Parameters.AddWithValue("MobileNo", userModel.MobileNo);
                cmd.Parameters.AddWithValue("IsActive", userModel.IsActive);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region GetUserByPK
        public UserModel GetUserByPK(int UserID) 
        {
            UserModel userModel = new UserModel();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectByPK_Users", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("UserID", UserID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    userModel.UserID = Convert.ToInt32(reader["UserID"]);
                    userModel.UserName = reader["UserName"].ToString();
                    userModel.Email = reader["Email"].ToString();
                    userModel.Password = reader["Password"].ToString();
                    userModel.MobileNo = reader["MobileNo"].ToString();
                    userModel.Address = reader["Address"].ToString();
                    userModel.IsActive = Convert.ToBoolean(reader["IsActive"]);
                }

                return userModel;
            }
        }
        #endregion
    }
}
