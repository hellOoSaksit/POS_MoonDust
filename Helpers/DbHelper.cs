using Npgsql;
using POS_MoonDust.Class;
using POS_MoonDust.CustomControls;
using System;
using System.Windows.Forms;

namespace POS_MoonDust.Helpers
{
    class DbHelper
    {
        NpgsqlConnection conn;

        public DbHelper()
        {

        }

        public void GetButtons(string query, FlowLayoutPanel panel)
        {
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                try
            {

                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string id = reader["product_id"].ToString();
                                string ItemName = reader["product_name"].ToString();
                                string ItemPrice = reader["product_price"].ToString();
                                string picturebox = reader["product_link"].ToString();
                                string category = reader["product_category"].ToString();
                                // Create a new btnProduct
                                btnProduct btnProduct = new btnProduct();
                                btnProduct.ItemName = ItemName;
                                btnProduct.ItemPrice = ItemPrice;
                                //ใช้งานได้ปกติทุกอย่าง แต่ตังไม่มีซื้อ Token แล้วค้าบบบบบ
                                if (picturebox != "NoLink") { btnProduct.ItemImg = picturebox; }
                                btnProduct.ItemCategory = category;
                                // Add btnProduct to the panel
                                panel.Controls.Add(btnProduct);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { connection.Close(); }
            }

        }

    }
}
