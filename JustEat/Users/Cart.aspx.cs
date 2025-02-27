﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JustEat.Admin;
using System.ComponentModel;

namespace JustEat.Users
{
    public partial class Cart : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataTable dt;
        decimal grandTotal = 0; 
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
                    getCartItems();
                }
            }
        }

        void getCartItems()
        {
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            adp = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adp.Fill(dt);
            rCartItem.DataSource = dt;
            if (dt.Rows.Count == 0)
            {
                rCartItem.HeaderTemplate = null;
                rCartItem.FooterTemplate = null;
                rCartItem.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            rCartItem.DataBind();
        }

        protected void rCartItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Connection.Utils utils = new Connection.Utils();
            if (e.CommandName == "remove")
            {
                conn = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Cart_Crud", conn);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    getCartItems();
                    //Cart count
                    Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["userId"]));
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
            else if (e.CommandName == "updateCart")
            {
                bool isCartUpdated = false;
                for (int item = 0; item < rCartItem.Items.Count; item++)
                {
                    if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox quantity = rCartItem.Items[item].FindControl("txtQuantity") as TextBox;
                        HiddenField _productId = rCartItem.Items[item].FindControl("hdnProductId") as HiddenField;
                        HiddenField _quantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;

                        int quantityFromCart = Convert.ToInt32(quantity.Text);
                        int productId = Convert.ToInt32(_productId.Value);
                        int quantityFromDB = Convert.ToInt32(_quantity.Value);
                        //bool isTrue = false;
                        //int updatedQuantity = 1;
                        //if (quantityFromCart > quantityFromDB)
                        //{
                        //    updatedQuantity = quantityFromCart;
                        //    isTrue = true;
                        //}
                        //else if (quantityFromCart < quantityFromDB)
                        //{
                        //    updatedQuantity = quantityFromCart;
                        //    isTrue = true;
                        //}

                        if (quantityFromCart != quantityFromDB)
                        {
                            // Update cart item quantity in DB.
                            isCartUpdated = utils.updateCartQuantity(quantityFromCart, productId, Convert.ToInt32(Session["userId"]));
                        }

                    }
                }
                getCartItems();
            }
            else if (e.CommandName == "checkout")
            {
                bool isCheckoutPossible = true;
                string outOfStockProductName = string.Empty;
                int stockLeft = 0;

                // Check if all items are in stock
                for (int item = 0; item < rCartItem.Items.Count; item++)
                {
                    if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField _productId = rCartItem.Items[item].FindControl("hdnProductId") as HiddenField;
                        HiddenField _cartQuantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
                        HiddenField _productQuantity = rCartItem.Items[item].FindControl("hdnPrdQuantity") as HiddenField;
                        Label productName = rCartItem.Items[item].FindControl("lblName") as Label;

                        int productId = Convert.ToInt32(_productId.Value);
                        int cartQuantity = Convert.ToInt32(_cartQuantity.Value);
                        int productQuantity = Convert.ToInt32(_productQuantity.Value);

                        if (productQuantity < cartQuantity)
                        {
                            isCheckoutPossible = false;
                            outOfStockProductName = productName.Text;
                            stockLeft = productQuantity; 
                            break;
                        }
                    }
                }

                if (isCheckoutPossible)
                {
                    Response.Redirect("Payment.aspx");
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Your favorite food <b>'" + outOfStockProductName + "'</b> is out of stock :( <b> Only " + stockLeft + " left.</b>";
                    lblMsg.CssClass = "alert alert-warning";
                }
            }
        }

        protected void rCartItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label totalPrice = e.Item.FindControl("lblTotalPrice") as Label;
                Label productPrice = e.Item.FindControl("lblPrice") as Label;
                TextBox quantity = e.Item.FindControl("txtQuantity") as TextBox;

                decimal calTotalPrice = 0;
                decimal productPriceValue = 0;
                int quantityValue = 0;

                // Validate and convert product price
                if (decimal.TryParse(productPrice.Text, out productPriceValue) &&
                    int.TryParse(quantity.Text, out quantityValue))
                {
                    calTotalPrice = productPriceValue * quantityValue;
                    totalPrice.Text = calTotalPrice.ToString("F2"); // Format as a decimal with 2 decimal places
                    grandTotal += calTotalPrice;
                }
                else
                {
                    // Handle the case where the conversion fails
                    totalPrice.Text = "0.00";
                }
            }
            Session["grandTotalPrice"] = grandTotal;
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
                    var footer = new LiteralControl("<tr><td colspan='5'><b>Your Cart is empty.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Continue Shopping</a></td></tr><t/body></table>");
                    container.Controls.Add(footer);
                }
            }
        }
    }
}