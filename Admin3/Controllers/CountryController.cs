using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Security.Cryptography.Pkcs;
using System.Text.Json.Nodes;
using Admin3.Bal;

namespace Admin3.Controllers
{
    public class CountryController : Controller
    {
        private IConfiguration configuration;

        Uri baseAddress = new Uri("http://localhost:5223/api");
        private readonly HttpClient _client;

        #region Configuration
        //public CountryController()
        //{
        //    _client = new HttpClient();
        //    _client.BaseAddress = baseAddress;
        //}

        public CountryController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region CountryRetrive
        public DataTable RetriveTable(String SP, int? PKID = 0, string PKName = "")
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SP;
            if(PKID != 0)
            {
                cmd.Parameters.AddWithValue("@" + PKName, PKID);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            return dt;
        }
        #endregion

        #region CountryTable
        [HttpGet]
        public IActionResult CountryTable()
        {
            //List<CountryModel> Countries = new List<CountryModel>();
            //HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Country").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    string data = response.Content.ReadAsStringAsync().Result;
            //    Countries = JsonConvert.DeserializeObject<List<CountryModel>>(data);
            //}
            DataTable Countries = RetriveTable("PR_LOC_Country_SelectAll");
            return View(Countries);
        }
        #endregion

        #region CountryDelete
        public IActionResult Delete(string? CountryID)
        {
            int decryptedID = Convert.ToInt32(UrlEncryptor.Decrypt(CountryID));

            try
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_Country_Delete";
                cmd.Parameters.AddWithValue("@CountryID", decryptedID);
                cmd.ExecuteNonQuery();
                conn.Close();

                TempData["DeleteMSg"] = "Country Delete SuccessFully";
            }
            catch (Exception ex)
            {
                TempData["DeleteMsg"] = ex.Message;
            }
            return RedirectToAction("CountryTable");
        }
        #endregion

        #region CountryAddEdit
        public IActionResult CountryAddEdit(string? CountryID)
        {
            int? decryptedID = null;

            if (!string.IsNullOrEmpty(CountryID)) 
            {
                decryptedID = Convert.ToInt32(UrlEncryptor.Decrypt(CountryID));    
            }

            if (decryptedID.HasValue)
            {
                DataTable Countrydt = RetriveTable("PR_LOC_Country_SelectByPK", decryptedID, "CountryID");
                
                CountryModel countrymodel = new CountryModel();

                foreach(DataRow row in Countrydt.Rows)
                {
                    countrymodel.CountryID = Convert.ToInt32(row["CountryID"]);
                    countrymodel.CountryName = row["CountryName"].ToString();
                    countrymodel.CountryCode = row["CountryCode"].ToString();
                }

                return View("CountryForm",countrymodel);
            }
            else
            {
                return View("CountryForm",new CountryModel());
            }
        }
        #endregion

        #region CountrySave
        public IActionResult CountrySave(CountryModel cmodel)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                Console.WriteLine(cmodel.CountryID);
                if(cmodel.CountryID == 0 || cmodel.CountryID == null)
                {
                    cmd.CommandText = "PR_LOC_Country_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_LOC_Country_Update";
                    cmd.Parameters.AddWithValue("CountryID", cmodel.CountryID);
                }
                cmd.Parameters.AddWithValue("CountryName", cmodel.CountryName);
                cmd.Parameters.AddWithValue("CountryCode", cmodel.CountryCode);
                cmd.ExecuteNonQuery();
                conn.Close();

                return RedirectToAction("CountryTable");
            }
            else
            {
                return RedirectToAction("CountryAddEdit", cmodel);
            }
        }
        #endregion

        #region AjaxGet
        [HttpGet]
        public DataTable GetAll()
        {
            return RetriveTable("PR_LOC_Country_SelectAll");
        }
        #endregion

    }
}
