using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management.Instrumentation;
using System.Windows.Forms;
using Npgsql;
using POS_MoonDust.Class;
using POS_MoonDust.CustomControls;
using POS_MoonDust.Helpers;

namespace POS_MoonDust
{
    public partial class MainLoad : Form
    {
        private float cartTotal = 0;

        public MainLoad()
        {
            InitializeComponent();
        }

        public static void MainLoadForm()
        {
            MainLoad mainLoad = new MainLoad();
            mainLoad.Show();
        }

        private void guna2Button_NewOrder_Click(object sender, EventArgs e)
        {
            guna2Panel_LoadForm.Enabled = true;
            guna2Panel_LoadForm.FillColor = Color.White;
            guna2Panel2.Enabled = false;
           
        }

        private void MainLoad_Load(object sender, EventArgs e)
        {
            LB_USERNAME.Text = Global.currentUser.U_name.ToString();
            LB_Days.Text = DateTime.Now.ToString("dd/MM/yyyy");
           

            CatRefresh();
                if (Global.currentUser.IsAdmin)
            {
                guna2Button_Newproduct.Visible = true;
                guna2Button_EditProduct.Visible = true;
            }
            SqlRefresh();

        }

        private void guna2ComboBox_Category_SelectedValueChanged(object sender, EventArgs e)
        {

            string selectedCategory = guna2ComboBox_Category.SelectedItem?.ToString();
            selectedCategory = selectedCategory.Replace("(", "").Replace(")", "");
            if (selectedCategory == "All" || string.IsNullOrEmpty(selectedCategory))
            {
                ShowAllProducts();
            }
            else
            {
                foreach (Control control in panelItems.Controls)
                {
                    if (control is btnProduct btn)
                    {
                        if (btn.ItemCategory == selectedCategory)
                        {
                            btn.Visible = true;
                        }
                        else
                        {
                            btn.Visible = false;
                        }
                    }
                }
            }
            
        }

        private void ShowAllProducts()
        {
            foreach (Control control in panelItems.Controls)
            {
                if (control is btnProduct btn)
                {
                    btn.Visible = true;
                }
            }
        }


        public void AddCartItem(string itemId, string itemName, string itemPrice)
        {
            CartItem cartItem = new CartItem();
            cartItem.ItemId = itemId;
            cartItem.ItemName = itemName;
            cartItem.ItemPrice = itemPrice;
            cartItem.ItemQuantity = "1";
            cartItem.CalculateTotalPrice();
            panelCartItem.Controls.Add(cartItem);
        }

        private void guna2Button_logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            login_Form.Login_Form();
        }

        private void guna2Button_Newproduct_Click(object sender, EventArgs e)
        {
            NewProduct.NewProduct_Load();
        }

        private void guna2Button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You want to cancel the order", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                guna2Panel_LoadForm.Enabled = false;
                guna2Panel_LoadForm.FillColor = Color.Silver;
                guna2Panel2.Enabled = true;
            }
            panelCartItem.Controls.Clear();
        }

        private void MainLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void RecalculateTotalPrice()
        {
            float totalPrice = 0;

            foreach (Control control in panelCartItem.Controls)
            {
                if (control is CartItem cartItem)
                {
                    totalPrice += float.Parse(cartItem.ItemPerUnitPrice);
                }
            }

            Global.CartTotal = totalPrice;
        }
        private void btn_Checkout_Click(object sender, EventArgs e)
        {
            RecalculateTotalPrice(); // เรียกใช้เมธอดเพื่อคำนวณยอดรวม

            List<string[]> itemsData = new List<string[]>(); // สร้าง List เพื่อเก็บข้อมูลสินค้า

            foreach (Control control in panelCartItem.Controls)
            {
                if (control is CartItem cartItem)
                {
                    // เก็บข้อมูลของแต่ละสินค้าลงใน string array
                    string[] itemInfo = new string[4];
                    itemInfo[0] = cartItem.ItemName;
                    itemInfo[1] = cartItem.ItemPrice.ToString(); // แปลงเป็น string ก่อนเพราะราคาเป็นตัวเลข
                    itemInfo[2] = cartItem.ItemQuantity.ToString(); // แปลงเป็น string ก่อนเพราะปริมาณเป็นตัวเลข
                    itemInfo[3] = cartItem.ItemPerUnitPrice.ToString(); // แปลงเป็น string ก่อนเพราะราคาต่อหน่วยเป็นตัวเลข

                    // เพิ่มข้อมูลของสินค้าในรูปแบบของ string array เข้าไปใน List
                    itemsData.Add(itemInfo);
                }
            }

            
            // ส่งข้อมูลไปยัง ReceiptGenerator
            ReceiptGenerator receiptGenerator = new ReceiptGenerator();
            receiptGenerator.GenerateReceipt(itemsData);

            panelCartItem.Controls.Clear();
        }

        private void guna2TextBox_search_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = guna2TextBox_search.Text.Trim().ToLower();
            foreach (Control control in panelItems.Controls)
            {
                if (control is btnProduct btn)
                {
                    if (btn.ItemName.ToLower().Contains(searchQuery))
                    {
                        btn.Visible = true;
                    }
                    else
                    {
                        btn.Visible = false;
                    }
                }
            }
        }

        private void guna2Button_EditProduct_Click(object sender, EventArgs e)
        {
            EditProduct.EditProduct_Load();
        }

        public void SqlRefresh()
        {
            panelItems.Controls.Clear();
            DbHelper db = new DbHelper();
            db.GetButtons("SELECT * FROM public.product_item;", panelItems);
            CatRefresh();

        }
        public void CatRefresh()
        {
            guna2ComboBox_Category.Items.Clear();
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();
                try
                {
                    string categorySQL = "SELECT category_id , category_name FROM category";
                    using (NpgsqlCommand command = new NpgsqlCommand(categorySQL, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            guna2ComboBox_Category.DisplayMember = "Item1";
                            while (reader.Read())
                            {
                                string Category_name = reader["category_name"].ToString();
                               
                                guna2ComboBox_Category.Items.Add(Tuple.Create(Category_name));
                            }
                            guna2ComboBox_Category.Items.Add("All");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ไม่สามารถอัปเดตข้อมูล", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally { connection.Close(); }
            }
        }
            private void guna2Button_Reload_Click(object sender, EventArgs e)
        {
            SqlRefresh();
        }

        private void guna2Button_Setting_Click(object sender, EventArgs e)
        {
            Profile_Form.NewProfile();
        }
    }
}
