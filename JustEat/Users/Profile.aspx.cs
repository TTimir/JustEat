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
                Session["username"] = dt.Rows[0]["Username"].ToString();
                Session["mobile"] = dt.Rows[0]["Mobile"].ToString();
                Session["email"] = dt.Rows[0]["Email"].ToString();
                Session["address"] = dt.Rows[0]["Address"].ToString();
                Session["postcode"] = dt.Rows[0]["PostCode"].ToString();
                Session["imageUrl"] = dt.Rows[0]["ImageUrl"].ToString();
                Session["createdDate"] = dt.Rows[0]["CreatedDate"].ToString();
            }
            else
            {
                lblMessage.Text = "No user details found.";
                lblMessage.CssClass = "alert alert-warning";
            }
        }
    }
}