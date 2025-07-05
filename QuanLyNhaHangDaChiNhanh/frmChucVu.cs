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
    public partial class frmChucVu : Form
    {
        public frmChucVu()
        {
            InitializeComponent();
        }

        private void frmChucVu_Load(object sender, EventArgs e)
        {
            txtMaCV.Text = HamXuLy.MaTuDong("CHUCVU");
            Show();
        }
        private void ShowChucVuForm()
        {
            HamXuLy.Connect();
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM CHUCVU";
            if (HamXuLy.TruyVan(sql, dt))
            {
                dtgvChucVu.DataSource = dt;
                dtgvChucVu.Columns[0].HeaderText = "Mã Chức Vụ";
                dtgvChucVu.Columns[1].HeaderText = "Tên Chức Vụ";
                dtgvChucVu.EnableHeadersVisualStyles = false;
                dtgvChucVu.ColumnHeadersDefaultCellStyle.BackColor = Color.Cyan;
            } 
        }
    }
}
