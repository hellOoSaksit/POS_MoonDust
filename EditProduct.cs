using Npgsql;
using POS_MoonDust.Class;
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using POS_MoonDust.Properties;

namespace POS_MoonDust
{
    public partial class EditProduct : Form
    {
        public EditProduct()
        {
            InitializeComponent();
        }

        private void EditProduct_Load(object sender, EventArgs e)
        {
            LoadProductNames();
            LoadProductCategories();
        }
        public static void EditProduct_Load()
        {
            EditProduct editProduct = new EditProduct();
            editProduct.ShowDialog();
        }
        private void LoadProductNames()
        {
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();
                string categorySQL = "SELECT product_name FROM public.product_item";
                using (NpgsqlCommand command = new NpgsqlCommand(categorySQL, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = reader["product_name"].ToString();
                            guna2ComboBox_List.Items.Add(productName);
                        }
                    }
                }
                connection.Close();
            }
        }

        private void LoadProductCategories()
        {
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();
                string categorySQL = "SELECT DISTINCT product_category FROM public.product_item";
                using (NpgsqlCommand command = new NpgsqlCommand(categorySQL, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productCategory = reader["product_category"].ToString();
                            guna2ComboBox_Category.Items.Add(productCategory);
                        }
                    }
                }
                connection.Close();
            }
        }
        int _ProductID;
        private async void guna2ComboBox_List_SelectedValueChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox_List.SelectedItem != null)
            {
                string selectedProductName = guna2ComboBox_List.SelectedItem.ToString();
                string query = "SELECT * FROM public.product_item WHERE product_name = @productName";

                using (var connection = new NpgsqlConnection(Conn.connectionString))
                {
                    connection.Open();
                    try
                    {
                        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@productName", selectedProductName);

                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    _ProductID = (int)reader["product_id"]; // บันทึก Product ID ลงในตัวแปร _ProductID
                                    string productName = reader["product_name"].ToString();
                                    string productDescription = reader["product_des"].ToString();
                                    string productCategory = reader["product_category"].ToString();
                                    decimal productPrice = (decimal)reader["product_price"];
                                    string productLink = reader["product_link"].ToString(); // ดึง URL ของรูปภาพจากฐานข้อมูล
                                    guna2TextBox_ProductName.Text = productName;
                                    guna2TextBox_Description.Text = productDescription;
                                    guna2ComboBox_Category.Text = productCategory;
                                    NUD_Price.Value = productPrice;

                                    if(productLink == "NoLink")
                                    {
                                        guna2PictureBox_Product.Image = Properties.Resources.no_image_icon;
                                    }
                                    else
                                    {
                                        guna2PictureBox_Product.Load(productLink);
                                    }
                                    
                                }

                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show("ไม่สามารถอัปเดตข้อมูลได้ โปรดติดต่อผู้ดูแล" + "\n" +
                           "1.ไม่สามารถอัปโหลดรูปได้" + "\n" +
                           "2.ไม่สามารถเชื่อมต่อฐานข้อมูลได้"
                           , "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } finally { connection.Close(); }
                }
            }
        }

        bool CheckPhoto;
        private void guna2Button_Img_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an Image";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Data_Product data_Product = new Data_Product
                {
                    Path_Img = openFileDialog.FileName
                };
                Global.currentProduct = data_Product;
                guna2PictureBox_Product.Image = System.Drawing.Image.FromFile(Global.currentProduct.Path_Img);
                CheckPhoto = true;
            }
        }

        private async void guna2Button_Edit_Click(object sender, EventArgs e)
        {
            string query;
            // บันทึกข้อมูลผู้ใช้ลงในฐานข้อมูล
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();
                try
                {
                    query = CheckPhoto ? "UPDATE public.product_item SET product_name = @product_name, product_des = @product_des, product_category = @product_category, product_price = @product_price, product_link = @product_link WHERE product_id = @product_id" 
                                       : "UPDATE public.product_item SET product_name = @product_name, product_des = @product_des, product_category = @product_category, product_price = @product_price WHERE product_id = @product_id";


                    var product_item = new NpgsqlCommand(query, connection);
                    product_item.Parameters.AddWithValue("@product_name", guna2TextBox_ProductName.Text);
                    product_item.Parameters.AddWithValue("@product_des", guna2TextBox_Description.Text);
                    product_item.Parameters.AddWithValue("@product_price", NUD_Price.Value);
                    product_item.Parameters.AddWithValue("@product_id", _ProductID); 
                    string category = guna2ComboBox_Category.SelectedItem?.ToString();
                    category = category?.Trim('(', ')');
                    product_item.Parameters.AddWithValue("@product_category", category);
                    if (CheckPhoto)
                    {
                        // กำหนด path ของรูปภาพ
                        string imagePath = Global.currentProduct.Path_Img;

                        // เชื่อมต่อ Imgur API
                        var imgurUrl = await ImgurUploader.UploadImageToImgur(imagePath);
                        product_item.Parameters.AddWithValue("@product_link", imgurUrl);
                    }
                    product_item.ExecuteNonQuery();

                    MessageBox.Show("อัปเดตข้อมูลสำเร็จ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ไม่สามารถอัปเดตข้อมูลได้ โปรดติดต่อผู้ดูแล" + "\n" +
                        "1.ไม่สามารถอัปโหลดรูปได้" + "\n" +
                        "2.ไม่สามารถเชื่อมต่อฐานข้อมูลได้"
                        , "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }finally { connection.Close(); } 
            }
        }

        private void guna2Button_Delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2ComboBox_List.Text))
            {
                MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการลบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedProductName = guna2ComboBox_List.SelectedItem.ToString();

            // ลบสินค้าจากฐานข้อมูล
            try
            {
                using (var connection = new NpgsqlConnection(Conn.connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM public.product_item WHERE product_name = @productName";
                    using (var deleteCommand = new NpgsqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@productName", selectedProductName);
                        int rowsAffected = deleteCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("ลบสินค้าสำเร็จ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProductNames();
                            ClearProductFields();
                        }
                        else
                        {
                            MessageBox.Show("ไม่สามารถลบสินค้าได้", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ไม่สามารถลบสินค้าได้: " + ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearProductFields()
        {
            guna2TextBox_ProductName.Text = "";
            guna2TextBox_Description.Text = "";
            guna2ComboBox_Category.SelectedIndex = -1;
            NUD_Price.Value = 0;
            guna2PictureBox_Product.Image = null; // Clear image
        }
    }
}
