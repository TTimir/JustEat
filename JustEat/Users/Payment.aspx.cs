using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JustEat.Users
{
    public partial class Payment : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr, dr1;
        DataTable dt;
        SqlTransaction transaction = null;
        string _name = string.Empty;
        string _cardNo = string.Empty;
        string _expiryDate = string.Empty;
        string _cvv = string.Empty;
        string _address = string.Empty;
        string _paymentMode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void lbCardSubmit_Click(object sender, EventArgs e)
        {
            _name = txtName.Text.Trim();
            _cardNo = txtCardNo.Text.Trim();
            _cardNo = string.Format("************{0}", txtCardNo.Text.Trim().Substring(12, 4));
            _expiryDate = txtExpMonth.Text.Trim() + "/" + txtExpYear.Text.Trim();
            _cvv = txtCvv.Text.Trim();
            _address = txtAddress.Text.Trim();
            _paymentMode = "card";
            if (Session["userId"] != null)
            {
                OrderPayment(_name, _cardNo, _expiryDate, _cvv, _address, _paymentMode);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void lbCodSubmit_Click(object sender, EventArgs e)
        {
            _address = txtCODAddress.Text.Trim();
            _paymentMode = "cod";
            if (Session["userId"] != null)
            {
                OrderPayment(_name, "not required", "not required", "not required", _address, _paymentMode);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        void OrderPayment(string name, string cardNo, string expiryDate, string cvv, string address, string paymentMode)
        {
            int paymentId;
            int productId;
            int quantity;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("OrderNo", typeof(string)),
                new DataColumn("ProductId", typeof(int)),
                new DataColumn("Quantity", typeof(int)),
                new DataColumn("UserId", typeof(int)),
                new DataColumn("Status", typeof(string)),
                new DataColumn("PaymentId", typeof(int)),
                new DataColumn("OrderDate", typeof(DateTime))
            });

            SqlConnection conn = new SqlConnection(Connection.GetConnectionString());
            conn.Open();

            SqlTransaction transaction = conn.BeginTransaction();

            SqlCommand cmd = new SqlCommand("Save_Payment", conn, transaction);
            cmd.CommandType = CommandType.StoredProcedure;

            // Handle card details based on paymentMode
            if (paymentMode == "cod")
            {
                // Set as null for not required fields
                cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                cmd.Parameters.AddWithValue("@CardNo", DBNull.Value);
                cmd.Parameters.AddWithValue("@ExpiryDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@Cvv", DBNull.Value);
            }
            else
            {
                // Use actual card details
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@CardNo", cardNo);
                cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                cmd.Parameters.AddWithValue("@Cvv", cvv);
            }

            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
            cmd.Parameters.Add("@InsertedId", SqlDbType.Int).Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                paymentId = Convert.ToInt32(cmd.Parameters["@InsertedId"].Value);

                cmd = new SqlCommand("Cart_Crud", conn, transaction);
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["ProductId"] != DBNull.Value)
                    {
                        productId = (int)dr["ProductId"];
                    }
                    else
                    {
                        productId = 0;
                    }

                    if (dr["Quantity"] != DBNull.Value)
                    {
                        quantity = (int)dr["Quantity"];
                    }
                    else
                    {
                        quantity = 0;
                    }

                    // Logging values for debugging
                    System.Diagnostics.Debug.WriteLine($"ProductId: {productId}, Quantity: {quantity}");

                    UpdateQuantity(productId, quantity, transaction, conn);

                    DeleteCartItem(productId, transaction, conn);

                    dt.Rows.Add(Connection.Utils.GetUniqueId(), productId, quantity, (int)Session["userId"], "Pending",
                        paymentId, DateTime.Now);
                }
                dr.Close();

                if (dt.Rows.Count > 0)
                {
                    cmd = new SqlCommand("Save_Orders", conn, transaction);
                    cmd.Parameters.AddWithValue("@tblOrders", dt);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                lblMsg.Visible = true;
                lblMsg.Text = "Your item ordered successfully!";
                lblMsg.CssClass = "alert alert-success";
                Response.AddHeader("REFRESH", "1;URL=Invoice1.aspx?id=" + paymentId);
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Transaction rolled back due to an error.";
                    lblMsg.CssClass = "alert alert-info";
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
                catch (Exception rollbackEx)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Rollback failed: " + rollbackEx.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            finally
            {
                conn.Close();
            }
        }

        void UpdateQuantity(int _productId, int _quantity, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            int dbQuantity;
            SqlCommand cmd = new SqlCommand("Product_Crud", sqlConnection, sqlTransaction);
            cmd.Parameters.AddWithValue("@Action", "GETBYID");
            cmd.Parameters.AddWithValue("@ProductId", _productId);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataReader dr1 = cmd.ExecuteReader();
                while (dr1.Read())
                {
                    dbQuantity = (int)dr1["Quantity"];

                    if (dbQuantity > _quantity && dbQuantity > 2)
                    {
                        dbQuantity = dbQuantity - _quantity;
                        cmd = new SqlCommand("Product_Crud", sqlConnection, sqlTransaction);
                        cmd.Parameters.AddWithValue("@Action", "QTYUPDATE");
                        cmd.Parameters.AddWithValue("@Quantity", dbQuantity);
                        cmd.Parameters.AddWithValue("@ProductId", _productId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
                dr1.Close();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void DeleteCartItem(int _productId, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            SqlCommand cmd = new SqlCommand("Cart_Crud", sqlConnection, sqlTransaction);
            cmd.Parameters.AddWithValue("@Action", "DELETE");
            cmd.Parameters.AddWithValue("@ProductId", _productId);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}