using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeShop_API.DATA
{
    public class CityRepository
    {
        private readonly IConfiguration configuration;

        public CityRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region GetAllCities
        public IEnumerable<CityModel> GetAllCities()
        {
            var cities = new List<CityModel>();
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {

                SqlCommand cmd = new SqlCommand("PR_LOC_City_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cities.Add(new CityModel
                    {
                        CityID = Convert.ToInt32(reader["CityID"]),
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        StateID = Convert.ToInt32(reader["StateID"]),
                        CityName = reader["CityName"].ToString(),
                        CityCode = reader["CityCode"].ToString()
                    });
                }
            }
            return cities;
        }
        #endregion

        #region InsertCity
        public bool InsertCity(CityModel cityModel)
        {
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CityName", cityModel.CityName);
                cmd.Parameters.AddWithValue("CityCode", cityModel.CityCode);
                cmd.Parameters.AddWithValue("CountryID", cityModel.CountryID);
                cmd.Parameters.AddWithValue("StateID", cityModel.StateID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;

            }
        }
        #endregion

        #region UpdateCity
        public bool UpdateCity(CityModel cityModel)
        {
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CityID", Convert.ToInt32(cityModel.CityID));
                cmd.Parameters.AddWithValue("CityName", cityModel.CityName);
                cmd.Parameters.AddWithValue("CityCode", cityModel.CityCode);
                cmd.Parameters.AddWithValue("CountryID", Convert.ToInt32(cityModel.CountryID));
                cmd.Parameters.AddWithValue("StateID", Convert.ToInt32(cityModel.StateID));

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;

            }
        }
        #endregion

        #region DeleteCity
        public bool DeleteCity(int CityID)
        {
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CityID", Convert.ToInt32(CityID));

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;

            }
        }
        #endregion

        #region GetCityByPK
        public CityModel GetCityByPK(int CityID)
        {
            CityModel cmodel = new CityModel();
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"))) 
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CityID", CityID);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    cmodel.CityID = Convert.ToInt32(reader["CityID"]);
                    cmodel.CountryID = Convert.ToInt32(reader["CountryID"]);
                    cmodel.StateID = Convert.ToInt32(reader["StateID"]);
                    cmodel.CityName = reader["CityName"].ToString();
                    cmodel.CityCode = reader["CityCode"].ToString();
                }
            }
            return cmodel;
        }
        #endregion
    }
}
