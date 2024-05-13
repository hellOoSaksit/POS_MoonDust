using Npgsql;
using POS_MoonDust.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

public class ReceiptGenerator
{
    public void GenerateReceipt(List<string[]> itemsData)
    {
        // สร้างคำสั่ง SQL สำหรับแทรกข้อมูลบิล
        string insertSql = "INSERT INTO bills (order_number, itemdata, date, time) VALUES (@orderNumber, @itemData::jsonb, @date, @time)";

        // เชื่อมต่อกับฐานข้อมูล
        using (var connection = new NpgsqlConnection(Conn.connectionString))
        {
            connection.Open();

            // เริ่ม Transaction
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // เตรียมคำสั่ง SQL และพารามิเตอร์
                    using (var cmd = new NpgsqlCommand(insertSql, connection, transaction))
                    {
                        // กำหนดหมายเลขออเดอร์
                        int orderNumber = GetNextOrderNumber(connection, transaction);

                        foreach (string[] item in itemsData)
                        {
                            // คำนวณราคารวมของสินค้า
                            decimal itemPriceTotal = Convert.ToDecimal(item[1]) * Convert.ToDecimal(item[2]);

                            // สร้างข้อมูล JSON จากชื่อสินค้า ปริมาณ ราคาต่อหน่วย ราคารวม วันที่ และเวลา
                            string jsonData = $"{{\"itemName\": \"{item[0]}\", \"itemQuantity\": {item[1]}, \"itemPerUnitPrice\": {item[2]}, \"itemPriceTotal\": {itemPriceTotal}, \"date\": \"{DateTime.Now.Date}\", \"time\": \"{DateTime.Now.TimeOfDay}\"}}";

                            // กำหนดค่าพารามิเตอร์
                            cmd.Parameters.AddWithValue("orderNumber", orderNumber);
                            cmd.Parameters.AddWithValue("itemData", jsonData);
                            cmd.Parameters.AddWithValue("date", DateTime.Now.Date);
                            cmd.Parameters.AddWithValue("time", DateTime.Now.TimeOfDay);

                            // ประมวลผลคำสั่ง SQL
                            cmd.ExecuteNonQuery();

                            // ล้างค่าพารามิเตอร์เพื่อการใช้งานครั้งถัดไป
                            cmd.Parameters.Clear();
                        }
                    }

                    // Commit Transaction เมื่อทุกอย่างเสร็จสมบูรณ์
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback Transaction ในกรณีเกิดข้อผิดพลาด
                    transaction.Rollback();

                    MessageBox.Show("ไม่สามารถเชื่อมต่อกับเซิฟเวอร์ได้ ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    // ฟังก์ชันสำหรับดึงหมายเลขออเดอร์ถัดไป
    private int GetNextOrderNumber(NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        string selectSql = "SELECT COALESCE(MAX(order_number), 0) + 1 FROM bills";

        using (var cmd = new NpgsqlCommand(selectSql, connection, transaction))
        {
            // ดึงค่าหมายเลขออเดอร์จากฐานข้อมูล
            object result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }
    }
}
