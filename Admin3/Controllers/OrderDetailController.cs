using Admin3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using OfficeOpenXml;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Admin3.Bal;

namespace Admin3.Controllers
{
    [CheckAccess]
    public class OrderDetailController : Controller
    {
        #region StaticData
        //public static List<OrderDetailModel> ODModels = new List<OrderDetailModel>()
        //{
        //    new OrderDetailModel
        //    {
        //        OrderDetailID = 1,
        //        OrderID = 1,
        //        ProductID = 1001,
        //        Quantity = 2,
        //        Amount = 50.00m,
        //        TotalAmount = 100.00m,
        //        UserID = 1
        //    },
        //    new OrderDetailModel
        //    {
        //        OrderDetailID = 2,
        //        OrderID = 1,
        //        ProductID = 1002,
        //        Quantity = 1,
        //        Amount = 150.99m,
        //        TotalAmount = 150.99m,
        //        UserID = 1
        //    },
        //    new OrderDetailModel
        //    {
        //        OrderDetailID = 3,
        //        OrderID = 2,
        //        ProductID = 1003,
        //        Quantity = 3,
        //        Amount = 40.00m,
        //        TotalAmount = 120.00m,
        //        UserID = 2
        //    },
        //    new OrderDetailModel
        //    {
        //        OrderDetailID = 4,
        //        OrderID = 3,
        //        ProductID = 1004,
        //        Quantity = 5,
        //        Amount = 20.00m,
        //        TotalAmount = 100.00m,
        //        UserID = 3
        //    },
        //    new OrderDetailModel
        //    {
        //        OrderDetailID = 5,
        //        OrderID = 4,
        //        ProductID = 1005,
        //        Quantity = 4,
        //        Amount = 25.00m,
        //        TotalAmount = 100.00m,
        //        UserID = 4
        //    }
        //};
        #endregion

        private IConfiguration configuration;

        #region ConfigurationConstructor

        public OrderDetailController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region OrderDetailTable
        public IActionResult OrderDetailTable()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand ODcmd = conn.CreateCommand();
            ODcmd.CommandType = System.Data.CommandType.StoredProcedure;
            ODcmd.CommandText = "PR_SelectAll_OrderDetail";
            SqlDataReader reader = ODcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View(dt);
        }
        #endregion

        #region OrderDetailForm
        public IActionResult ODForm()
        {
            return View();
        }
        #endregion

        #region DeleteOrderDetail
        public IActionResult DelOD(int id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand ODcmd = conn.CreateCommand();
                ODcmd.CommandType = CommandType.StoredProcedure;
                ODcmd.CommandText = "PR_Delete_OrderDetail";
                ODcmd.Parameters.AddWithValue("@OrderDetailID", id);
                ODcmd.ExecuteNonQuery();
                conn.Close();

                TempData["Message"] = "OrderDetail Deleted Successfully.";
            }
            catch (Exception ex) {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("OrderDetailTable");
        }
        #endregion

        #region DropDownTable

        public List<OrderDropDownModel> OModel()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "PR_DropDown_Order";
            SqlDataReader data = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(data);

            List<OrderDropDownModel> oddm = new List<OrderDropDownModel>();

