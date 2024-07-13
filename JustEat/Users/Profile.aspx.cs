using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace JustEat.Users
{
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    getUserDetails();
                    getPurchaseHistory();
                }
            }
        }

        void getUserDetails()
        {
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("User_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", "SELECT4PROFILE");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            rUserProfile.DataSource = dt;
            rUserProfile.DataBind();

            if (dt.Rows.Count == 1)
            {
                // Update session variables with fresh data
                Session["name"] = dt.Rows[0]["Name"].ToString();
                Session["email"] = dt.Rows[0]["Email"].ToString();
                Session["imageUrl"] = dt.Rows[0]["ImageUrl"].ToString();
                Session["createdDate"] = ((DateTime)dt.Rows[0]["CreatedDate"]).ToString("yyyy-MM-dd");
                //Session["username"] = dt.Rows[0]["Username"].ToString();
                //Session["mobile"] = dt.Rows[0]["Mobile"].ToString();
                //Session["address"] = dt.Rows[0]["Address"].ToString();
                //Session["postcode"] = dt.Rows[0]["PostCode"].ToString(); 
            }
            else
            {
                lblMessage.Text = "No user details found.";
                lblMessage.CssClass = "alert alert-warning";
            }
        }

        void getPurchaseHistory()
        {
            int sr = 1;
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", conn);
            cmd.Parameters.AddWithValue("@Action", "ODRHISTORY");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            dt.Columns.Add("SrNo", typeof(Int32));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow drow in dt.Rows)
                {
                    drow["SrNo"] = sr;
                    sr++;
                }
            }
            if (dt.Rows.Count == 0)
            {
                rPurchaseHistory.HeaderTemplate = null;
                rPurchaseHistory.FooterTemplate = null;
                rPurchaseHistory.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            rPurchaseHistory.DataSource = dt;
            rPurchaseHistory.DataBind();
        }

        protected void rPurchaseHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                double grandTotal = 0;
                HiddenField paymentId = e.Item.FindControl("hdnPaymentId") as HiddenField;
                Repeater repOrders = e.Item.FindControl("rOrders") as Repeater;

                conn = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Invoice", conn);
                cmd.Parameters.AddWithValue("@Action", "INVOICEBYID");
                cmd.Parameters.AddWithValue("@PaymentId", Convert.ToInt32(paymentId.Value));
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
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
                }
                DataRow dr = dt.NewRow();
                dr["TotalPrice"] = grandTotal;
                dt.Rows.Add(dr);
                repOrders.DataSource = dt;
                repOrders.DataBind();
            }
        }

        // Custom template class to add controls to the repeater's header, item and footer sections.
        private sealed class CustomTemplate : ITemplate
        {
            private ListItemType ListItemType { get; set; }

            public CustomTemplate(ListItemType type)
            {
                ListItemType = type;
            }

            public void InstantiateIn(Control container)
            {
                if (ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td><b>Hungry! Why not order your favourite food for you.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Click to Order</a></td></tr><t/body></table>");
                    container.Controls.Add(footer);
                }
            }
        }

    }
}