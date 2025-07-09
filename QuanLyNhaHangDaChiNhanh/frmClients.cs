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
    public partial class frmClients : Form
    {
        public frmClients()
        {
            InitializeComponent();
        }
        private Stack<Clients> undoStack = new Stack<Clients>();
        private Stack<Clients> redoStack = new Stack<Clients>();

        // phân trang
        private int currentPage = 1;
        private int pageSize = 10;
        private void frmClients_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();

            ShowKhachHangPhanTrang();
            EnableForm(false);
        }
        private void ShowKhachHangPhanTrang()
        {
            DataTable dt = HamXuLy.ShowKhachHangPhanTrang(currentPage, pageSize);
            luoikhachhang.DataSource = dt;

            luoikhachhang.Columns["MAKH"].HeaderText = "Mã Khách Hàng";
            luoikhachhang.Columns["TENKH"].HeaderText = "Tên Khách Hàng";
            luoikhachhang.Columns["SODIENTHOAI"].HeaderText = "Số Điện Thoại";
            luoikhachhang.Columns["EMAIL"].HeaderText = "Email";
            luoikhachhang.Columns["DIEMTICHLUY"].HeaderText = "Điểm Tích Lũy";

            luoikhachhang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoikhachhang.ReadOnly = true;
            luoikhachhang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblPage.Text = string.Format("Trang {0}", currentPage);
        }
        private void EnableForm(bool enable)
        {
            txtMaKhachHang.ReadOnly = true;
            txtTenKhachHang.Enabled = enable;
            txtSoDienThoai.Enabled = enable;
            txtEmail.Enabled = enable;
            txtDiemTichLuy.Enabled = enable;
            btnSave.Enabled = enable;
        }

        private void luoikhachhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableForm(true);
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoikhachhang.Rows[e.RowIndex];

                txtMaKhachHang.Text = row.Cells["MAKH"].Value.ToString();
                txtTenKhachHang.Text = row.Cells["TENKH"].Value.ToString();
                txtSoDienThoai.Text = row.Cells["SODIENTHOAI"].Value.ToString();
                txtEmail.Text = row.Cells["EMAIL"].Value.ToString();
                txtDiemTichLuy.Text = row.Cells["DIEMTICHLUY"].Value.ToString();

            }
        }
        private void ResetButtonState()
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnUndo.Enabled = true;
            btnRedo.Enabled = true;
            btnBack.Enabled = false;
        }
        private void ClearForm() // HÀM XÓA CÁC KÍ TỰ TRÊN PANEL THÔNG TIN ĐỂ THÊM NHÂN VIÊN MỚI
        {
            txtMaKhachHang.Text = "";
            txtTenKhachHang.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";
            txtDiemTichLuy.Text = "";
        }
        private bool isAdding = false;
        private bool isEditing = false;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(true);

            isAdding = true;
            isEditing = false;

            // Trạng thái các nút khi Thêm
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnUndo.Enabled = false;
            btnRedo.Enabled = false;
            btnBack.Enabled = true;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Trước khi sửa hoặc xóa
            Clients currentBA = new Clients
            {
                MaKhachHang = txtMaKhachHang.Text,
                TenKhachHang = txtTenKhachHang.Text,
                SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text,
                DiemTichLuy = txtDiemTichLuy.Text
            };
            undoStack.Push(currentBA);
            redoStack.Clear(); // Khi có thao tác mới, clear redoStack
            string makhachhang = txtMaKhachHang.Text.Trim();
            string tenkhachhang = txtTenKhachHang.Text.Trim();
            string sodienthoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diemtichluy = txtDiemTichLuy.Text.Trim();

            HamXuLy.Connect();

            if (isAdding)
            {
                string sql = "INSERT INTO KHACHHANG (MAKH, TENKH, SODIENTHOAI, EMAIL, DIEMTICHLUY) VALUES ('" + makhachhang + "', N'" + tenkhachhang + "', '" + sodienthoai + "','" + email + "', '" + diemtichluy + "')";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm Khách hàng!");
            }
            else if (isEditing)
            {
                string MaKhachHang = txtMaKhachHang.Text;
                string sql = "UPDATE KHACHHANG SET" +
                             "TENKH = N'" + tenkhachhang + "', " +
                             "SODIENTHOAI = '" + sodienthoai + "', " +
                             "EMAIL = N'" + email + "', " +
                             "DIEMTICHLUY = '" + diemtichluy + "' " +
                             "WHERE MAKH = '" + makhachhang + "'";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật thông tin khách hàng");
            }
            ShowKhachHangPhanTrang();
            HamXuLy.Disconnect();
            ResetButtonState();
            EnableForm(false);
            ClearForm();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoikhachhang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableForm(true); // Cho phép nhập liệu

            isEditing = true;
            isAdding = false;

            // Trạng thái các nút khi sửa
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnUndo.Enabled = false;
            btnRedo.Enabled = false;
            btnBack.Enabled = true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (luoikhachhang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string makhachhang = luoikhachhang.SelectedRows[0].Cells["MAKH"].Value.ToString();

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa vị khách này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    HamXuLy.Connect();
                    string sql = "DELETE FROM KHACHHANG WHERE MAKH = '" + makhachhang + "'";
                    HamXuLy.RunSQL(sql);
                    HamXuLy.Disconnect();

                    MessageBox.Show("Đã xóa bàn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowKhachHangPhanTrang();
                    ClearForm();
                    EnableForm(false);
                    ResetButtonState();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
        private Clients GetCurrentKhachHangFromForm()
        {
            return new Clients
            {
                MaKhachHang = txtMaKhachHang.Text,
                TenKhachHang = txtTenKhachHang.Text,
                SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text,
                DiemTichLuy = txtDiemTichLuy.Text,
            };
        }
        private void btnRedo_Click(object sender, EventArgs e)
        {

        }
        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                Clients ba = redoStack.Pop();
                undoStack.Push(GetCurrentKhachHangFromForm());

                txtMaKhachHang.Text = ba.MaKhachHang;
                txtTenKhachHang.Text = ba.TenKhachHang;
                txtSoDienThoai.Text = ba.SoDienThoai;
                txtEmail.Text = ba.Email;
                txtDiemTichLuy.Text = ba.DiemTichLuy;

                EnableForm(true);
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Không có thao tác nào để hoàn tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(false);
            ResetButtonState();
            isAdding = false;
            isEditing = false;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTenKhachHang.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                ShowKhachHangPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = "SELECT * FROM KHACHHANG WHERE TENKHACHHANG LIKE N'%" + keyword + "%'";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoikhachhang.DataSource = dt;
            HamXuLy.Disconnect();
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            ShowKhachHangPhanTrang();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDashBoard f = new frmDashBoard();
            f.Show();
        }
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ShowKhachHangPhanTrang();
            }
        }
    }
}
