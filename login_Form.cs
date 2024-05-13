using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using POS_MoonDust.Class;

namespace POS_MoonDust
{
    public partial class login_Form : Form
    {
        private LoginService loginService;

        public login_Form()
        {
            InitializeComponent();
            loginService = new LoginService();
        }
        public static void Login_Form()
        {
            login_Form loginForm = new login_Form();
            loginForm.Show();
        }
        private void guna2Button_login_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = guna2TextBox_username.Text;
            string password = guna2TextBox_Password.Text;

            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("กรุณากรอกชื่อผู้ใช้หรืออีเมลและรหัสผ่าน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool loginSuccess = loginService.Login(usernameOrEmail, password);

            if (loginSuccess)
            {
                MessageBox.Show("เข้าสู่ระบบสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // ทำต่อไปที่หน้าหลักหรือหน้าที่ต้องการเปิดหลังจาก Login สำเร็จ
                this.Hide();
                MainLoad.MainLoadForm();

            }
            else
            {
                MessageBox.Show("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2TextBox_Password.Clear();
            }
        }

        private void login_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void guna2ToggleSwitch_RME_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch_RME.Checked)
            {
                Properties.Settings.Default.UserRemembered = guna2TextBox_username.Text;
            }
            else
            {
                Properties.Settings.Default.UserRemembered = null;
            }
            Properties.Settings.Default.Save();
        }

        private void login_Form_Load(object sender, EventArgs e)
        {
            string rememberedUsername = Properties.Settings.Default.UserRemembered;
            if (!string.IsNullOrEmpty(rememberedUsername))
            {
                guna2TextBox_username.Text = rememberedUsername;
                guna2ToggleSwitch_RME.Checked = true;
            }
        }
    }

    public class LoginService
    {

        public bool Login(string usernameOrEmail, string password)
        {
            // เข้ารหัสรหัสผ่าน
                var encodedPassword = EncodePasswordToSHA256(password);
        try {
                // ดึงข้อมูลผู้ใช้จากฐานข้อมูล
                using (var connection = new NpgsqlConnection(Conn.connectionString))
                {
                    connection.Open();

                    // ตรวจสอบว่า usernameOrEmail เป็น username หรือ email
                    string query;
                    if (IsEmail(usernameOrEmail))
                    {
                        query = "SELECT * FROM public.users WHERE email = @usernameOrEmail AND password = @password";
                    }
                    else
                    {
                        // ใช้ username เข้าสู่ระบบ
                        query = "SELECT * FROM users WHERE username = @usernameOrEmail AND password = @password";
                    }

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@usernameOrEmail", usernameOrEmail);
                    command.Parameters.AddWithValue("@password", encodedPassword);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // สร้างอ็อบเจกต์ Data_User จากข้อมูลในฐานข้อมูล
                            Data_User currentUser = new Data_User
                            {
                                U_username = reader["username"].ToString(),
                                U_name = reader["name"].ToString(),
                                U_email = reader["email"].ToString(),
                                IsAdmin = Convert.ToBoolean(reader["admin"])
                            };

                            // เก็บข้อมูลผู้ใช้ในตัวแปร Global
                            Global.currentUser = currentUser;
                            // รหัสผ่านถูกต้อง อนุญาตให้เข้าถึงระบบ
                            return true;
                        }
                        else
                        {
                            // รหัสผ่านไม่ถูกต้อง
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("ไม่สามารถเชื่อมต่อกับเซิฟเวอร์ได้ ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private bool IsEmail(string input)
        {
            // ตรวจสอบว่า input เป็น email หรือไม่
            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.Address == input;
            }
            catch
            {
                return false;
            }
        }

        private string EncodePasswordToSHA256(string password)
        {
            // เข้ารหัสรหัสผ่านด้วย SHA256
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                return hashString;
            }
        }



    }
}
