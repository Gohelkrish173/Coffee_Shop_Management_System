using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using System.Security.Cryptography.Xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Admin3.Bal;

namespace Admin3.Controllers
{
    [CheckAccess]
    public class UserController : Controller
    {
        #region StaticData
        //public static List<UserModel> UserModels = new List<UserModel>()
        //{
        //    new UserModel { UserID = 1, UserName = "Alice", Email = "alice@example.com", Password = "password1", MobileNo = "1234567890", Address = "123 Main St", IsActive = true },
        //    new UserModel { UserID = 2, UserName = "Bob", Email = "bob@example.com", Password = "password2", MobileNo = "1234567891", Address = "124 Main St", IsActive = true },
        //    new UserModel { UserID = 3, UserName = "Charlie", Email = "charlie@example.com", Password = "password3", MobileNo = "1234567892", Address = "125 Main St", IsActive = true },
        //    new UserModel { UserID = 4, UserName = "David", Email = "david@example.com", Password = "password4", MobileNo = "1234567893", Address = "126 Main St", IsActive = true },
        //    new UserModel { UserID = 5, UserName = "Eva", Email = "eva@example.com", Password = "password5", MobileNo = "1234567894", Address = "127 Main St", IsActive = true },
        //    new UserModel { UserID = 6, UserName = "Frank", Email = "frank@example.com", Password = "password6", MobileNo = "1234567895", Address = "128 Main St", IsActive = true },
        //    new UserModel { UserID = 7, UserName = "Grace", Email = "grace@example.com", Password = "password7", MobileNo = "1234567896", Address = "129 Main St", IsActive = true },
        //    new UserModel { UserID = 8, UserName = "Hank", Email = "hank@example.com", Password = "password8", MobileNo = "1234567897", Address = "130 Main St", IsActive = true },
        //    new UserModel { UserID = 9, UserName = "Ivy", Email = "ivy@example.com", Password = "password9", MobileNo = "1234567898", Address = "131 Main St", IsActive = true },
        //    new UserModel { UserID = 10, UserName = "Jack", Email = "jack@example.com", Password = "password10", MobileNo = "1234567899", Address = "132 Main St", IsActive = true }
        //};
        #endregion

        private IConfiguration configuration;

        #region ConfigrationConnection
        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region UserTable
        public IActionResult UserTable()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand Ucmd = conn.CreateCommand();
            Ucmd.CommandType = System.Data.CommandType.StoredProcedure;
            Ucmd.CommandText = "PR_SelectAll_Users";
            SqlDataReader reader = Ucmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            return View(dt);
        }
        #endregion

        #region UserForm
        public IActionResult UserForm()
        {
            return View();
        }
        #endregion

