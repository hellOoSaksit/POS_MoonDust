using POS_MoonDust.Class;
using System;
using System.Windows.Forms;

namespace POS_MoonDust.CustomControls
{
    public partial class CartItem : UserControl
    {
        public CartItem()
        {
            InitializeComponent();
        }

        public string ItemId { get; set; }
        public string ItemPerUnitPrice
        {
            get { return lbl_PerItemPrice.Text; }
            set { lbl_PerItemPrice.Text = value; }
        }
        public string ItemName
        {
            get { return lbl_ItemName.Text; }
            set { lbl_ItemName.Text = value; }
        }
        public string ItemPrice
        {
            get { return lbl_ItemPrice.Text; }
            set { lbl_ItemPrice.Text = value; }
        }
        public string ItemQuantity
        {
            get { return btnQuantity.Text; }
            set { btnQuantity.Text = value; }
        }

        public event EventHandler QuantityChanged;

        protected virtual void OnQuantityChanged(EventArgs e)
        {
            QuantityChanged?.Invoke(this, e);
        }

        public void IncreaseQuantity()
        {
            int quantity = int.Parse(btnQuantity.Text);
            quantity++;
            btnQuantity.Text = quantity.ToString();
            CalculateTotalPrice(); // เรียกใช้เมธอดนี้เพื่อคำนวณราคารวมใหม่
            OnQuantityChanged(EventArgs.Empty);
        }

        public void DecreaseQuantity()
        {
            int quantity = int.Parse(btnQuantity.Text);
            if (quantity > 1)
            {
                quantity--;
                btnQuantity.Text = quantity.ToString();
                CalculateTotalPrice(); // เรียกใช้เมธอดนี้เพื่อคำนวณราคารวมใหม่
                OnQuantityChanged(EventArgs.Empty);
            }
        }
        public void RemoveItem()
        {
            EventArgs args = new EventArgs();
            OnItemRemoved(args);
            this.Parent.Controls.Remove(this);
            CalculateTotalPrice(); // เรียกใช้เมธอดนี้เพื่อคำนวณราคารวมใหม่
        }

        public void CalculateTotalPrice()
        {
            int quantity = int.Parse(ItemQuantity);
            float pricePerUnit = float.Parse(ItemPrice);
            float totalPrice = quantity * pricePerUnit;
            ItemPerUnitPrice = totalPrice.ToString();
        }

        public event EventHandler ItemRemoved;

        protected virtual void OnItemRemoved(EventArgs e)
        {
            ItemRemoved?.Invoke(this, e);
        }
        private void btnQuantity_Click(object sender, EventArgs e)
        {
            IncreaseQuantity();
            CalculateTotalPrice();
        }

        private void guna2Button_DecreaseQuantity_Click(object sender, EventArgs e)
        {
            DecreaseQuantity();
            CalculateTotalPrice();
        }

        private void guna2Button_Remove_Click(object sender, EventArgs e)
        {
            RemoveItem();
            CalculateTotalPrice();
        }
    }
}
