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
using static Dropbox.Api.TeamLog.ActorLogInfo;

namespace POS_MoonDust
{
    public partial class Profile_Form : Form
    {
        public Profile_Form()
        {
            InitializeComponent();
        }
        public static void NewProfile()
        {
           Profile_Form profile_Form = new Profile_Form();
           profile_Form.ShowDialog();
        }




        string query;
        bool CheckPassword ;
        //ฝึกหัดเขียนโค้ดให้สั่นลด การใช้ if else แบบ ? : 
        private void guna2Button_Edit_Click(object sender, EventArgs e)
        {
            string query;
            try
            {
                using (var connection = new NpgsqlConnection(Conn.connectionString))
                {
                    connection.Open();
                    query = CheckPassword
                     /*True*/    ? "UPDATE public.users SET password = @password, name = @name, email = @email WHERE username = @username"
                     /*False*/   : "UPDATE public.users SET name = @name, email = @email WHERE username = @username";

                    using (var users = new NpgsqlCommand(query, connection))
                    {
                        users.Parameters.AddWithValue("@name", guna2TextBox_FullName.Text);
                        users.Parameters.AddWithValue("@email", guna2TextBox_email.Text);
                        users.Parameters.AddWithValue("@username", Global.currentUser.U_username);

                        if (CheckPassword)
                        {
                            register_Form register_Form = new register_Form();
                            string EncodePassword = register_Form.EncodePasswordToSHA256(guna2TextBox_NewPassword.Text);
                            users.Parameters.AddWithValue("@password", EncodePassword);
                        }

                        int rowsAffected = users.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("อัปเดตข้อมูลสำเร็จ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("ไม่สามารถอัปเดตข้อมูล", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ไม่สามารถอัปเดตข้อมูลได้: " + ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Profile_Form_Load(object sender, EventArgs e)
        {
             query = "SELECT * FROM public.users WHERE username = @username";

            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", Global.currentUser.U_username);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guna2TextBox_username.Text = reader["username"].ToString();
                            guna2TextBox_email.Text = reader["email"].ToString();
                            guna2TextBox_FullName.Text = reader["name"].ToString();
                        }

                    }
                }

                connection.Close();
            }
        }

        private void guna2ToggleSwitch_RME_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch_RME.Checked)
            {
                guna2Panel_EditPassword.Enabled = true;
                CheckPassword = true;
            }
            else
            {
                guna2Panel_EditPassword.Enabled = false;
                CheckPassword = false;
            }
        }

        private void guna2CheckBox_ShowPassword_1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox_ShowPassword_1.Checked)
            {
                guna2TextBox_NewPassword.UseSystemPasswordChar = false;
                guna2TextBox_NewPassword.PasswordChar = '\0';
            }
            else
            {
                guna2TextBox_NewPassword.UseSystemPasswordChar = true;
                guna2TextBox_NewPassword.PasswordChar = '●';
            }
        }

        private void guna2Button_register_Click(object sender, EventArgs e)
        {
            register_Form.Register();
        }

        private void guna2Button_EditMember_Click(object sender, EventArgs e)
        {
            EditMember.EditMembers();
        }
    }
}
