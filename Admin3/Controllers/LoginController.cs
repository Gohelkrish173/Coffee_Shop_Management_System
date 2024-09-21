using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin3.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration configuration;

        public LoginController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Login()
        {
            return View();
        }

        #region UserLogin
        public IActionResult UserLogin(UserLoginModel modeluser)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Login_User";
                cmd.Parameters.AddWithValue("@UserName", modeluser.UserName);
                cmd.Parameters.AddWithValue("@Password", modeluser.Password);
                SqlDataReader r = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(r);

                foreach (DataRow dr in dt.Rows)
                {
                    HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                    HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                    HttpContext.Session.SetString("EmailAddress", dr["Email"].ToString());
                }

                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToAction("Login");
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("UserLogin");
        }
        #endregion
    }
}
