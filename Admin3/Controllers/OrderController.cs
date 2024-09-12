using Admin3.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using Admin3.Bal;

namespace Admin3.Controllers
{
    [CheckAccess]
    public class OrderController : Controller
    {
        #region StaticData
        //    public static List<OrderModel> OrderModelList = new List<OrderModel>()
        //{
        //        new OrderModel
        //        {
        //            OrderId = 1,
        //            OrderDate = DateTime.Now.AddDays(-10),
        //            CustomerID = 101,
        //            PaymentMode = "Credit Card",
        //            TotalAmount = 250.99m,
        //            ShippingAddress = "123 Elm Street, Springfield, IL",
        //            UserID = 1
        //        },
        //        new OrderModel
        //        {
        //            OrderId = 2,
        //            OrderDate = DateTime.Now.AddDays(-8),
        //            CustomerID = 102,
        //            PaymentMode = "PayPal",
        //            TotalAmount = 320.50m,
        //            ShippingAddress = "456 Oak Avenue, Chicago, IL",
        //            UserID = 2
        //        },
        //        new OrderModel
        //        {
        //            OrderId = 3,
        //            OrderDate = DateTime.Now.AddDays(-6),
        //            CustomerID = 103,
        //            PaymentMode = "Cash on Delivery",
        //            TotalAmount = 150.00m,
        //            ShippingAddress = "789 Pine Road, Evanston, IL",
        //            UserID = 3
        //        },
        //        new OrderModel
        //        {
        //            OrderId = 4,
        //            OrderDate = DateTime.Now.AddDays(-4),
        //            CustomerID = 104,
        //            PaymentMode = "Debit Card",
        //            TotalAmount = 400.75m,
        //            ShippingAddress = "101 Maple Street, Naperville, IL",
        //            UserID = 4
        //        },
        //        new OrderModel
        //        {
        //            OrderId = 5,
        //            OrderDate = DateTime.Now.AddDays(-2),
        //            CustomerID = 105,
        //            PaymentMode = "Bank Transfer",
        //            TotalAmount = 275.40m,
        //            ShippingAddress = "202 Birch Lane, Aurora, IL",
        //            UserID = 5
        //        }
        //    };
        #endregion
        private IConfiguration configuration;

        #region ConfigurationConstructor
        public OrderController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region OrderTable
        public IActionResult OrderTable()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand ocmd = conn.CreateCommand();
            ocmd.CommandType = System.Data.CommandType.StoredProcedure;
            ocmd.CommandText = "PR_SelectAll_Orders";
            SqlDataReader reader = ocmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View(dt);
        }
        #endregion

        #region OrderForm
        public IActionResult OrderForm()
        {
            return View();
        }
        #endregion

        #region DeleteOrder
        public IActionResult DelOrder(int id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand ocmd = conn.CreateCommand();
                ocmd.CommandType = CommandType.StoredProcedure;
                ocmd.CommandText = "PR_Delete_Order";
                ocmd.Parameters.AddWithValue("@OrderId", id);
                ocmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = ex.Message;
            }

