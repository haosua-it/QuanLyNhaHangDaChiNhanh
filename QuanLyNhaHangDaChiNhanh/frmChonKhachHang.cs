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
    public partial class frmChonKhachHang : Form
    {
        public string MaKhachHangDuocChon;
        public string TenKhachHangDuocChon;

        public frmChonKhachHang()
        {
            InitializeComponent();
        }

        private void LoadDanhSachKhach()
        {
            string sql = "SELECT * FROM KHACHHANG";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            dgvKhachHang.DataSource = dt;

        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

                    if (row.Cells["MAKH"].Value != null && row.Cells["TENKH"].Value != null)
                    {
                        MaKhachHangDuocChon = row.Cells["MAKH"].Value.ToString();
                        TenKhachHangDuocChon = row.Cells["TENKH"].Value.ToString();

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể đọc dữ liệu khách hàng. Kiểm tra lại tên cột!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn khách hàng: " + ex.Message);
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTenKhach.Text.Trim();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng cần tìm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sql = "SELECT * FROM KHACHHANG WHERE TENKH LIKE @ten";
            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "@ten", "%" + tuKhoa + "%" }
            };

            DataTable dt = HamXuLy.GetDataToTable(sql, param);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy khách hàng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dgvKhachHang.DataSource = dt; 
            }
        }

        private void frmChonKhachHang_Load(object sender, EventArgs e)
        {
            LoadDanhSachKhach();
            dgvKhachHang.CellDoubleClick += dgvKhachHang_CellDoubleClick; 
        }
    }
}
