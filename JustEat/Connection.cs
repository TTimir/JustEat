using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using JustEat.Admin;

namespace JustEat
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        }

        public class Utils
        {
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataAdapter adp;
            DataTable dt;
            public static bool IsValidExtension(string fileName)
            {
                bool IsValid = false;
                string[] fileExtension = { ".jpg", ".jpeg", ".png" };
                for (int i = 0; i <= fileExtension.Length - 1; i++)
                {
                    if (fileName.Contains(fileExtension[i]))
                    {
                        IsValid = true;
                        break;
                    }
                }
                return IsValid;
            }

            public static string GetImageUrl(Object url)
            {
                string url1 = "";
                if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
                {
                    url1 = "../Images/No_image.png";
                }
                else
                {
                    url1 = string.Format("../{0}", url);
                }
                return url1;
            }

            public bool updateCartQuantity(int quantity, int productId, int userId)
            {
                bool isUpdated = false;
                conn = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Cart_Crud", conn);
                cmd.Parameters.AddWithValue("@Action", "UPDATE");
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    isUpdated = false;
                    System.Web.HttpContext.Current.Response.Write("<script>alert(' Error - " + ex.Message + " ');</script>");
                }
                finally
                {
                    conn.Close();
                }
                return isUpdated;
            }

            public int cartCount(int userId)
            {
                conn = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Cart_Crud", conn);
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.CommandType = CommandType.StoredProcedure;
                adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt.Rows.Count;
            }

            public static string GetUniqueId()
            {
                Guid guid = Guid.NewGuid();
                String uniqueId = guid.ToString();
                return uniqueId;
            }
        }

    }
}