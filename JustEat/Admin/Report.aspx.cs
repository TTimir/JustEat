using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JustEat.Admin
{
    public partial class Report : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Selling Reports";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../Users/Login.aspx");
                }
            }
        }

        private void getReportData(DateTime fromDate, DateTime toDate)
        {
            double grandTotal = 0;
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("SellingReport", conn);
            cmd.Parameters.AddWithValue("@FromDate", fromDate);
            cmd.Parameters.AddWithValue("@ToDate", toDate);
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow drow in dt.Rows)
                {
                    grandTotal += Convert.ToDouble(drow["TotalPrice"]);
                }
                lblTotal.Text = "Sold Cost: ₹" + grandTotal;
                lblTotal.CssClass = "badge badge-primary";
            }
            rReport.DataSource = dt;
            rReport.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime fromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime toDate = Convert.ToDateTime(txtToDate.Text);
            if (toDate > DateTime.Now)
            {
                Response.Write("<script>alert('End date(To Date) can not be after today!');</script>");
            }
            else if (fromDate > toDate)
            {
                Response.Write("<script>alert('Start date(From Date) can not be after the end date!');</script>");
            }
            else if (toDate < fromDate)
            {
                Response.Write("<script>alert('End date(To Date) can not be before the start date!');</script>");
            }
            else
            {
                getReportData(fromDate, toDate);
            }
        }
    }
}