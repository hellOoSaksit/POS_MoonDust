using System;
using System.Windows.Forms;
using Npgsql;
using POS_MoonDust.Class;
using System.Data;

namespace POS_MoonDust
{
    public partial class EditMember : Form
    {
        private DataTable userData;
        
        public EditMember()
        {
            InitializeComponent();
        }

        public static void EditMembers()
        {
            EditMember editMember = new EditMember();
            editMember.ShowDialog();
        }

        private void EditMember_Load(object sender, EventArgs e)
        {
            LoadUserNames();
        }

        private void LoadUserNames()
        {
            using (var connection = new NpgsqlConnection(Conn.connectionString))
            {
                try
                {
                    connection.Open();
                    string usernameSQL = "SELECT * FROM public.users";
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(usernameSQL, connection))
                    {
                        userData = new DataTable();
                        adapter.Fill(userData);
                        comboBox_Name.DataSource = userData;
                        comboBox_Name.DisplayMember = "username";
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
        string usernameMember;
        private void comboBox_Name_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox_Name.Text) && comboBox_Name.Text.Length >= 3)
            {
                DataRow[] foundRows = userData.Select($"username LIKE '{comboBox_Name.Text}%'");
                comboBox_Name.DataSource = foundRows.Length > 0 ? foundRows.CopyToDataTable() : userData;
                if (foundRows.Length > 0)
                {
                    DataRow selectedRow = foundRows[0]; 
                    guna2TextBox_FullName.Text = selectedRow["name"].ToString();
                    guna2TextBox_email.Text = selectedRow["email"].ToString();
                    usernameMember = selectedRow["username"].ToString();
                }
                else
                {
                    ClearTextBoxes(); 
                }
            }
            else
            {
                comboBox_Name.DataSource = userData;
                ClearTextBoxes(); 
            }
        }
        private void ClearTextBoxes()
        {
            guna2TextBox_FullName.Text = "";
            guna2TextBox_email.Text = "";
        }

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
                        users.Parameters.AddWithValue("@username", comboBox_Name.Text);

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

        bool CheckPassword;
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

        private void guna2Button_Delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox_Name.Text))
            {
                MessageBox.Show("กรุณาเลือกชื่อผู้ใช้ที่ต้องการลบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // แสดงกล่องโต้ตอบเพื่อยืนยันการลบผู้ใช้
            DialogResult result = MessageBox.Show("คุณแน่ใจหรือไม่ว่าต้องการลบผู้ใช้นี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // ทำการลบผู้ใช้
                try
                {
                    using (var connection = new NpgsqlConnection(Conn.connectionString))
                    {
                        connection.Open();
                        string deleteQuery = "DELETE FROM public.users WHERE username = @username";
                        using (var deleteCommand = new NpgsqlCommand(deleteQuery, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@username", comboBox_Name.Text);
                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("ลบผู้ใช้สำเร็จ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadUserNames(); // โหลดรายชื่อผู้ใช้ใหม่
                            }
                            else
                            {
                                MessageBox.Show("ไม่สามารถลบผู้ใช้ได้", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการลบผู้ใช้: " + ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
