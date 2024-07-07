using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JustEat.Admin;

namespace JustEat.Users
{
    public partial class Menu : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                getCategories();
                getProducts();
            }
        }

        private void getCategories()
        {
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", "ACTIVECAT");
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }

        private void getProducts()
        {
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Product_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", "ACTIVEPROD");
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
           
            if (dt.Rows.Count == 0)
            {
                lblNoProducts.Visible = true;
                lblNoProducts.Text = "No products available";
                rProducts.Visible = false;
            }
            else
            {
                lblNoProducts.Visible = false;
                rProducts.Visible = true;
                rProducts.DataSource = dt;
                rProducts.DataBind();
            }
        }

        //public string LowerCase(object obj)
        //{
        //    return obj.ToString().ToLower();
        //}
    }
}