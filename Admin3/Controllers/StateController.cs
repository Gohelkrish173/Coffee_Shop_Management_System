using Admin3.Bal;
using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Admin3.Controllers
{
    public class StateController : Controller
    {
        private IConfiguration configuration;

        #region Configuration
        public StateController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region RetrieveData
        public DataTable RetrieveData(String SP,int? PKID=0,String PKName = "")
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SP;
            if(PKID != 0)
            {
                cmd.Parameters.AddWithValue("@"+PKName,PKID);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            return dt;
        }
        #endregion

        #region StateTable
        public IActionResult StateTable()
        {
            DataTable Statedt = RetrieveData("PR_LOC_State_SelectAll");
            return View(Statedt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(string? StateID)
        {
            int decryptedID = Convert.ToInt32(UrlEncryptor.Decrypt(StateID));

            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_Delete";
            cmd.Parameters.AddWithValue("@StateID", decryptedID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("StateTable");
        }
        #endregion

        #region LoadCountry
        private void LoadCountry()
        {
            DataTable Countrydt = RetrieveData("PR_LOC_Country_SelectComboBox");

            List<CountryDropDownModel> cdd = new List<CountryDropDownModel>();

            foreach (DataRow row in Countrydt.Rows)
            {
                CountryDropDownModel model = new CountryDropDownModel();
                model.CountryID = Convert.ToInt32(row["CountryID"]);
                model.CountryName = row["CountryName"].ToString();
                cdd.Add(model);
            }

            ViewBag.CountryList = cdd;
        }
        #endregion

        #region StateAddEdit
        public IActionResult StateAddEdit(string? StateID)
        {
            int? decryptedID = null;

            if (!string.IsNullOrEmpty(StateID))
            {
                decryptedID = Convert.ToInt32(UrlEncryptor.Decrypt(StateID));
            }

            LoadCountry();

            if(decryptedID.HasValue)
            {
                DataTable Statedt = RetrieveData("PR_LOC_State_SelectPK", decryptedID, "StateID");
                StateModel smodel = new StateModel();
                
                foreach(DataRow row in Statedt.Rows)
                {
                    smodel.StateID = Convert.ToInt32(row["StateID"]);
                    smodel.StateName = row["StateName"].ToString();
                    smodel.StateCode = row["StateCode"].ToString();
                    smodel.CountryID = Convert.ToInt32(row["CountryID"]);
                }
                return View("StateForm", smodel);
            }
            else
            {
                return View("StateForm",new StateModel());
            }
        }
        #endregion

        #region StateSave
        public IActionResult StateSave(StateModel smodel)
        {
            if (smodel.CountryID <= 0 && smodel.CountryID == null)
            {
                ModelState.AddModelError("CountryID", "Invalid CountryID");
            }

            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if(smodel.StateID == 0 || smodel.StateID == null)
                {
                    cmd.CommandText = "PR_LOC_State_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_LOC_State_Update";
                    cmd.Parameters.AddWithValue("@StateID", smodel.StateID);
                }
                cmd.Parameters.AddWithValue("@StateName", smodel.StateName);
                cmd.Parameters.AddWithValue("@StateCode", smodel.StateCode);
                cmd.Parameters.AddWithValue("@CountryID", smodel.CountryID);
                cmd.ExecuteNonQuery();

                return RedirectToAction("StateTable");
            }
            else
            {
                LoadCountry();
                return View("StateForm", smodel);
            }
        }
        #endregion
    }
}
