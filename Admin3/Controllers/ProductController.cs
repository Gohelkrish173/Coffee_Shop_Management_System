using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using static Admin3.Controllers.ProductController;
using Admin3.Bal;


namespace Admin3.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        #region StaticData
        //public static List<ProductModel> productModels = new List<ProductModel>()
        //{
        //    new ProductModel { ProductID = 2, ProductName = "Product B", ProductPrice = 29.99m, ProductCode = "PB100", Description = "Description for Product B", UserId = 101 },
        //    new ProductModel { ProductID = 3, ProductName = "Product C", ProductPrice = 39.99m, ProductCode = "PC100", Description = "Description for Product C", UserId = 101 },
        //    new ProductModel { ProductID = 4, ProductName = "Product D", ProductPrice = 49.99m, ProductCode = "PD100", Description = "Description for Product D", UserId = 101 },
        //    new ProductModel { ProductID = 5, ProductName = "Product E", ProductPrice = 59.99m, ProductCode = "PE100", Description = "Description for Product E", UserId = 101 },
        //    new ProductModel { ProductID = 7, ProductName = "Product G", ProductPrice = 79.99m, ProductCode = "PG100", Description = "Description for Product G", UserId = 101 },
        //    new ProductModel { ProductID = 6, ProductName = "Product F", ProductPrice = 69.99m, ProductCode = "PF100", Description = "Description for Product F", UserId = 101 },
        //    new ProductModel { ProductID = 8, ProductName = "Product H", ProductPrice = 89.99m, ProductCode = "PH100", Description = "Description for Product H", UserId = 101 },
        //    new ProductModel { ProductID = 9, ProductName = "Product I", ProductPrice = 99.99m, ProductCode = "PI100", Description = "Description for Product I", UserId = 101 },
        //    new ProductModel { ProductID = 10, ProductName = "Product J", ProductPrice = 109.99m, ProductCode = "PJ100", Description = "Description for Product J", UserId = 101 }
        //};
        #endregion
        private IConfiguration configuration;

        #region ConfigurationConnection
        public ProductController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        //private readonly EmailService _emailService;
        //public ProductController(EmailService emailService)
        //{
        //    _emailService = emailService;
        //}

        #region DisplayProductTable
        public IActionResult ProductTable()
        {
            SqlConnection Conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            Conn.Open();

            SqlCommand sqlCommand = Conn.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_SelectAll_Product";
            DataTable dt = new DataTable();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            dt.Load(reader);

            return View(dt);
        }
        #endregion

        #region ProductForm
        public IActionResult ProductForm()
        {
            return View();
        }
        #endregion

        #region DeleteProduct
        public IActionResult DelProduct(int lol)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand pdcmd = conn.CreateCommand();
                pdcmd.CommandType = CommandType.StoredProcedure;
                pdcmd.CommandText = "PR_Delete_Product";
                pdcmd.Parameters.AddWithValue("@ProductID", lol);
                pdcmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.Write(ex.ToString());
            }
            return RedirectToAction("ProductTable");

        }
        #endregion

        #region ProdcutAddEdit
        public IActionResult ProductAddEdit(int ProductID = 0)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand pdcmd = conn.CreateCommand();
            pdcmd.CommandType = CommandType.StoredProcedure;
            pdcmd.CommandText = "PR_DropDown_Users";
            SqlDataReader reader = pdcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            List<UserDropDownModel> users = new List<UserDropDownModel>();

            foreach (DataRow row in dt.Rows)
            {
                UserDropDownModel u = new UserDropDownModel();
                u.UserID = Convert.ToInt32(row["UserID"]);
                u.UserName = row["UserName"].ToString();
                users.Add(u);
            }

            ViewBag.UserList = users;

            if (ProductID != 0)
            {
                SqlConnection conn1 = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn1.Open();

                SqlCommand pdcmd1 = conn1.CreateCommand();
                pdcmd1.CommandType = CommandType.StoredProcedure;
                pdcmd1.CommandText = "PR_SelectByPK_Product";
                pdcmd1.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader reader1 = pdcmd1.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Load(reader1);
                conn1.Close();
                ProductModel prodmodel = new ProductModel();

                foreach (DataRow row in dt1.Rows)
                {
                    prodmodel.ProductID = Convert.ToInt32(@row["ProductID"]);
                    prodmodel.ProductName = @row["ProductName"].ToString();
                    prodmodel.ProductCode = @row["ProductCode"].ToString();
                    prodmodel.ProductPrice = Convert.ToDouble(@row["ProductPrice"]);
                    prodmodel.Description = @row["Description"].ToString();
                    prodmodel.UserID = Convert.ToInt32(@row["UserID"]);
                }
                return View("ProductForm", prodmodel);
            }

            return View("ProductForm");
        }
        #endregion

        #region ProductSave
        public IActionResult ProductSave(ProductModel pdmodel)
        {
            if (pdmodel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "a valid user id is required.");
            }

            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand pdcmd = conn.CreateCommand();
                pdcmd.CommandType = CommandType.StoredProcedure;

                if (pdmodel.ProductID == 0 || pdmodel.ProductID == null)
                {
                    pdcmd.CommandText = "PR_Insert_Product";
                }
                else
                {
                    pdcmd.CommandText = "PR_Update_Product";
                    pdcmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = pdmodel.ProductID;
                }
                pdcmd.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = pdmodel.ProductName;
                pdcmd.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = pdmodel.ProductPrice;
                pdcmd.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = pdmodel.ProductCode;
                pdcmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = pdmodel.Description;
                pdcmd.Parameters.Add("@UserID", SqlDbType.Int).Value = pdmodel.UserID;
                pdcmd.ExecuteNonQuery();

                //_emailService.SendEmail("krishgohel173@gmail.com", "Thank you for contacting us!", $"Dear {pdmodel.ProductName},\n\n{pdmodel.ProductCode}\n\n{pdmodel.ProductPrice}\n\nWe will get back to you shortly.");

                //ViewBag.Message = "Thank you for contacting us. An email has been sent to you.";

                return RedirectToAction("ProductTable");
            }
            else
            {
                return View("ProductAddEdit", pdmodel);
            }
        }
        #endregion

        #region CancelBtn
        public IActionResult cancelbtn()
        {
            return RedirectToAction("ProductTable");
        }
        #endregion

        #region ExportToExcelProduct
        public IActionResult ExportToExcel()
        {
            SqlConnection Conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            Conn.Open();

            SqlCommand sqlCommand = Conn.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_SelectAll_Product";
            DataTable dt = new DataTable();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            dt.Load(reader);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // load data into the workshet
                worksheet.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium9);

                //// Add header row
                //worksheet.Cells[1, 1].Value = "ProductID";
                //worksheet.Cells[1, 2].Value = "ProductName";
                //worksheet.Cells[1, 3].Value = "ProductCode";
                //worksheet.Cells[1, 4].Value = "ProductPrice";
                //worksheet.Cells[1, 5].Value = "Description";
                //worksheet.Cells[1, 6].Value = "UserName";

                //// Add data rows
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    worksheet.Cells[i + 2, 1].Value = dt.Rows[i]["ProductID"];
                //    worksheet.Cells[i + 2, 2].Value = dt.Rows[i]["ProductName"];
                //    worksheet.Cells[i + 2, 3].Value = dt.Rows[i]["ProductCode"];
                //    worksheet.Cells[i + 2, 4].Value = dt.Rows[i]["ProductPrice"];
                //    worksheet.Cells[i + 2, 5].Value = dt.Rows[i]["Description"];
                //    worksheet.Cells[i + 2, 6].Value = dt.Rows[i]["UserName"];
                //}

                //generate the excel file
                var stream = new MemoryStream(package.GetAsByteArray());

                // Set auto-fit columns
                //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Return as Excel file
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Product.xlsx");
            }
        }
        #endregion

        #region ExportToPDF
        public IActionResult ExportToPDF()
        {
            SqlConnection Conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            Conn.Open();

            SqlCommand sqlCommand = Conn.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_SelectAll_Product";
            DataTable dt = new DataTable();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            dt.Load(reader);

            var document = new Document();
            using (var memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add a title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                document.Add(new Paragraph("Product List", titleFont));

                // Add some space
                document.Add(new Paragraph(" "));

                // Create a table with 3 columns (Id, Name, Price)
                var table = new PdfPTable(6);
                table.AddCell("ProductID");
                table.AddCell("ProductName");
                table.AddCell("ProductPrice");
                table.AddCell("ProductCode");
                table.AddCell("Description");
                table.AddCell("UserName");

                // Add rows to the table
                foreach (DataRow row in dt.Rows)
                {
                    table.AddCell(Convert.ToInt32(row["ProductID"]).ToString());
                    table.AddCell(row["ProductName"].ToString());
                    table.AddCell(Convert.ToDouble(row["ProductPrice"]).ToString());
                    table.AddCell(row["ProductCode"].ToString());
                    table.AddCell(row["Description"].ToString());
                    table.AddCell(row["UserName"].ToString());
                }

                document.Add(table);

                document.Close();

                // Return the PDF as a byte array
                var bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", "ProductList.pdf");
            }
        }
        #endregion

        public class EmailService
        {
            private readonly SMTPModel _smtpSettings;
            public EmailService(IOptions<SMTPModel> smtpSettings)
            {
                _smtpSettings = smtpSettings.Value;
            }

            public void SendEmail(string to, string subject, string body)
            {
                using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);
                    client.EnableSsl = _smtpSettings.EnableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.From),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(to);

                    client.Send(mailMessage);
                }
            }
        }

        #region EmailService

        [HttpPost]
        public IActionResult SubmitForm(ProductModel pd)
        {
            if (ModelState.IsValid)
            {

            }

            return View("ProductTable");
        }
        #endregion

        #region StaticCrud

        // static CrudCode

        //public IActionResult SaveProduct(ProductModel Pd)
        //{
        //    if (Pd.ProductID == 0)
        //    {
        //        Pd.ProductID = productModels.Max(x => x.ProductID + 1);
        //        productModels.Add(Pd);
        //    }
        //    else
        //    {
        //        int n = productModels.FindIndex(p => p.ProductID == Pd.ProductID);
        //        productModels[n].ProductName = Pd.ProductName;
        //        productModels[n].ProductCode = Pd.ProductCode;
        //        productModels[n].ProductPrice = Pd.ProductPrice;
        //        productModels[n].Description = Pd.Description;
        //        productModels[n].UserId = Pd.UserId;
        //    }
        //    return RedirectToAction("ProductTable");
        //}

        //public IActionResult AddEdit(int ProductId = 0)
        //{
        //    ProductModel pd = new ProductModel();
        //    if (ProductId != 0)
        //    {
        //        ProductModel selectProduct = productModels.Find(p => p.ProductID == ProductId);
        //        pd.ProductID = selectProduct.ProductID;
        //        pd.ProductName = selectProduct.ProductName;
        //        pd.ProductCode = selectProduct.ProductCode;
        //        pd.ProductPrice = selectProduct.ProductPrice;
        //        pd.Description = selectProduct.Description;
        //        pd.UserId = selectProduct.UserId;
        //    }
        //    return View("ProductForm", pd);
        //}
        //public IActionResult DelProduct(int ProductID)
        //{
        //    int n = productModels.FindIndex(p => p.ProductID == ProductID);
        //    productModels.RemoveAt(n);
        //    return RedirectToAction("ProductTable");
        //}
        #endregion
    }
}


