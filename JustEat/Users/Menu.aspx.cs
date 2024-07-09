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

        protected void rProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["userId"] != null)
            {
                bool isCartItemUpdated = false;
                int i = isItemExistInCart(Convert.ToInt32(e.CommandArgument));
                if (i == 0)
                {
                    // Addiing new item in cart
                    conn = new SqlConnection(Connection.GetConnectionString());
                    cmd = new SqlCommand("Cart_Crud", conn);
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@Quantity", 1);
                    cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) 
                    {
                        Response.Write("<script>alert(' Error - " + ex.Message + " ');</script>");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    // Addiing existing item into cart
                    Connection.Utils utils = new Connection.Utils();
                    isCartItemUpdated = utils.updateCartQuantity(i + 1, Convert.ToInt32(e.CommandArgument), 
                        Convert.ToInt32(Session["userId"]));
                }
                lblMsg.Visible = true;
                lblMsg.Text = "<b>hooray!</b> Your food is waiting for you in your cart.";
                lblMsg.CssClass = "alert alert-success";
                Response.AddHeader("REFRESH", "1;URL=Cart.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        int isItemExistInCart(int productId)
        {
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", "GETBYID");
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            int quantity = 0;
            if (dt.Rows.Count > 0)
            {
                quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]);
            }
            return quantity;
        }

        //public string LowerCase(object obj)
        //{
        //    return obj.ToString().ToLower();
        //}
    }
}