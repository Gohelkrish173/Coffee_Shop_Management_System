using Admin3.Bal;
using Admin3.Models;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Admin3.Controllers
{
    public class CityController : Controller
    {
        private IConfiguration configuration;

        #region Configuration
        public CityController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region RetriveData
        public DataTable RetriveData(String SP,int? PKID = 0,String RIDName = "") 
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SP;
            if (PKID != 0) 
            {
                cmd.Parameters.AddWithValue("@" + RIDName, PKID);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            return dt;
        }
        #endregion

        #region CityTable
        public IActionResult CityTable()
        {
            DataTable CityTable = RetriveData("PR_LOC_City_SelectAll");
            return View(CityTable);
        }
        #endregion

        #region CityAddEdit
        public IActionResult CityAddEdit(string? CityID)
        {
            int? decryptedID = null;

            // Decrypt only if CityID is not null or empty
            if (!string.IsNullOrEmpty(CityID))
            {
                string decryptedCityIDString = UrlEncryptor.Decrypt(CityID); // Decrypt the encrypted CityID
                decryptedID = int.Parse(decryptedCityIDString); // Convert decrypted string to integer
            }

            LoadCountryList();

            if(decryptedID.HasValue)
            {
                DataTable CityTable = RetriveData("PR_LOC_City_SelectByPK",decryptedID,"CityID");

                CityModel cmodel = new CityModel();

                foreach (DataRow row in CityTable.Rows) 
                {
                    cmodel.CityID = Convert.ToInt32(row["CityID"]);
                    cmodel.CityName = row["CityName"].ToString();
                    cmodel.CityCode = row["CityCode"].ToString();
                    cmodel.StateID = Convert.ToInt32(row["StateID"]);
                    cmodel.CountryID = Convert.ToInt32(row["CountryID"]);
                    ViewBag.StateList = GetStateByCountry(cmodel.CountryID);
                }
                GetStateByCountry(cmodel.CountryID);
                return View("CityForm",cmodel);
            }
            return View("CityForm",new CityModel());
        }
        #endregion

        #region CitySave
        [HttpPost]
        public IActionResult CitySave(CityModel citymodel)
        {
            if(citymodel.CountryID <= 0 && citymodel.CountryID == null)
            {
                ModelState.AddModelError("CountryID", "Invalid CountryID");
            }

            if (citymodel.StateID <= 0 && citymodel.StateID == null)
            {
                ModelState.AddModelError("StateID", "Invalid StateID");
            }

            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                if (citymodel.CityID == 0 || citymodel.CityID == null)
                {
                    cmd.CommandText = "PR_LOC_City_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_LOC_City_Update";
                    cmd.Parameters.AddWithValue("@CityID", citymodel.CityID);
                }
                cmd.Parameters.AddWithValue("@CityName", citymodel.CityName);
                cmd.Parameters.AddWithValue("@CityCode", citymodel.CityCode);
                cmd.Parameters.AddWithValue("@CountryID", citymodel.CountryID);
                cmd.Parameters.AddWithValue("@StateID", citymodel.StateID);

                cmd.ExecuteNonQuery();
                conn.Close();

                return RedirectToAction("CityTable");
            }

            LoadCountryList();
            GetStateByCountry(citymodel.CountryID);
            return View("CityForm", citymodel);
        }
        #endregion

        #region Delete
        public IActionResult Delete(string? CityID) 
        {
            int decryptedID = Convert.ToInt32(UrlEncryptor.Decrypt(CityID));

            try
            {
                SqlConnection Conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                Conn.Open();

                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_City_Delete";
                cmd.Parameters.AddWithValue("@CityID", decryptedID);
                cmd.ExecuteNonQuery();
                Conn.Close();

                TempData["DeleteMsg"] = "City Delete Successfully!";
            }
            catch (Exception ex) 
            {
                TempData["DeleteMsg"] = ex.Message;
            }

            return RedirectToAction("CityTable");
        }
        #endregion

        #region LoadCountryList
        private void LoadCountryList()
        {
            string connectionstr = configuration.GetConnectionString("myConnection");
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand objCmd = conn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_LOC_Country_SelectComboBox";

                    using (SqlDataReader objSDR = objCmd.ExecuteReader())
                    {
                        dt.Load(objSDR); // Load data into DataTable
                    }
                }
            }

            // Map data to list
            List<CountryDropDownModel> countryList = new List<CountryDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                countryList.Add(new CountryDropDownModel
                {
                    CountryID = Convert.ToInt32(dr["CountryID"]),
                    CountryName = dr["CountryName"].ToString()
                });
            }
            ViewBag.CountryList = countryList; // Pass list to view
        }
        #endregion

        #region GetStateByCountry
        [HttpPost]
        public JsonResult GetStateByCountry(int CountryID)
        {
            List<StateDropDownModel> states = GetStates(CountryID);
            Console.WriteLine(Json(states));
            return Json(states);
        }
        #endregion

        #region GetStates
        public List<StateDropDownModel> GetStates(int CountryID)
        {
            DataTable StateTable = RetriveData("PR_LOC_State_SelectComboBoxByCountryID", CountryID, "CountryID");

            List<StateDropDownModel> stateList = new List<StateDropDownModel>();
            foreach (DataRow row in StateTable.Rows)
            {
                var state = new StateDropDownModel
                {
                    StateID = Convert.ToInt32(row["StateID"]),
                    StateName = row["StateName"].ToString()
                };
                stateList.Add(state);
            }

            ViewBag.StateList = stateList;
            return stateList;
        }
        #endregion
    }
}
