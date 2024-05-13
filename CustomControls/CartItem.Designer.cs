namespace POS_MoonDust.CustomControls
{
    partial class CartItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnQuantity = new Guna.UI2.WinForms.Guna2CircleButton();
            this.lbl_ItemName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_PerItemPrice = new System.Windows.Forms.Label();
            this.lbl_ItemPrice = new System.Windows.Forms.Label();
            this.guna2Button_DecreaseQuantity = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button_Remove = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // btnQuantity
            // 
            this.btnQuantity.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnQuantity.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnQuantity.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnQuantity.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnQuantity.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(60)))));
            this.btnQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuantity.ForeColor = System.Drawing.Color.White;
            this.btnQuantity.Location = new System.Drawing.Point(3, 3);
            this.btnQuantity.Name = "btnQuantity";
            this.btnQuantity.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnQuantity.Size = new System.Drawing.Size(53, 47);
            this.btnQuantity.TabIndex = 0;
            this.btnQuantity.Text = "1";
            this.btnQuantity.Click += new System.EventHandler(this.btnQuantity_Click);
            // 
            // lbl_ItemName
            // 
            this.lbl_ItemName.AutoSize = true;
            this.lbl_ItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Bold);
            this.lbl_ItemName.ForeColor = System.Drawing.Color.Black;
            this.lbl_ItemName.Location = new System.Drawing.Point(62, 3);
            this.lbl_ItemName.Name = "lbl_ItemName";
            this.lbl_ItemName.Size = new System.Drawing.Size(101, 16);
            this.lbl_ItemName.TabIndex = 2;
            this.lbl_ItemName.Text = "ProductName";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(62, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Price | One";
            // 
            // lbl_PerItemPrice
            // 
            this.lbl_PerItemPrice.AutoSize = true;
            this.lbl_PerItemPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PerItemPrice.ForeColor = System.Drawing.Color.Red;
            this.lbl_PerItemPrice.Location = new System.Drawing.Point(156, 20);
            this.lbl_PerItemPrice.Name = "lbl_PerItemPrice";
            this.lbl_PerItemPrice.Size = new System.Drawing.Size(44, 20);
            this.lbl_PerItemPrice.TabIndex = 4;
            this.lbl_PerItemPrice.Text = "0.00";
            // 
            // lbl_ItemPrice
            // 
            this.lbl_ItemPrice.AutoSize = true;
            this.lbl_ItemPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ItemPrice.ForeColor = System.Drawing.Color.Black;
            this.lbl_ItemPrice.Location = new System.Drawing.Point(62, 37);
            this.lbl_ItemPrice.Name = "lbl_ItemPrice";
            this.lbl_ItemPrice.Size = new System.Drawing.Size(32, 13);
            this.lbl_ItemPrice.TabIndex = 5;
            this.lbl_ItemPrice.Text = "0.00";
            // 
            // guna2Button_DecreaseQuantity
            // 
            this.guna2Button_DecreaseQuantity.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button_DecreaseQuantity.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button_DecreaseQuantity.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button_DecreaseQuantity.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button_DecreaseQuantity.FillColor = System.Drawing.Color.Red;
            this.guna2Button_DecreaseQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.guna2Button_DecreaseQuantity.ForeColor = System.Drawing.Color.White;
            this.guna2Button_DecreaseQuantity.Location = new System.Drawing.Point(224, 0);
            this.guna2Button_DecreaseQuantity.Name = "guna2Button_DecreaseQuantity";
            this.guna2Button_DecreaseQuantity.Size = new System.Drawing.Size(43, 53);
            this.guna2Button_DecreaseQuantity.TabIndex = 6;
            this.guna2Button_DecreaseQuantity.Text = "-1";
            this.guna2Button_DecreaseQuantity.Click += new System.EventHandler(this.guna2Button_DecreaseQuantity_Click);
            // 
            // guna2Button_Remove
            // 
            this.guna2Button_Remove.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button_Remove.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button_Remove.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button_Remove.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button_Remove.FillColor = System.Drawing.Color.Maroon;
            this.guna2Button_Remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.guna2Button_Remove.ForeColor = System.Drawing.Color.White;
            this.guna2Button_Remove.Location = new System.Drawing.Point(267, 0);
            this.guna2Button_Remove.Name = "guna2Button_Remove";
            this.guna2Button_Remove.Size = new System.Drawing.Size(48, 53);
            this.guna2Button_Remove.TabIndex = 7;
            this.guna2Button_Remove.Text = "RM";
            this.guna2Button_Remove.Click += new System.EventHandler(this.guna2Button_Remove_Click);
            // 
            // CartItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.guna2Button_Remove);
            this.Controls.Add(this.guna2Button_DecreaseQuantity);
            this.Controls.Add(this.lbl_ItemPrice);
            this.Controls.Add(this.lbl_PerItemPrice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_ItemName);
            this.Controls.Add(this.btnQuantity);
            this.Name = "CartItem";
            this.Size = new System.Drawing.Size(315, 53);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2CircleButton btnQuantity;
        private System.Windows.Forms.Label lbl_ItemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_PerItemPrice;
        private System.Windows.Forms.Label lbl_ItemPrice;
        private Guna.UI2.WinForms.Guna2Button guna2Button_DecreaseQuantity;
        private Guna.UI2.WinForms.Guna2Button guna2Button_Remove;
    }
}
