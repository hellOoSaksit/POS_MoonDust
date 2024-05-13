using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace POS_MoonDust.CustomControls
{
    public partial class btnProduct : UserControl
    {
        private string _itemImgUrl;
        private Image _itemImage;

        public btnProduct()
        {
            InitializeComponent();
        }

        public string ItemId { get; set; }
        public string ItemCategory
        {
            get { return lbl_Category.Text; }
            set { lbl_Category.Text = value; }
        }
        public string ItemName
        {
            get { return lbl_ProductName.Text; }
            set { lbl_ProductName.Text = value; }
        }
        public string ItemPrice
        {
            get { return lbl_Price.Text; }
            set { lbl_Price.Text = value; }
        }

        public string ItemImg
        {
            get { return _itemImgUrl; }
            set
            {
                _itemImgUrl = value;
                // ตรวจสอบว่าภาพถูกโหลดแล้วหรือไม่ หากยังไม่ได้โหลด ให้โหลดภาพ
                if (_itemImage == null)
                {
                    LoadImageFromUrl(_itemImgUrl);
                }
            }
        }

        private void LoadImageFromUrl(string imageUrl)
        {
            // โหลดภาพจาก URL และเก็บไว้ในตัวแปร _itemImage
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(imageUrl);
                using (System.IO.MemoryStream mem = new System.IO.MemoryStream(data))
                {
                    _itemImage = Image.FromStream(mem);
                }
            }
            // แสดงภาพใน Guna2PictureBox
            guna2PictureBox_Product.Image = _itemImage;
        }

        private void lbl_ProductName_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
            MainLoad mainLoad = this.ParentForm as MainLoad;
            mainLoad?.AddCartItem(this.ItemId, this.ItemName, this.ItemPrice);
        }
    }
}
