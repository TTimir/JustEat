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
                        string orderId = Request.QueryString["id"].ToString();
                        GetOrderDetails(orderId);
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        DataTable GetOrderDetails(string orderId)
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
                rOrderItem1.DataSource = dt;
                rOrderItem1.DataBind();

                rOrderItem2.DataSource = dt;
                rOrderItem2.DataBind();

                rOrderItem5.DataSource = dt;
                rOrderItem5.DataBind();

                foreach (DataRow drow in dt.Rows)
                {
                    grandTotal += Convert.ToDouble(drow["TotalPrice"]);
                }
            }

            DataRow dr = dt.NewRow();
            dr["TotalPrice"] = grandTotal;
            dt.Rows.Add(dr);
            return dt;
        }

        private void CalculateGrandTotal()
        {
            decimal grandTotal = 0;

            foreach (RepeaterItem item in rOrderItem1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblTotalPrice = (Label)item.FindControl("lblTotalPrice");
                    decimal totalPrice;
                    if (Decimal.TryParse(lblTotalPrice.Text.TrimStart('₹'), out totalPrice))
                    {
                        grandTotal += totalPrice;
                    }
                }
            }

            litGrandTotal.Text = grandTotal.ToString("0.00");
        }
    }
}