using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeShop_API.DATA
{
    public class CountryRepository
    {
        private readonly IConfiguration configuration;

        public CountryRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region GetAllCountries
        public IEnumerable<CountryModel> GetAllCountries()
        {
            var countries = new List<CountryModel>();
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_SelectAll", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    countries.Add(new CountryModel
                    {
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CountryName = reader["CountryName"].ToString(),
                        CountryCode = reader["CountryCode"].ToString(),
                        StateCount = Convert.ToInt32(reader["StateCount"])
                    });
                }
                return countries;
            }
        }
        #endregion

        #region InsertCountry
        public bool InsertCountry(CountryModel countryModel)
        {
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CountryName", countryModel.CountryName);
                cmd.Parameters.AddWithValue("CountryCode", countryModel.CountryCode);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;

            }
        }
        #endregion

        #region UpdateCountry
        public bool UpdateCountry(CountryModel countryModel)
        {
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CountryName", countryModel.CountryName);
                cmd.Parameters.AddWithValue("CountryCode", countryModel.CountryCode);
                cmd.Parameters.AddWithValue("CountryID", Convert.ToInt32(countryModel.CountryID));

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;

            }
        }
        #endregion

        #region DeleteCountry
        public bool DeleteCountry(int CountryID)
        {
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();

                cmd.Parameters.AddWithValue("CountryID", Convert.ToInt32(CountryID));

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;

            }
        }
        #endregion

        #region GetCountryByPK
        public CountryModel GetCountryByPK(int CountryID)
        {
            CountryModel countryModel = new CountryModel();
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                conn.Open();
                cmd.Parameters.AddWithValue("CountryID", CountryID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) 
                {
                    countryModel.CountryID = Convert.ToInt32(reader["CountryID"]);
                    countryModel.CountryName = reader["CountryName"].ToString();
                    countryModel.CountryCode = reader["CountryCode"].ToString();
                }
            }
            return countryModel;
        }
        #endregion
    }
}
