using Npgsql;
using POS_MoonDust.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_MoonDust
{
    public partial class EditCategoryForm : Form
    {
        public EditCategoryForm()
        {
            InitializeComponent();
        }
        public static void EditCategorys()
        {
            EditCategoryForm editCategory = new EditCategoryForm();
            editCategory.ShowDialog();
        }
        private void guna2Button_ADD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox_Category.Text))
            {
                // แสดงข้อความแจ้งเตือนว่าต้องป้อนประเภท
                MessageBox.Show("กรุณาป้อนประเภท");
                return;
            }
            // บันทึกข้อมูลผู้ใช้ลงในฐานข้อมูล
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();
                try
                {
                    var product_item = new NpgsqlCommand("INSERT INTO public.category (category_name) " +
                        "VALUES (@category_name)", connection);
                    product_item.Parameters.AddWithValue("@category_name", guna2TextBox_Category.Text);
                    product_item.ExecuteNonQuery();

                    MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ไม่สามารถเพิ่มข้อมูลได้ โปรดต่อผู้ดูแล" + "\n" +
                        "1.ไม่สามารถอัพโหลดรูปได้" + "\n" +
                        "2.ไม่สามารถเชื่อมต่อฐานข้อมูลได้"
                        , "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { connection.Close(); }
                LoadCategories();
            }
        }
        private DataTable userData;
        private void NewCategoryForm_Load(object sender, EventArgs e)
        {
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                try
                {
                    connection.Open();
                    string usernameSQL = "SELECT * FROM public.category";
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(usernameSQL, connection))
                    {
                        userData = new DataTable();
                        adapter.Fill(userData);
                        comboBox_Category.DataSource = userData;
                        comboBox_Category.DisplayMember = "category_name";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        int _ProductID;
        private void guna2Button_Delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox_Category.Text))
            {
                MessageBox.Show("กรุณาเลือกหมวดหมู่ที่ต้องการลบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ตรวจสอบก่อนว่ามีการเลือกหมวดหมู่ที่ต้องการลบหรือไม่
            DataRow selectedRow = userData.AsEnumerable().FirstOrDefault(row => row.Field<string>("category_name") == comboBox_Category.Text);
            if (selectedRow == null)
            {
                MessageBox.Show("ไม่พบหมวดหมู่ที่เลือก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ถ้าพบหมวดหมู่ที่เลือก ให้ทำการลบออกจากฐานข้อมูล
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                try
                {
                    connection.Open();
                    var deleteCommand = new NpgsqlCommand("DELETE FROM public.category WHERE category_name = @category_name", connection);
                    deleteCommand.Parameters.AddWithValue("@category_name", comboBox_Category.Text);
                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("ลบหมวดหมู่สำเร็จ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // โหลดหมวดหมู่ใหม่หลังจากที่ลบ
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถลบหมวดหมู่ได้", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล: " + ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void LoadCategories()
        {
            // โหลดหมวดหมู่ใหม่ลงใน ComboBox
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                try
                {
                    connection.Open();
                    string usernameSQL = "SELECT * FROM public.category";
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(usernameSQL, connection))
                    {
                        userData = new DataTable();
                        adapter.Fill(userData);
                        comboBox_Category.DataSource = userData;
                        comboBox_Category.DisplayMember = "category_name";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }
}
