using Npgsql;
using POS_MoonDust.Class;
using System;
using System.Windows.Forms;

namespace POS_MoonDust
{
    public partial class NewProduct : Form
    {
        public NewProduct()
        {
            InitializeComponent();
        }
        public static void NewProduct_Load()
        {
            NewProduct newProduct = new NewProduct();
            newProduct.ShowDialog();
        }

        private void NewProduct_Load(object sender, EventArgs e)
        {
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();
                // ดึงข้อมูลจากฐานข้อมูล

                string categorySQL = "SELECT category_id , category_name FROM category";
                using (NpgsqlCommand command = new NpgsqlCommand(categorySQL, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        guna2ComboBox_Category.DisplayMember = "Item1";
                        // ลูปอ่านข้อมูล comboBox DataSet
                        while (reader.Read())
                        {
                            string Category_name = reader["category_name"].ToString();

                            guna2ComboBox_Category.Items.Add(Tuple.Create(Category_name)); //(Food)
                        }
                    }
                }
            }
        }

        private async void guna2Button_ADD_Click(object sender, EventArgs e)
        {
            string query;
            // บันทึกข้อมูลผู้ใช้ลงในฐานข้อมูล 
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();
                try
                {
                    // ? :
                    query = CheckPhoto ? "INSERT INTO public.product_item (product_name, product_des, product_category ,product_price ,product_link ) " +
                        "VALUES (@product_name, @product_des, @product_category , @product_price , @product_link)"
                        : "INSERT INTO public.product_item (product_name, product_des, product_category ,product_price) " +
                        "VALUES (@product_name, @product_des, @product_category , @product_price )";


                    var product_item = new NpgsqlCommand(query, connection);
                    product_item.Parameters.AddWithValue("@product_name", guna2TextBox_ProductName.Text);
                    product_item.Parameters.AddWithValue("@product_des", guna2TextBox_Description.Text);
                    product_item.Parameters.AddWithValue("@product_price", NUD_Price.Value);
                    string category = guna2ComboBox_Category.SelectedItem?.ToString();
                    category = category?.Trim('(', ')');
                    product_item.Parameters.AddWithValue("@product_category", category);
                    if (CheckPhoto)
                    {
                        // กำหนด path ของรูปภาพ
                        string imagePath = Global.currentProduct.Path_Img;
                        // เชื่อมต่อ Dropbox API
                        // var dropboxUrl = await ImageUploader.UploadImageToDropbox(imagePath, "Product_Images");
                        // เชื่อมต่อ Imgur API
                        var imgurUrl = await ImgurUploader.UploadImageToImgur(imagePath);
                        product_item.Parameters.AddWithValue("@product_link", imgurUrl);

                    }

                    product_item.ExecuteNonQuery();

                    MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ไม่สามารถเพิ่มข้อมูลได้ โปรดต่อผู้ดูแล" +"\n"+
                        "1.ไม่สามารถอัพโหลดรูปได้" + "\n"+
                        "2.ไม่สามารถเชื่อมต่อฐานข้อมูลได้" 
                        , "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { connection.Close(); }
            }
            
        }


        bool CheckPhoto = false;
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

        private void guna2Button_EditCategory_Click(object sender, EventArgs e)
        {
            EditCategoryForm.EditCategorys();
        }
    }
}
