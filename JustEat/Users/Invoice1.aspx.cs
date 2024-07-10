using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Asn1.X509;

namespace JustEat.Users
{
    public partial class Invoice1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataAdapter adp;
            DataTable dt;
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    if (Session["userId"] != null)
                    {
                        if (Request.QueryString["id"] != null)
                        {
                            GetOrderDetails();
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

                try
                {
                    conn.Open();
                    adp = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adp.Fill(dt);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (dt.Rows.Count > 0 && reader.Read())
                        {
                            // Display payment mode
                            string paymentMode = dt.Rows[0]["PaymentMode"].ToString().ToUpper();
                            string cardNumber = reader["CardNo"].ToString();
                            lblPaymentMode.Text = "Payment Mode: " + paymentMode;

                            if (paymentMode == "CARD")
                            {
                                lblPaymentMode.Text = "Payment Mode: <br />CREDIT CARD ****" + cardNumber.Substring(cardNumber.Length - 4);
                            }

                            // Display order date
                            DateTime orderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"]);
                            lblOrderDate.Text = "Date: " + orderDate.ToString("dd-MM-yyyy");

                            // Calculate and display grand total
                            double totalAmount = Convert.ToDouble(dt.Compute("SUM(TotalPrice)", string.Empty));
                            lblGrandTotal.Text = "Amount: ₹" + totalAmount.ToString("0.00");

                            // Bind order items to all repeater controls using a loop
                            List<Repeater> repeaters = new List<Repeater> { rOrderItem1, rOrderItem2, rOrderItem5 };
                            foreach (Repeater repeater in repeaters)
                            {
                                repeater.DataSource = dt;
                                repeater.DataBind();
                            }

                            // Calculate grand total for session storage
                            foreach (DataRow drow in dt.Rows)
                            {
                                grandTotal += Convert.ToDouble(drow["TotalPrice"]);
                            }
                        }
                        else
                        {
                            // Handle case where no data is returned
                            Response.Write("Error no data is returned");
                        }
                    }
                    DataRow dr = dt.NewRow();
                    dr["TotalPrice"] = grandTotal;
                    dt.Rows.Add(dr);

                    // Save the grand total to session
                    Session["grandTotalPrice"] = grandTotal;
                    return dt;
                }
                catch (Exception ex)
                {
                    // Handle any database or other exceptions
                    throw new Exception("Error retrieving order details: " + ex.Message);
                }
                finally
                {
                    conn.Close(); // Close connection in finally block
                }
            }
        }
    }
}