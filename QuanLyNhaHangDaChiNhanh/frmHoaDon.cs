using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyNhaHangDaChiNhanh
{
    public partial class frmHoaDon : Form
    {
        public frmHoaDon()
        {
            InitializeComponent();
        }
        int pageSize = 10;
int currentPage = 1;

private void LoadHoaDon()
{
    int offset = (currentPage - 1) * pageSize;
    string keyword = txtTimKiem.Text.Trim();

    string sql = string.Format("SELECT MAHD, MACHINHANH, NGAYLAP, MANV, MABAN, TONGTIENPHAITRA, HINHTHUCTHANHTOAN, TRANGTHAI, TENKHACHHANG FROM HOADON WHERE TENKHACHHANG LIKE N'%{0}%' ORDER BY NGAYLAP DESC OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", keyword, offset, pageSize);

    LuoiHoaDon.DataSource = HamXuLy.GetDataToTable(sql);
    LuoiHoaDon.Columns["MAHD"].HeaderText = "Mã Hóa Đơn";
    LuoiHoaDon.Columns["NGAYLAP"].HeaderText = "Ngày Lập";
    LuoiHoaDon.Columns["MANV"].HeaderText = "Mã Nhân Viên";
    LuoiHoaDon.Columns["MABAN"].HeaderText = "Mã Bàn";
    LuoiHoaDon.Columns["TONGTIENPHAITRA"].HeaderText = "Tổng Tiền (VNĐ)";
    LuoiHoaDon.Columns["HINHTHUCTHANHTOAN"].HeaderText = "Thanh Toán";
    LuoiHoaDon.Columns["TRANGTHAI"].HeaderText = "Trạng Thái";
    LuoiHoaDon.Columns["TENKHACHHANG"].HeaderText = "Tên Khách Hàng";

    LuoiHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    LuoiHoaDon.Columns["TONGTIENPHAITRA"].DefaultCellStyle.Format = "#,##0 VNĐ";
    LuoiHoaDon.ReadOnly = true;
    LuoiHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblPage.Text = string.Format("Trang {0}", currentPage);
}


        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnPrevPage_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadHoaDon();
            }
        }

        private void btnNextPage_Click_1(object sender, EventArgs e)
        {
            currentPage++;
            LoadHoaDon();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            string sql = "SELECT * FROM vw_HoaDonChiTiet ORDER BY NGAYLAP DESC";
            LuoiHoaDon.DataSource = HamXuLy.GetDataToTable(sql);

        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xem chi tiết.");
                return;
            }

            int maHD = int.Parse(txtMaHD.Text);
            frmChiTiet frm = new frmChiTiet(maHD);
            frm.ShowDialog();
        }

        private void LuoiHoaDon_Click(object sender, EventArgs e)
        {
            if (LuoiHoaDon.CurrentRow != null)
            {
                txtMaHD.Text = LuoiHoaDon.CurrentRow.Cells["MAHD"].Value.ToString();
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadHoaDon();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {

        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadHoaDon();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadHoaDon();
        }
       


    }
}
