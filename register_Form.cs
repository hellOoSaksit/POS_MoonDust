using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

//เพิ่มเติม
using Npgsql;
using POS_MoonDust.Class;


namespace POS_MoonDust
{
    public partial class register_Form : Form
    {
        public register_Form()
        {
            InitializeComponent();
        }
        public static void Register()
        {
            register_Form register_Form = new register_Form();
            register_Form.ShowDialog();
        }
        private void guna2CheckBox_ShowPassword_1CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox_ShowPassword_1.Checked)
            {
                guna2TextBox_Password.UseSystemPasswordChar = false;
                guna2TextBox_Password.PasswordChar = '\0';
            }
            else
            {
                guna2TextBox_Password.UseSystemPasswordChar = true;
                guna2TextBox_Password.PasswordChar = '●';
            }
            
        }

        private void guna2CheckBox_ShowPassword_2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox_ShowPassword_2.Checked)
            {
                guna2TextBox_ConfirmPassword.UseSystemPasswordChar = false;
                guna2TextBox_ConfirmPassword.PasswordChar= '\0';
            }
            else
            {
                guna2TextBox_ConfirmPassword.UseSystemPasswordChar = true;
                guna2TextBox_ConfirmPassword.PasswordChar = '●';
            }
        }

        bool Admin = false;
        private void guna2ToggleSwitch_RME_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch_RME.Checked)
            {
                Admin = true;
            }
            else
            {
                Admin = false;
            }
        }

        private void guna2Button_register_Click(object sender, EventArgs e)
        {
            //Username
            if (string.IsNullOrEmpty(guna2TextBox_username.Text))
            {
                // แสดงข้อความแจ้งเตือนว่าต้องป้อนชื่อผู้ใช้
                MessageBox.Show("กรุณาป้อนชื่อผู้ใช้");
                return;
            }

            if (!guna2TextBox_username.Text.All(c => char.IsLetter(c) || c == ' '))
            {
                // แสดงข้อความแจ้งเตือนว่าชื่อผู้ใช้ต้องเป็นภาษาอังกฤษ
                MessageBox.Show("ชื่อผู้ใช้ต้องเป็นภาษาอังกฤษ");
                return;
            }

            if (guna2TextBox_username.Text.Length < 3)
            {
                // แสดงข้อความแจ้งเตือนว่าชื่อผู้ใช้ต้องมีความยาวอย่างน้อย 3 ตัวอักษร
                MessageBox.Show("ชื่อผู้ใช้ต้องมีความยาวอย่างน้อย 3 ตัวอักษร");
                return;
            }
            //Password
            if (string.IsNullOrEmpty(guna2TextBox_Password.Text))
            {
                // แสดงข้อความแจ้งเตือนว่าต้องป้อนรหัสผ่าน
                MessageBox.Show("กรุณาป้อนรหัสผ่าน");
                return;
            }

            if (guna2TextBox_Password.Text.Length < 8)
            {
                // แสดงข้อความแจ้งเตือนว่ารหัสผ่านต้องมีความยาวอย่างน้อย 8 ตัวอักษร
                MessageBox.Show("รหัสผ่านต้องมีความยาวอย่างน้อย 8 ตัวอักษร");
                return;
            }

            if (!guna2TextBox_Password.Text.Any(c => char.IsUpper(c)))
            {
                // แสดงข้อความแจ้งเตือนว่ารหัสผ่านต้องมีตัวอักษรตัวใหญ่
                MessageBox.Show("รหัสผ่านต้องมีตัวอักษรตัวใหญ่");
                return;
            }

            if (!guna2TextBox_Password.Text.Any(c => !char.IsLetterOrDigit(c)))
            {
                // แสดงข้อความแจ้งเตือนว่ารหัสผ่านต้องมีตัวอักษรพิเศษ
                MessageBox.Show("รหัสผ่านต้องมีตัวอักษรพิเศษ");
                return;
            }
            //Email
            if (string.IsNullOrEmpty(guna2TextBox_email.Text))
            {
                // แสดงข้อความแจ้งเตือนว่าต้องป้อนชื่อผู้ใช้
                MessageBox.Show("กรุณาป้อน Email");
                return;
            }
            //Confim Password
            if (guna2TextBox_Password.Text != guna2TextBox_ConfirmPassword.Text)
            {
                MessageBox.Show("กรุณาป้อนรหัสผ่าน ให้ตรงกัน");
                return;
            }
            //Full Name
            if (string.IsNullOrEmpty(guna2TextBox_FullName.Text))
            {
                MessageBox.Show("กรุณาใส่ชื่อจริงของคุณ"); 
                return;
            }
            try 
            {
                // ตรวจสอบว่ามี username ในระบบหรือไม่
                using (var connection = new NpgsqlConnection(Conn.connectionString))
                {
                    connection.Open();

                    var checkUsernameCommand = new NpgsqlCommand("SELECT COUNT(*) FROM public.users WHERE username = @username", connection);
                    checkUsernameCommand.Parameters.AddWithValue("@username", guna2TextBox_username.Text);

                    var usernameExists = (long)checkUsernameCommand.ExecuteScalar() > 0;
                    if (usernameExists)
                    {
                        MessageBox.Show("ชื่อผู้ใช้นี้มีอยู่ในระบบแล้ว");
                        return;
                    }
                    connection.Close();
                }

                // ตรวจสอบว่ามี email ในระบบหรือไม่
                using (var connection = new NpgsqlConnection(Conn.connectionString))
                {
                    connection.Open();

                    var checkEmailCommand = new NpgsqlCommand("SELECT COUNT(*) FROM public.users WHERE email = @email", connection);
                    checkEmailCommand.Parameters.AddWithValue("@email", guna2TextBox_email.Text);

                    var emailExists = (long)checkEmailCommand.ExecuteScalar() > 0;

                    if (emailExists)
                    {
                        MessageBox.Show("อีเมลนี้มีอยู่ในระบบแล้ว");
                        return;
                    }
                    connection.Close();
                }

                // เข้ารหัสรหัสผ่าน
                var encodedPassword = EncodePasswordToSHA256(guna2TextBox_Password.Text);

                // บันทึกข้อมูลผู้ใช้ลงในฐานข้อมูล
                using (var connection = new NpgsqlConnection(Conn.connectionString))
                {
                    connection.Open();

                    var command = new NpgsqlCommand("INSERT INTO public.users (username, password, name, email, admin) VALUES (@username, @password, @name, @email, @admin)", connection);
                    command.Parameters.AddWithValue("@username", guna2TextBox_username.Text);
                    command.Parameters.AddWithValue("@password", encodedPassword);
                    command.Parameters.AddWithValue("@name", guna2TextBox_FullName.Text); // แทน name ด้วยข้อมูลที่ต้องการบันทึก
                    command.Parameters.AddWithValue("@email", guna2TextBox_email.Text);
                    command.Parameters.AddWithValue("@admin", Admin); // แทน Admin ด้วยข้อมูลที่ต้องการบันทึก
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            } 
            catch(Exception ex) 
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
            
        }

        public string EncodePasswordToSHA256(string password)
        {
            // เข้ารหัสรหัสผ่านด้วย SHA256
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString;
            }
        }
        private void register_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
