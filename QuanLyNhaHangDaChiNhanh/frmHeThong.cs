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
    public partial class frmHeThong : Form
    {
        public frmHeThong()
        {
            InitializeComponent();
        }

        private void btnQuanLyHeThong_Click(object sender, EventArgs e)
        {
            frmQuanLyHeThong f = new frmQuanLyHeThong();
            f.ShowDialog();
        }

        private void btnBaoCaoHeThong_Click(object sender, EventArgs e)
        {
            frmBaoCaoHeThong f = new frmBaoCaoHeThong();
            f.ShowDialog();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {

        }

        private void frmHeThong_Load(object sender, EventArgs e)
        {
            //timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //CultureInfo vi = new CultureInfo("vi-VN");
            //lbThoiGian.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy HH:mm:ss");
        }

    }
}
