using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Admin3.Bal;

namespace Admin3.Controllers
{
    [CheckAccess]
    public class CustomerController : Controller
    {
        //public static List<CustomerModel> CustModels = new List<CustomerModel>()
        //{
        //    new CustomerModel
        //    {
        //        CustomerID = 1,
        //        CustomerName = "John Doe",
        //        HomeAddress = "123 Elm Street, Springfield, IL",
        //        Email = "john.doe@example.com",
        //        MobileNo = "123-456-7890",
        //        GSTNO = "GSTIN1234567890",
        //        CityName = "Springfield",
        //        PinCode = "62704",
        //        NetAmount = 500.00m,
        //        UserID = 1
        //    },
        //    new CustomerModel
        //    {
        //        CustomerID = 2,
        //        CustomerName = "Jane Smith",
        //        HomeAddress = "456 Oak Avenue, Chicago, IL",
        //        Email = "jane.smith@example.com",
        //        MobileNo = "098-765-4321",
        //        GSTNO = "GSTIN0987654321",
        //        CityName = "Chicago",
        //        PinCode = "60616",
        //        NetAmount = 750.25m,
        //        UserID = 2
        //    },
        //    new CustomerModel
        //    {
        //        CustomerID = 3,
        //        CustomerName = "Robert Brown",
        //        HomeAddress = "789 Pine Road, Evanston, IL",
        //        Email = "robert.brown@example.com",
        //        MobileNo = "555-123-4567",
        //        GSTNO = "GSTIN5551234567",
        //        CityName = "Evanston",
        //        PinCode = "60201",
        //        NetAmount = 320.75m,
        //        UserID = 3
        //    },
        //    new CustomerModel
        //    {
        //        CustomerID = 4,
        //        CustomerName = "Emily Davis",
        //        HomeAddress = "101 Maple Street, Naperville, IL",
        //        Email = "emily.davis@example.com",
        //        MobileNo = "444-321-6547",
        //        GSTNO = "GSTIN4443216547",
        //        CityName = "Naperville",
        //        PinCode = "60540",
        //        NetAmount = 420.00m,
        //        UserID = 4
        //    },
        //    new CustomerModel
        //    {
        //        CustomerID = 5,
        //        CustomerName = "Michael Johnson",
        //        HomeAddress = "202 Birch Lane, Aurora, IL",
        //        Email = "michael.johnson@example.com",
        //        MobileNo = "333-456-7890",
        //        GSTNO = "GSTIN3334567890",
        //        CityName = "Aurora",
        //        PinCode = "60506",
        //        NetAmount = 275.40m,
        //        UserID = 5
        //    }
        //};
        private IConfiguration configuration;

        #region ConnectionConstructor
        public CustomerController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region CustomerTable
        public IActionResult CustomerTable()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand Ccmd = conn.CreateCommand();
            Ccmd.CommandType = System.Data.CommandType.StoredProcedure;
            Ccmd.CommandText = "PR_SelectAll_Customer";
            SqlDataReader reader = Ccmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View(dt);
        }
        #endregion

        #region CustomerForm
        public IActionResult CustomerForm()
        {
            return View();
        }
        #endregion

        #region DeleteCustomer
        public IActionResult DelCust(int id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand Ccmd = conn.CreateCommand();
                Ccmd.CommandType = CommandType.StoredProcedure;
                Ccmd.CommandText = "PR_Delete_Customer";
                Ccmd.Parameters.AddWithValue("@CustomerID", id);
                Ccmd.ExecuteNonQuery();
                conn.Close();

                TempData["Message"] = "Customer Deleted Successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("CustomerTable");

        }
        #endregion

        #region CustomerAddEditPageRender
        public IActionResult CustomerAddEdit(int CustomerID = 0)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand ccmd = conn.CreateCommand();
            ccmd.CommandType = CommandType.StoredProcedure;
            ccmd.CommandText = "PR_DropDown_Users";
            SqlDataReader reader = ccmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            List<UserDropDownModel> ulist = new List<UserDropDownModel>();

            foreach (DataRow row in dt.Rows)
            {
                UserDropDownModel ud = new UserDropDownModel();
                ud.UserID = Convert.ToInt32(row["UserID"]);
                ud.UserName = row["UserName"].ToString();
                ulist.Add(ud);
            }

            ViewBag.userlist = ulist;

