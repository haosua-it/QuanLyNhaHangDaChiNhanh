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
    public partial class frmChiTiet : Form
    {
        private int maHD;

        public frmChiTiet(int mahd)
        {
            InitializeComponent();
            maHD = mahd;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnPrevPage_Click_1(object sender, EventArgs e)
        {

        }

        private void btnNextPage_Click_1(object sender, EventArgs e)
        {

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

        private void frmChiTiet_Load(object sender, EventArgs e)
{
    HamXuLy.Connect();
    LoadChiTiet();
}

int pageSize = 10;
int currentPage = 1;

private void LoadChiTiet()
{
    int offset = (currentPage - 1) * pageSize;
    string keyword = txtTimKiem.Text.Trim();

    string sql = string.Format("SELECT MACHITIETHD, CTHD.MAMON, M.TENMON, CTHD.SOLUONG, CTHD.DONGIA, CTHD.THANHTIEN FROM CHITIETHOADON CTHD JOIN MONAN M ON M.MAMON = CTHD.MAMON WHERE CTHD.MAHD = {0} AND M.TENMON LIKE N'%{1}%' ORDER BY M.TENMON OFFSET {2} ROWS FETCH NEXT {3} ROWS ONLY", maHD, keyword, offset, pageSize);
    LuoiChiTiet.DataSource = HamXuLy.GetDataToTable(sql);
    LuoiChiTiet.Columns["MACHITIETHD"].HeaderText = "Mã Chi Tiết";
LuoiChiTiet.Columns["MAMON"].HeaderText = "Mã Món";
LuoiChiTiet.Columns["TENMON"].HeaderText = "Tên Món";
LuoiChiTiet.Columns["SOLUONG"].HeaderText = "Số Lượng";
LuoiChiTiet.Columns["DONGIA"].HeaderText = "Đơn Giá (VNĐ)";
LuoiChiTiet.Columns["THANHTIEN"].HeaderText = "Thành Tiền (VNĐ)";

LuoiChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
LuoiChiTiet.Columns["DONGIA"].DefaultCellStyle.Format = "#,##0 VNĐ";
LuoiChiTiet.Columns["THANHTIEN"].DefaultCellStyle.Format = "#,##0 VNĐ";
LuoiChiTiet.ReadOnly = true;
LuoiChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


                lblPage.Text = string.Format("Trang {0}", currentPage);
}

private void btnSearch_Click_1(object sender, EventArgs e)
{
    currentPage = 1;
    LoadChiTiet();
}

private void btnPrevPage_Click(object sender, EventArgs e)
{
    if (currentPage > 1)
    {
        currentPage--;
        LoadChiTiet();
    }
}

private void btnNextPage_Click(object sender, EventArgs e)
{
    currentPage++;
    LoadChiTiet();
}

private void btnBack_Click_1(object sender, EventArgs e)
{
    this.Close();
}

       


    }
}