            foreach (DataRow dr in dt.Rows)
            {
                OrderDropDownModel omodel = new OrderDropDownModel();
                omodel.OrderID = Convert.ToInt32(dr["OrderID"]);
                omodel.OrderNO = dr["OrderNO"].ToString();
                oddm.Add(omodel);
            }
            return oddm;
        }

        public List<ProductDropDownModel> PModel()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_DropDown_Product";
            SqlDataReader data1 = cmd.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(data1);

            List<ProductDropDownModel> pddm = new List<ProductDropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                ProductDropDownModel pmodel = new ProductDropDownModel();
                pmodel.ProductID = Convert.ToInt32(dr["ProductID"]);
                pmodel.ProductName = dr["ProductName"].ToString();
                pddm.Add(pmodel);
            }
            return pddm;
        }

        public List<UserDropDownModel> UModel()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_DropDown_Users";
            SqlDataReader data2 = cmd.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(data2);

            List<UserDropDownModel> uddm = new List<UserDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                UserDropDownModel umodel = new UserDropDownModel();
                umodel.UserID = Convert.ToInt32(dr["UserID"]);
                umodel.UserName = dr["UserName"].ToString();
                uddm.Add(umodel);
            }

            return uddm;

        }
        #endregion

        #region SelectByPKData
        public OrderDetailModel ODModel(int OrderDetailID)
        {
            SqlConnection conn1 = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn1.Open();

            SqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_SelectByPK_OrderDetail";
            cmd1.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
            cmd1.ExecuteNonQuery();
            SqlDataReader data3 = cmd1.ExecuteReader();
            DataTable dt3 = new DataTable();
            dt3.Load(data3);
            conn1.Close();

            OrderDetailModel odm = new OrderDetailModel();

            foreach (DataRow i in dt3.Rows)
            {
                odm.OrderDetailID = Convert.ToInt32(i["OrderDetailID"]);
                odm.OrderID = Convert.ToInt32(i["OrderID"]);
                odm.ProductID = Convert.ToInt32(i["ProductID"]);
                odm.Quantity = Convert.ToInt32(i["Quantity"]);
                odm.Amount = Convert.ToDecimal(i["Amount"]);
                odm.TotalAmount = Convert.ToDecimal(i["TotalAmount"]);
                odm.UserID = Convert.ToInt32(i["UserID"]);
            }

            return odm;
        }
        #endregion

        #region OrderDetailAddEdit
        public IActionResult OrderDetailAddEdit(int OrderDetailID = 0)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            ViewBag.OrderList = OModel();
            ViewBag.ProductList = PModel();
            ViewBag.UserList = UModel();
            conn.Close();

            if (OrderDetailID != 0)
            {
                return View("ODForm", ODModel(OrderDetailID));
            }
            return View("ODForm");
        }
        #endregion

        #region OrderDetailSave
        public IActionResult OrderDetialSave(OrderDetailModel odmodel)
        {
            if (odmodel.OrderID <= 0 && odmodel.OrderID == null)
            {
                ModelState.AddModelError("OrderID", "Invalid OrderID");
            }

            if (odmodel.ProductID <= 0 && odmodel.ProductID == null)
            {
                ModelState.AddModelError("ProductID", "Invalid ProductID");
            }

            if (odmodel.UserID <= 0 && odmodel.UserID == null)
            {
                ModelState.AddModelError("UserID", "Invalid UserID");
            }

            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                if (odmodel.OrderDetailID == 0 || odmodel.OrderDetailID == null)
                {
                    cmd.CommandText = "PR_Insert_OrderDetail";
                }
                else
                {
                    cmd.CommandText = "PR_Update_OrderDetail";
                    cmd.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = odmodel.OrderDetailID;
                }
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = odmodel.OrderID;
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = odmodel.ProductID;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = odmodel.Quantity;
                cmd.Parameters.Add("@Amount", SqlDbType.Int).Value = odmodel.Amount;
                cmd.Parameters.Add("@TotalAmount", SqlDbType.Int).Value = odmodel.TotalAmount;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = odmodel.UserID;
                cmd.ExecuteNonQuery();
                conn.Close();

                return RedirectToAction("OrderDetailTable");
            }
            else
            {
                return View("OrderDetailAddEdit", odmodel);
            }
        }
        #endregion

        #region CancelBtn
        public IActionResult cancelbtn()
        {
            return RedirectToAction("OrderDetailTable");
        }
        #endregion

        #region EmportTabletoExcel
        public IActionResult ExportToExcel()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand ODcmd = conn.CreateCommand();
            ODcmd.CommandType = System.Data.CommandType.StoredProcedure;
            ODcmd.CommandText = "PR_SelectAll_OrderDetail";
            SqlDataReader reader = ODcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true,OfficeOpenXml.Table.TableStyles.Dark10);

                // Add header row
                //worksheet.Cells[1, 1].Value = "OrderDetailID";
                //worksheet.Cells[1, 2].Value = "OrderNO";
                //worksheet.Cells[1, 3].Value = "ProductName";
                //worksheet.Cells[1, 4].Value = "Quantity";
                //worksheet.Cells[1, 5].Value = "Amount";
                //worksheet.Cells[1, 6].Value = "TotalAmount";
                //worksheet.Cells[1, 7].Value = "UserName";

                // Add data rows
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    worksheet.Cells[i + 2, 1].Value = dt.Rows[i]["OrderDetailID"];
                //    worksheet.Cells[i + 2, 2].Value = dt.Rows[i]["OrderNO"];
                //    worksheet.Cells[i + 2, 3].Value = dt.Rows[i]["ProductName"];
                //    worksheet.Cells[i + 2, 4].Value = dt.Rows[i]["Quantity"];
                //    worksheet.Cells[i + 2, 5].Value = dt.Rows[i]["Amount"];
                //    worksheet.Cells[i + 2, 6].Value = dt.Rows[i]["TotalAmount"];
                //    worksheet.Cells[i + 2, 7].Value = dt.Rows[i]["UserName"];
                //}

                // Set auto-fit columns
                //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Convert to byte array
                //var fileContents = package.GetAsByteArray();

                var stream = new MemoryStream(package.GetAsByteArray());

                // Return as Excel file
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderDetail.xlsx");
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
            billcmd.CommandText = "PR_SelectAll_OrderDetail";
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
                document.Add(new Paragraph("Order List", titleFont));

                // Add some space
                document.Add(new Paragraph(" "));

                // Create a table with 3 columns (Id, Name, Price)
                var table = new PdfPTable(7);
                table.AddCell("OrderDetailID");
                table.AddCell("OrderNO");
                table.AddCell("ProductName");
                table.AddCell("Quantity");
                table.AddCell("Amount");
                table.AddCell("TotalAmount");
                table.AddCell("UserName");

                // Add rows to the table
                foreach (DataRow row in dt.Rows)
                {
                    table.AddCell(Convert.ToInt32(row["OrderDetailID"]).ToString());
                    table.AddCell(row["OrderNO"].ToString());
                    table.AddCell(row["ProductName"].ToString());
                    table.AddCell(row["Quantity"].ToString());
                    table.AddCell(row["Amount"].ToString());
                    table.AddCell(row["TotalAmount"].ToString());
                    table.AddCell(row["UserName"].ToString());
                }

                document.Add(table);

                document.Close();

                // Return the PDF as a byte array
                var bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", "OrderDetailList.pdf");
            }
        }
        #endregion

        #region StaticCrud

        // Static Crud

        //public IActionResult SaveOrderDetial(OrderDetailModel odModel)
        //{
        //    if(odModel.OrderDetailID == 0)
        //    {
        //        odModel.OrderDetailID = ODModels.Max(od => od.OrderDetailID + 1);
        //        ODModels.Add(odModel);
        //    }
        //    else
        //    {
        //        ODModels[odModel.OrderDetailID -1].OrderID = odModel.OrderDetailID;
        //        ODModels[odModel.OrderDetailID -1].ProductID = odModel.ProductID;
        //        ODModels[odModel.OrderDetailID - 1].Quantity = odModel.Quantity;
        //        ODModels[odModel.OrderDetailID - 1].Amount = odModel.Amount;
        //        ODModels[odModel.OrderDetailID - 1].TotalAmount = odModel.TotalAmount;
        //        ODModels[odModel.OrderDetailID - 1].UserID = odModel.UserID;
        //    }
        //    return RedirectToAction("OrderDetailTable");
        //}

        //public IActionResult AddEdit(int OrderDetailID = 0)
        //{
        //    OrderDetailModel odd = new OrderDetailModel();
        //    if(odd.OrderDetailID != 0)
        //    {
        //        OrderDetailModel od = ODModels.Find(o => o.OrderDetailID == OrderDetailID);
        //        Console.WriteLine(od);
        //        odd.OrderDetailID = od.OrderDetailID;
        //        odd.OrderID = od.OrderID;
        //        odd.ProductID = od.ProductID;
        //        odd.Quantity = od.Quantity;
        //        odd.Amount = od.Amount;
        //        odd.TotalAmount = od.TotalAmount;
        //        odd.UserID = od.UserID;
        //    }
        //    return View("OrderDetailForm", odd);
        //}
        //public IActionResult DelOD(int OrderDetailID) 
        //{
        //    int n = ODModels.FindIndex(ood => ood.OrderDetailID == OrderDetailID);
        //    ODModels.RemoveAt(n);
        //    return RedirectToAction("OrderDetailTable");
        //}
        #endregion

    }
}
