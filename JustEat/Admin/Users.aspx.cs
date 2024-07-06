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
    public partial class Users : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Users";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../Users/Login.aspx");
                }
                else
                {
                    getUsers();
                }
            }
            lblMsg.Visible = false;
        }

        private void getUsers()
        {
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("User_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", "SELECT4ADMIN");
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            rUsers.DataSource = dt;
            rUsers.DataBind();
        }

        protected void rUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                conn = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("User_Crud", conn);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@UserId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "User deleted succesfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getUsers();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error! " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}