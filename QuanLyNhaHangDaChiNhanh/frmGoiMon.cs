using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaHangDaChiNhanh
{
    public partial class frmGoiMon: Form
    {
        private int maHoaDon;
        public frmGoiMon(int maHoaDon)
        {
            InitializeComponent();
            this.maHoaDon = maHoaDon;

            // Có thể gọi hàm hiển thị dữ liệu theo bàn ở đây
            LoadThongTinBan();
        }
        private void LoadThongTinBan()
        {
            // Load dữ liệu gọi món theo maBan
        }
        private void frmGoiMon_Load(object sender, EventArgs e)
        {

        }
    }
}