        #region DeleteUser
        public IActionResult DelUser(int id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand Ucmd = conn.CreateCommand();
                Ucmd.CommandType = CommandType.StoredProcedure;
                Ucmd.CommandText = "PR_Delete_Users";
                Ucmd.Parameters.AddWithValue("@UserID", id);
                Ucmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }
            return RedirectToAction("UserTable");
        }
        #endregion

        #region UserAddEditRender

        public IActionResult UserAddEdit(int UserID = 0)
        {
            if (UserID != 0)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand Ucmd = conn.CreateCommand();
                Ucmd.CommandType = CommandType.StoredProcedure;
                Ucmd.CommandText = "PR_SelectByPK_Users";
                Ucmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader reader = Ucmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();

                UserModel umodel = new UserModel();

                foreach (DataRow dr in dt.Rows)
                {
                    umodel.UserID = Convert.ToInt32(dr["UserID"]);
                    umodel.UserName = dr["UserName"].ToString();
                    umodel.Email = dr["Email"].ToString();
                    umodel.Address = dr["Address"].ToString();
                    umodel.Password = dr["Password"].ToString();
                    umodel.MobileNo = dr["MobileNO"].ToString();
                    umodel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
                return View("UserForm", umodel);
            }
            return View("UserForm");
        }

        #endregion

        #region UserSave
        public IActionResult UserSave(UserModel umodel)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand Ucmd = conn.CreateCommand();
            Ucmd.CommandType = CommandType.StoredProcedure;

            if (ModelState.IsValid)
            {
                if (umodel.UserID == 0 || umodel.UserID == null)
                {
                    Ucmd.CommandText = "PR_Insert_Users";
                }
                else
                {
                    Ucmd.CommandText = "PR_Update_Users";
                    Ucmd.Parameters.Add("@UserID", SqlDbType.Int).Value = umodel.UserID;
                }
                Ucmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = umodel.UserName;
                Ucmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = umodel.Email;
                Ucmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = umodel.Password;
                Ucmd.Parameters.Add("@MobileNo", SqlDbType.Decimal).Value = umodel.MobileNo;
                Ucmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = umodel.Address;
                Ucmd.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = umodel.IsActive;
                Ucmd.ExecuteNonQuery();

                return RedirectToAction("UserTable");
            }
            else
            {
                return View("UserAddEdit", umodel);
            }
        }
        #endregion

        #region CancelBtn
        public IActionResult cancelbtn()
        {
            return RedirectToAction("UserTable");
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand Ucmd = conn.CreateCommand();
            Ucmd.CommandType = System.Data.CommandType.StoredProcedure;
            Ucmd.CommandText = "PR_SelectAll_Users";
            SqlDataReader reader = Ucmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage()) 
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium1);

                //worksheet.Cells[1, 1].Value = "UserID";
                //worksheet.Cells[1, 2].Value = "UserName";
                //worksheet.Cells[1, 3].Value = "Email";
                //worksheet.Cells[1, 4].Value = "Password";
                //worksheet.Cells[1, 5].Value = "MobileNO";
                //worksheet.Cells[1, 6].Value = "Address";
                //worksheet.Cells[1, 7].Value = "IsActive";

                //int i = 1;

                //foreach(DataRow row in dt.Rows)
                //{
                //    worksheet.Cells[i + 1, 1].Value = row["UserID"];
                //    worksheet.Cells[i + 1, 2].Value = row["UserName"];
                //    worksheet.Cells[i + 1, 3].Value = row["Email"];
                //    worksheet.Cells[i + 1, 4].Value = row["Password"];
                //    worksheet.Cells[i + 1, 5].Value = row["MobileNO"];
                //    worksheet.Cells[i + 1, 6].Value = row["Address"];
                //    worksheet.Cells[i + 1, 7].Value = row["IsActive"];
                //    i = i + 1;
                //}

                // set auto-fit columns
                //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //convert to byte array
                //var fileContents = package.GetAsByteArray();

                var stream = new MemoryStream(package.GetAsByteArray());

                // return excel file
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","UserDataTable.xlsx");
            }
        }
        #endregion

        #region ExportToPDF
        public IActionResult ExportToPDF()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand billcmd = conn.CreateCommand();
            billcmd.CommandType = System.Data.CommandType.StoredProcedure;
            billcmd.CommandText = "PR_SelectAll_Users";
            SqlDataReader reader = billcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            var document = new iTextSharp.text.Document();
            using (var memoryStream = new MemoryStream())
            {
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add a title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                document.Add(new Paragraph("Customer List", titleFont));

                // Add some space
                document.Add(new Paragraph(" "));

                // Create a table with 3 columns (Id, Name, Price)
                var table = new PdfPTable(7);
                table.AddCell("UserID");
                table.AddCell("UserName");
                table.AddCell("Email");
                table.AddCell("Password");
                table.AddCell("MobileNO");
                table.AddCell("Address");
                table.AddCell("IsActive");

                // Add rows to the table
                foreach (DataRow row in dt.Rows)
                {
                    table.AddCell(Convert.ToInt32(row["UserID"]).ToString());
                    table.AddCell(row["UserName"].ToString());
                    table.AddCell(row["Email"].ToString());
                    table.AddCell(row["Password"].ToString());
                    table.AddCell(row["MobileNO"].ToString());
                    table.AddCell(row["Address"].ToString());
                    table.AddCell(row["IsActive"].ToString());
                }

                document.Add(table);

                document.Close();

                // Return the PDF as a byte array
                var bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", "UserList.pdf");
            }
        }
        #endregion

        #region ExcelImport

        [HttpPost]
        public IActionResult ImportFromExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Save the uploaded file to a temporary location
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Load the data from the Excel file
                DataTable dataTable = LoadDataFromExcel(filePath);

                // Insert data into the Users table
                InsertDataIntoDatabase(dataTable);

                // Delete the temporary file
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction("UserTable");
        }

        #endregion

        #region LoadExcel
        private DataTable LoadDataFromExcel(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                DataTable dt = new DataTable();

                // Assuming the first row contains column names
                for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
                {
                    dt.Columns.Add(worksheet.Cells[1, i].Text);
                }

                // Start loading data from the second row
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    dt.Rows.Add(dataRow);
                }

                return dt;
            }
        }
        #endregion

        #region InsertInSql
        private void InsertDataIntoDatabase(DataTable dataTable)
        {
            using (SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection")))
            {
                conn.Open();
                foreach (DataRow row in dataTable.Rows)
                {
                    string query = "INSERT INTO Users (UserName, Email, Password, MobileNO, Address, IsActive) " +
                                   "VALUES (@UserName, @Email, @Password, @MobileNO, @Address, @IsActive)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", row["UserName"]);
                        cmd.Parameters.AddWithValue("@Email", row["Email"]);
                        cmd.Parameters.AddWithValue("@Password", row["Password"]);
                        cmd.Parameters.AddWithValue("@MobileNO", row["MobileNO"]);
                        cmd.Parameters.AddWithValue("@Address", row["Address"]);
                        cmd.Parameters.AddWithValue("@IsActive", row["IsActive"]);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        #endregion

        #region Static Crud
        // Static Crud

        //public IActionResult SaveUser(UserModel UM)
        //{
        //    if(UM.UserID == 0)
        //    {
        //        UM.UserID = UserModels.Max(u => u.UserID+1);
        //        UserModels.Add(UM);
        //    }
        //    else
        //    {
        //        UserModels[UM.UserID - 1].UserName = UM.UserName;
        //        UserModels[UM.UserID - 1].Email = UM.Email;
        //        UserModels[UM.UserID - 1].Password = UM.Password;
        //        UserModels[UM.UserID - 1].MobileNo = UM.MobileNo;
        //        UserModels[UM.UserID - 1].Address = UM.Address;
        //        UserModels[UM.UserID - 1].IsActive = UM.IsActive;
        //    }
        //    return RedirectToAction("UserTable");
        //}

        //public IActionResult AddEdit(int UserID = 0)
        //{
        //    UserModel u = new UserModel();
        //    if(UserID != 0)
        //    {
        //       UserModel obj = UserModels.Find(ui =>  ui.UserID == UserID);

        //        u.UserID = obj.UserID;
        //        u.UserName = obj.UserName;
        //        u.Email = obj.Email;
        //        u.Password = obj.Password;
        //        u.MobileNo = obj.MobileNo;
        //        u.Address = obj.Address;
        //        u.IsActive = obj.IsActive;
        //    }
        //    return View("UserForm", u);
        //}
        //public IActionResult DelUser(int UserID)
        //{ 
        //    int n = UserModels.FindIndex(ui =>  ui.UserID == UserID);
        //    UserModels.RemoveAt(n);
        //    return RedirectToAction("UserTable");
        //}
        #endregion
    }
}
