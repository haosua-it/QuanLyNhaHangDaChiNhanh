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
    public partial class frmInHoaDon : Form
    {
        private int maHoaDon;

        public frmInHoaDon(int maHoaDon)
        {
            InitializeComponent();
            this.maHoaDon = maHoaDon;
        }

        private void frmInHoaDon_Load(object sender, EventArgs e)
        {
            // Gọi logic lấy dữ liệu và hiển thị hóa đơn theo maHoaDon
            // Có thể load vào ReportViewer hoặc in bằng Crystal Report hoặc viết tay với PrintDocument
        }

        private void frmInHoaDon_Load_1(object sender, EventArgs e)
        {
            
        }
    }

}
