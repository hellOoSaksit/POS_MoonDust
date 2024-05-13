using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_MoonDust.Class
{
    // Class ที่ใช้เก็บข้อมูลผู้ใช้ในรูปแบบ Global
    public class Data_User
    {
        public string U_username { get; set; }
        public string U_name { get; set; }
        public string U_email { get; set; }
        public Boolean IsAdmin { get; set; }
    }

    public class Data_Product
    {
        public string Product_Name_Img { get; set; }
        public string Path_Img { get; set; }
    }
    // Class ที่ใช้เก็บข้อมูลผู้ใช้ในรูปแบบ Global
    public static class Global
    {
        public static Data_User currentUser;
        public static Data_Product currentProduct;
        public static float CartTotal { get; set; } = 0;
    }

}
