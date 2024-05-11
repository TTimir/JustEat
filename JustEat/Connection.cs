using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

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
        }

    }
}