            return RedirectToAction("OrderTable");
        }
        #endregion

        #region OrderAddEdit
        public IActionResult OrderAddEdit(int OrderID = 0)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand ocmd = conn.CreateCommand();
            ocmd.CommandType = CommandType.StoredProcedure;
            ocmd.CommandText = "PR_DropDown_Customer";
            SqlDataReader data1 = ocmd.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(data1);
            ocmd.CommandText = "PR_DropDown_Users";
            SqlDataReader data2 = ocmd.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(data2);
            conn.Close();

            List<CustomerDropDownModel> custlist = new List<CustomerDropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                CustomerDropDownModel cmodel = new CustomerDropDownModel();
                cmodel.CustomerID = Convert.ToInt32(dr["CustomerID"]);
                cmodel.CustomerName = dr["CustomerName"].ToString();
                custlist.Add(cmodel);
            }

            ViewBag.CList = custlist;

            List<UserDropDownModel> userlist = new List<UserDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                UserDropDownModel umodel = new UserDropDownModel();
                umodel.UserID = Convert.ToInt32(dr["UserID"]);
                umodel.UserName = dr["UserName"].ToString();
                userlist.Add(umodel);
            }

            ViewBag.UList = userlist;

            if(OrderID != 0)
            {
                SqlConnection conn1 = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn1.Open();

                SqlCommand ocmd1 = conn1.CreateCommand();
                ocmd1.CommandType = CommandType.StoredProcedure;
                ocmd1.CommandText = "PR_SelectByPK_Order";
                ocmd1.Parameters.AddWithValue("@OrderID", OrderID);
                ocmd1.ExecuteNonQuery();
                SqlDataReader data3 = ocmd1.ExecuteReader();
                DataTable dt3 = new DataTable();
                dt3.Load(data3);
                conn1.Close();

                OrderModel O = new OrderModel();

                foreach(DataRow dr in dt3.Rows)
                {
                    O.OrderID = Convert.ToInt32(dr["OrderID"]);
                    O.OrderNO = dr["OrderNO"].ToString();
                    O.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    O.CustomerID = Convert.ToInt32(dr["CustomerID"]);
                    O.PaymentMode = dr["PaymentMode"].ToString();
                    O.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                    O.ShippingAddress = dr["ShippingAddress"].ToString();
                    O.UserID = Convert.ToInt32(dr["UserID"]);
                }
                return View("OrderForm", O);
            }
            return View("OrderForm");
        }
        #endregion

        #region OrderSave
        public IActionResult OrderSave(OrderModel omodel)
        {
            if(omodel.UserID <= 0 && omodel.CustomerID <= 0)
            {
                ModelState.AddModelError("UserID", "UserID is not valid.");
                ModelState.AddModelError("CustomerID", "CustomerID is not valid.");
            }

            if (ModelState.IsValid) 
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                
                if(omodel.OrderID == 0 || omodel.OrderID == null)
                {
                    cmd.CommandText = "PR_Insert_Orders";
                }
                else
                {
                    cmd.CommandText = "PR_Update_Orders";
                    cmd.Parameters.Add("@OrderID",SqlDbType.Int).Value = omodel.OrderID;
                }
                cmd.Parameters.Add("@OrderNO",SqlDbType.VarChar).Value = omodel.OrderNO;
                cmd.Parameters.Add("@OrderDate",SqlDbType.DateTime).Value = omodel.OrderDate;
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = omodel.CustomerID ;
                cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = omodel.PaymentMode;
                cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = omodel.TotalAmount;
                cmd.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = omodel.ShippingAddress;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = omodel.UserID;
                cmd.ExecuteNonQuery();

                return RedirectToAction("OrderTable");
            }
            else
            {
                return View("OrderAddEdit", omodel);
            }
        }
        #endregion

        #region CancelBtn
        public IActionResult cancelbtn()
        {
            return RedirectToAction("OrderTable");
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnection"));
            conn.Open();

            SqlCommand ocmd = conn.CreateCommand();
            ocmd.CommandType = System.Data.CommandType.StoredProcedure;
            ocmd.CommandText = "PR_SelectAll_Orders";
            SqlDataReader reader = ocmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage()) 
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells["A1"].LoadFromDataTable(dt,true,OfficeOpenXml.Table.TableStyles.Medium9);

                //worksheet.Cells[1, 1].Value = "OrderID";
                //worksheet.Cells[1, 2].Value = "OrderNO";
                //worksheet.Cells[1, 3].Value = "OrderDate";
                //worksheet.Cells[1, 4].Value = "CustomerName";
                //worksheet.Cells[1, 5].Value = "PaymentMode";
                //worksheet.Cells[1, 6].Value = "TotalAmount";
                //worksheet.Cells[1, 7].Value = "ShippingAddress";
                //worksheet.Cells[1, 8].Value = "UserName";

                //int i = 1;

                //foreach (DataRow row in dt.Rows) 
                //{
                //    worksheet.Cells[i+1, 1].Value = row["OrderID"];
                //    worksheet.Cells[i + 1, 2].Value = row["OrderNO"];
                //    worksheet.Cells[i + 1, 3].Value = row["OrderDate"];
                //    worksheet.Cells[i + 1, 4].Value = row["CustomerName"];
                //    worksheet.Cells[i + 1, 5].Value = row["PaymentMode"];
                //    worksheet.Cells[i + 1, 6].Value = row["TotalAmount"];
                //    worksheet.Cells[i + 1, 7].Value = row["ShippingAddress"];
                //    worksheet.Cells[i + 1, 8].Value = row["UserName"];
                //    i++;
                //}

                //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //var fileContents = package.GetAsByteArray();

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Order.xlsx");

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
            billcmd.CommandText = "PR_SelectAll_Orders";
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
                var table = new PdfPTable(8);
                table.AddCell("OrderID");
                table.AddCell("OrderNO");
                table.AddCell("OrderDate");
                table.AddCell("CustomerName");
                table.AddCell("PaymentMode");
                table.AddCell("TotalAmount");
                table.AddCell("ShippingAddress");
                table.AddCell("UserName");

                // Add rows to the table
                foreach (DataRow row in dt.Rows)
                {
                    table.AddCell(Convert.ToInt32(row["OrderID"]).ToString());
                    table.AddCell(row["OrderNO"].ToString());
                    table.AddCell(Convert.ToDateTime(row["OrderDate"]).ToString());
                    table.AddCell(row["CustomerName"].ToString());
                    table.AddCell(row["PaymentMode"].ToString());
                    table.AddCell(row["TotalAmount"].ToString());
                    table.AddCell(row["ShippingAddress"].ToString());
                    table.AddCell(row["UserName"].ToString());
                }

                document.Add(table);

                document.Close();

                // Return the PDF as a byte array
                var bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", "OrderList.pdf");
            }
        }
        #endregion

        #region StaticCrud

        // Static Crud Operation

        //public IActionResult SaveOrder(OrderModel OM)
        //{
        //    if(OM.OrderId == 0)
        //    {
        //       OM.OrderId = OrderModelList.Max(o => o.OrderId+1);
        //       OrderModelList.Add(OM);
        //    }
        //    else
        //    {
        //        OrderModelList[OM.OrderId - 1].OrderDate = OM.OrderDate;
        //        OrderModelList[OM.OrderId - 1].CustomerID = OM.CustomerID;
        //        OrderModelList[OM.OrderId - 1].PaymentMode = OM.PaymentMode;
        //        OrderModelList[OM.OrderId - 1].TotalAmount = OM.TotalAmount;
        //        OrderModelList[OM.OrderId - 1].ShippingAddress = OM.ShippingAddress;
        //        OrderModelList[OM.OrderId - 1].UserID = OM.UserID;
        //    }
        //    return RedirectToAction("OrderTable");
        //}

        //public IActionResult AddEdit(int OrderId = 0)
        //{
        //    OrderModel o = new OrderModel();
        //    if (OrderId != 0)
        //    {
        //        OrderModel oo = OrderModelList.Find(oi => oi.OrderId == OrderId);
        //        o.OrderId = oo.OrderId;
        //        o.OrderDate = oo.OrderDate;
        //        o.CustomerID = oo.CustomerID;
        //        o.PaymentMode = oo.PaymentMode;
        //        o.ShippingAddress = oo.ShippingAddress;
        //        o.TotalAmount = oo.TotalAmount;
        //        o.UserID = oo.UserID;
        //    }
        //    return View("OrderForm", o);
        //}

        //public IActionResult DelOrder(int OrderId) 
        //{ 
        //    int n = OrderModelList.FindIndex(o => o.OrderId == OrderId);
        //    OrderModelList.RemoveAt(n);
        //    return RedirectToAction("OrderTable");
        //}
        #endregion

    }
}