            if (CustomerID != 0 || CustomerID != null)
            {
                SqlConnection conn1 = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn1.Open();

                SqlCommand ccmd1 = conn1.CreateCommand();
                ccmd1.CommandType = CommandType.StoredProcedure;
                ccmd1.CommandText = "PR_SelectByPK_Customer";
                ccmd1.Parameters.AddWithValue("@CustomerID", CustomerID);
                ccmd1.ExecuteNonQuery();
                SqlDataReader reader1 = ccmd1.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Load(reader1);
                conn1.Close();

                CustomerModel c = new CustomerModel();

                foreach (DataRow row1 in dt1.Rows)
                {
                    c.CustomerID = Convert.ToInt32(row1["CustomerID"]);
                    c.CustomerName = row1["CustomerName"].ToString();
                    c.HomeAddress = row1["HomeAddress"].ToString();
                    c.Email = row1["Email"].ToString();
                    c.MobileNO = row1["MobileNo"].ToString();
                    c.GST_NO = row1["GST_NO"].ToString();
                    c.CityName = row1["CityName"].ToString();
                    c.PinCode = row1["PinCode"].ToString();
                    c.NetAmount = Convert.ToDecimal(row1["NetAmount"]);
                    c.UserID = Convert.ToInt32(row1["UserID"]);
                }
                return View("CustomerForm", c);
            }
            return View("CustomerForm");
        }
        #endregion

        #region CustomerSave
        public IActionResult CustomerSave(CustomerModel Cmodel)
        {
            if (Cmodel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "a valid userid is required.");
            }

            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand ccmd = conn.CreateCommand();
            ccmd.CommandType = CommandType.StoredProcedure;

            if (ModelState.IsValid)
            {
                if (Cmodel.CustomerID == 0 || Cmodel.CustomerID == null)
                {
                    ccmd.CommandText = "PR_Insert_Customer";
                }
                else
                {
                    ccmd.CommandText = "PR_Update_Customer";
                    ccmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = Cmodel.CustomerID;
                }
                ccmd.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = Cmodel.CustomerName;
                ccmd.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = Cmodel.HomeAddress;
                ccmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Cmodel.Email;
                ccmd.Parameters.Add("@MobileNO", SqlDbType.VarChar).Value = Cmodel.MobileNO;
                ccmd.Parameters.Add("@GST_NO", SqlDbType.VarChar).Value = Cmodel.GST_NO;
                ccmd.Parameters.Add("@CityName", SqlDbType.VarChar).Value = Cmodel.CityName;
                ccmd.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = Cmodel.PinCode;
                ccmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = Cmodel.NetAmount;
                ccmd.Parameters.Add("@UserID", SqlDbType.Int).Value = Cmodel.UserID;
                ccmd.ExecuteNonQuery();
                conn.Close();
                return RedirectToAction("CustomerTable");
            }
            else
            {
                return View("CustomerAddEdit",Cmodel);
            }
        }
        #endregion

        #region CancelBtn
        public IActionResult cancelbtn()
        {
            return RedirectToAction("CustomerTable");
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand Ccmd = conn.CreateCommand();
            Ccmd.CommandType = System.Data.CommandType.StoredProcedure;
            Ccmd.CommandText = "PR_SelectAll_Customer";
            SqlDataReader reader = Ccmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells["A1"].LoadFromDataTable(dt,true,OfficeOpenXml.Table.TableStyles.Dark4);

                //worksheet.Cells[1, 1].Value = "CustomerID";
                //worksheet.Cells[1, 2].Value = "CustomerName";
                //worksheet.Cells[1, 3].Value = "HomeAddress";
                //worksheet.Cells[1, 4].Value = "Email";
                //worksheet.Cells[1, 5].Value = "MobileNO";
                //worksheet.Cells[1, 6].Value = "GST_NO";
                //worksheet.Cells[1, 7].Value = "CityName";
                //worksheet.Cells[1, 8].Value = "PinCode";
                //worksheet.Cells[1, 9].Value = "NetAmount";
                //worksheet.Cells[1, 10].Value = "UserName";

                //int i = 1;

                //foreach (DataRow row in dt.Rows) 
                //{
                //    worksheet.Cells[i + 1, 1].Value = row["CustomerID"];
                //    worksheet.Cells[i + 1, 2].Value = row["CustomerName"];
                //    worksheet.Cells[i + 1, 3].Value = row["HomeAddress"];
                //    worksheet.Cells[i + 1, 4].Value = row["Email"];
                //    worksheet.Cells[i + 1, 5].Value = row["MobileNO"];
                //    worksheet.Cells[i + 1, 6].Value = row["GST_NO"];
                //    worksheet.Cells[i + 1, 7].Value = row["CityName"];
                //    worksheet.Cells[i + 1, 8].Value = row["PinCode"];
                //    worksheet.Cells[i + 1, 9].Value = row["NetAmount"];
                //    worksheet.Cells[i + 1, 10].Value = row["UserName"];

                //    i++;
                //}

                //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");

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
            billcmd.CommandText = "PR_SelectAll_Customer";
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
                var table = new PdfPTable(10);
                table.AddCell("CustomerID");
                table.AddCell("CustomerName");
                table.AddCell("HomeAddress");
                table.AddCell("Email");
                table.AddCell("MobileNO");
                table.AddCell("GST_NO");
                table.AddCell("CityName");
                table.AddCell("PinCode");
                table.AddCell("NetAmount");
                table.AddCell("UserName");

                // Add rows to the table
                foreach (DataRow row in dt.Rows)
                {
                    table.AddCell(Convert.ToInt32(row["CustomerID"]).ToString());
                    table.AddCell(row["CustomerName"].ToString());
                    table.AddCell(row["HomeAddress"].ToString());
                    table.AddCell(row["Email"].ToString());
                    table.AddCell(row["MobileNO"].ToString());
                    table.AddCell(row["GST_NO"].ToString());
                    table.AddCell(row["CityName"].ToString());
                    table.AddCell(row["PinCode"].ToString());
                    table.AddCell(row["NetAmount"].ToString());
                    table.AddCell(row["UserName"].ToString());
                }

                document.Add(table);

                document.Close();

                // Return the PDF as a byte array
                var bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", "CustomerList.pdf");
            }
        }
        #endregion

        #region StaticCrud
        // Static Crud

        //public IActionResult SaveCustomer(CustomerModel custmodel)
        //{
        //    if (custmodel.CustomerID == 0) { 
        //        custmodel.CustomerID = CustModels.Max(c => c.CustomerID + 1);
        //        CustModels.Add(custmodel);
        //    }
        //    else
        //    {
        //        CustModels[custmodel.CustomerID - 1].CustomerName = custmodel.CustomerName ;
        //        CustModels[custmodel.CustomerID - 1].HomeAddress = custmodel.HomeAddress;
        //        CustModels[custmodel.CustomerID - 1].Email = custmodel.Email;
        //        CustModels[custmodel.CustomerID - 1].MobileNo = custmodel.MobileNo;
        //        CustModels[custmodel.CustomerID - 1].GSTNO = custmodel.GSTNO;
        //        CustModels[custmodel.CustomerID - 1].CityName = custmodel.CityName;
        //        CustModels[custmodel.CustomerID - 1].PinCode = custmodel.PinCode;
        //        CustModels[custmodel.CustomerID - 1].NetAmount = custmodel.NetAmount;
        //        CustModels[custmodel.CustomerID - 1].UserID = custmodel.UserID;
        //    }
        //    return RedirectToAction("CustomerTable");
        //}

        //public IActionResult AddEdit(int CustomerID = 0)
        //{
        //    CustomerModel c = new CustomerModel();
        //    if(CustomerID != 0)
        //    {
        //        var cc = CustModels.Find(C =>  C.CustomerID == CustomerID);
        //        c.CustomerID = cc.CustomerID;
        //        c.CustomerName = cc.CustomerName;
        //        c.HomeAddress = cc.HomeAddress;
        //        c.Email = cc.Email;
        //        c.MobileNo = cc.MobileNo;
        //        c.GSTNO = cc.GSTNO;
        //        c.CityName = cc.CityName;
        //        c.PinCode = cc.PinCode;
        //        c.NetAmount = cc.NetAmount;
        //        c.UserID = cc.UserID;
        //    }
        //    return View("CustomerForm", c);
        //}
        //public IActionResult DelCust(int CustomerID)
        //{
        //    int n = CustModels.FindIndex(c => c.CustomerID == CustomerID);
        //    CustModels.RemoveAt(n);
        //    return RedirectToAction("CustomerTable");
        //}
        #endregion
    }
}
