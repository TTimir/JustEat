using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using System.Web.UI.HtmlControls;
namespace JustEat.Users
{
    public partial class Invoice : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] != null)
                {
                    if (Request.QueryString["id"] != null)
                    {
                        GetOrderDetails();
                        //CalculateGrandTotal();
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        DataTable GetOrderDetails()
        {
            double grandTotal = 0;
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", conn);
            cmd.Parameters.AddWithValue("@Action", "INVOICEBYID");
            cmd.Parameters.AddWithValue("@PaymentId", Convert.ToInt32(Request.QueryString["id"]));
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;

            // Debug: Log the parameters
            System.Diagnostics.Debug.WriteLine("Action: INVOICEBYID");
            System.Diagnostics.Debug.WriteLine("PaymentId: " + Convert.ToInt32(Request.QueryString["id"]));
            System.Diagnostics.Debug.WriteLine("UserId: " + Session["userId"]);

            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            // Debug: Log the columns in the DataTable
            foreach (DataColumn column in dt.Columns)
            {
                System.Diagnostics.Debug.WriteLine("Column: " + column.ColumnName);
            }

            if (dt.Rows.Count > 0)
            {
                //string paymentMode = dt.Rows[0]["PaymentMode"].ToString().ToUpper();
                //lblPaymentMode.Text = "Payment Mode: " + paymentMode;

                //string orderDate = lblOrderDate.Text = Convert.ToDateTime(dt.Rows[0]["OrderDate"]).ToString("dd-MM-yyyy");
                //lblOrderDate.Text = "Date: " + orderDate;

                //string finalgrandTotal = lblGrandTotal.Text = "₹" + dt.Compute("SUM(TotalPrice)", string.Empty).ToString();
                //lblGrandTotal.Text = "Amount: " + finalgrandTotal;

                rOrderItem1.DataSource = dt;
                rOrderItem1.DataBind();

                //rOrderItem2.DataSource = dt;
                //rOrderItem2.DataBind();

                //rOrderItem5.DataSource = dt;
                //rOrderItem5.DataBind();

                foreach (DataRow drow in dt.Rows)
                {
                    grandTotal += Convert.ToDouble(drow["TotalPrice"]);
                }
            }

            DataRow dr = dt.NewRow();
            dr["TotalPrice"] = grandTotal;
            dt.Rows.Add(dr);

            // Save the grand total to session
            Session["grandTotalPrice"] = grandTotal;
            return dt;
        }


        protected void lbDownloadInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                string downloadPath = @"E:\order_invoice.pdf";
                DataTable dtbl = GetOrderDetails();
                ExportToPdf(dtbl, downloadPath, "Order Invoice");

                WebClient client = new WebClient();
                byte[] buffer = client.DownloadData(downloadPath);
                if (buffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", buffer.Length.ToString());
                    Response.BinaryWrite(buffer);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error - " + ex.Message.ToString();
            }

        }

        void ExportToPdf(DataTable dtblTable, String strPdfPath, string strHeader)
        {
            FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 16, 1, BaseColor.GRAY);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
            document.Add(prgHeading);

            // Author
            Paragraph prgAuthor = new Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntAuthor = new Font(btnAuthor, 8, 2, BaseColor.GRAY);
            prgAuthor.Alignment = Element.ALIGN_RIGHT;
            prgAuthor.Add(new Chunk("Order From : JustEat Fast Food", fntAuthor));
            prgAuthor.Add(new Chunk("\nOrder Date : " + dtblTable.Rows[0]["OrderDate"].ToString(), fntAuthor));
            document.Add(prgAuthor);

            // Add a line separation
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(p);

            // Add line break
            document.Add(new Chunk("\n", fntHead));

            // Write the table
            PdfPTable table = new PdfPTable(dtblTable.Columns.Count - 2);
            // Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, 9, 1, BaseColor.WHITE);
            for (int i = 0; i < dtblTable.Columns.Count - 2; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = BaseColor.GRAY;
                cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                table.AddCell(cell);
            }
            // Table Data
            Font fntColumnData = new Font(btnColumnHeader, 8, 1, BaseColor.BLACK);
            for (int i = 0; i < dtblTable.Rows.Count; i++)
            {
                for (int j = 0; j < dtblTable.Columns.Count - 2; j++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.AddElement(new Chunk(dtblTable.Rows[i][j].ToString(), fntColumnData));
                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
        }
    }
}