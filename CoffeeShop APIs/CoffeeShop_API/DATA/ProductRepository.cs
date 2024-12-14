using CoffeeShop_API.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShop_API.DATA
{
    public class ProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region GetAllProduct
        public IEnumerable<ProductModel> GetAllProduct()
        {
            List<ProductModel> products = new List<ProductModel>();
            using(SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectAll_Product", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new ProductModel
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCode = reader["ProductCode"].ToString(),
                        ProductPrice = Convert.ToDouble(reader["ProductPrice"]),
                        Description = reader["Description"].ToString(),
                        UserName = reader["UserName"].ToString(),
                    });
                }

                return products;
            }
        }
        #endregion

        #region DeleteProduct
        public bool DeleteProduct(int ProductID)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Delete_Product", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("ProductID", ProductID);
                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region InsertProduct
        public bool InsertProduct(ProductModel productModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Insert_Product", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                cmd.Parameters.AddWithValue("ProductName", productModel.ProductName);
                cmd.Parameters.AddWithValue("ProductCode", productModel.ProductCode);
                cmd.Parameters.AddWithValue("ProductPrice", productModel.ProductPrice);
                cmd.Parameters.AddWithValue("Description", productModel.Description);
                cmd.Parameters.AddWithValue("UserID", productModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region UpdateProduct
        public bool UpdateProduct(ProductModel productModel)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_Update_Product", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                conn.Open();
                cmd.Parameters.AddWithValue("ProductID", productModel.ProductID);
                cmd.Parameters.AddWithValue("ProductName", productModel.ProductName);
                cmd.Parameters.AddWithValue("ProductCode", productModel.ProductCode);
                cmd.Parameters.AddWithValue("ProductPrice", productModel.ProductPrice);
                cmd.Parameters.AddWithValue("Description", productModel.Description);
                cmd.Parameters.AddWithValue("UserID", productModel.UserID);

                int ReflectedMessage = cmd.ExecuteNonQuery();

                return ReflectedMessage > 0;
            }
        }
        #endregion

        #region GetProductByPK
        public ProductModel GetProductByPK(int productID)
        {
            ProductModel productModel = new ProductModel();
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("myConnection")))
            {
                SqlCommand cmd = new SqlCommand("PR_SelectByPK_Product", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();

                cmd.Parameters.AddWithValue("ProductID", productID);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productModel.ProductID = Convert.ToInt32(reader["ProductID"]);
                    productModel.ProductName = reader["ProductName"].ToString();
                    productModel.ProductCode = reader["ProductCode"].ToString();
                    productModel.ProductPrice = Convert.ToDouble(reader["ProductPrice"]);
                    productModel.Description = reader["Description"].ToString();
                    productModel.UserID = Convert.ToInt32(reader["UserID"]);
                }

                return productModel;
            }
        }
        #endregion
    }
}
