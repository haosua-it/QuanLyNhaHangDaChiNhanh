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
    public partial class frmBranch : Form
    {
        public frmBranch()
        {
            InitializeComponent();
        }
        private Stack<Branch> undoStack = new Stack<Branch>();
        private Stack<Branch> redoStack = new Stack<Branch>();

        private int currentPage = 1;
        private int pageSize = 10;

        private void frmBrand_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();

            //load combobox chucvu
            HamXuLy.FillCombo("SELECT * FROM CHUCVU", cbMaChucVuQL, "TENCHUCVU", "MACHUCVU");

            cbTrangThai.Items.Add("True");
            cbTrangThai.Items.Add("False");
            LoadChiNhanhPhanTrang();
            EnableForm(false);
        }
        private void LoadChiNhanhPhanTrang()
        {
            DataTable dt = HamXuLy.ShowChiNhanhPhanTrang(currentPage, pageSize);
            luoichinhanh.DataSource = dt;

            luoichinhanh.Columns["MACHINHANH"].HeaderText = "Mã Chi Nhánh";
            luoichinhanh.Columns["TENCHINHANH"].HeaderText = "Tên Chi Nhánh";
            luoichinhanh.Columns["DIACHI"].HeaderText = "Địa Chỉ";
            luoichinhanh.Columns["SODIENTHOAI"].HeaderText = "Số Điện Thoại";
            luoichinhanh.Columns["MACHUCVUQL"].HeaderText = "Chức Vụ";
            luoichinhanh.Columns["TRANGTHAI"].HeaderText = "Trạng Thái";

            luoichinhanh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoichinhanh.ReadOnly = true;
            luoichinhanh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblPage.Text = string.Format("Trang {0}", currentPage);
        }
        private void luoichinhanh_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableForm(true);
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoichinhanh.Rows[e.RowIndex];

                txtMaChiNhanh.Text = row.Cells["MACHINHANH"].Value.ToString();
                txtTenChiNhanh.Text = row.Cells["TENCHINHANH"].Value.ToString();
                txtDiaChi.Text = row.Cells["DIACHI"].Value.ToString();
                txtSoDienThoai.Text = row.Cells["SODIENTHOAI"].Value.ToString();
                cbMaChucVuQL.Text = row.Cells["MACHUCVUQL"].Value.ToString();
                cbTrangThai.Text = row.Cells["TRANGTHAI"].Value.ToString();
            }

        }
        private void EnableForm(bool enable)
        {
            txtMaChiNhanh.ReadOnly = true;
            txtTenChiNhanh.Enabled = enable;
            txtDiaChi.Enabled = enable;
            txtSoDienThoai.Enabled = enable;
            cbMaChucVuQL.Enabled = enable;
            cbTrangThai.Enabled = enable;
            btnSave.Enabled = enable;
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
            btnBack.Enabled = true;
        }
        private void ClearForm()
        {
            txtMaChiNhanh.Text = "";
            txtTenChiNhanh.Text = "";
            txtDiaChi.Text = "";
            txtSoDienThoai.Text = "";
            cbMaChucVuQL.Text = "";
            cbTrangThai.SelectedIndex = 0;
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
            Branch currentCN = new Branch
            {
                MaChiNhanh = txtMaChiNhanh.Text,
                TenChiNhanh = txtTenChiNhanh.Text,
                DiaChi = txtDiaChi.Text,
                SoDienThoai = txtSoDienThoai.Text,
                MaChucVuQL = cbMaChucVuQL.Text,
                TrangThai = cbTrangThai.Text

            };
            undoStack.Push(currentCN);
            redoStack.Clear(); // Khi có thao tác mới, clear redoStack
            string machinhanh = txtMaChiNhanh.Text.Trim();
            string tenchinhanh = txtTenChiNhanh.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();
            string sodienthoai = txtSoDienThoai.Text.Trim();
            string machucvuql = cbMaChucVuQL.Text.Trim();
            string trangthai = cbTrangThai.Text;

            HamXuLy.Connect();

            if (isAdding)
            {
                string sql = "INSERT INTO CHINHANH (MACHINHANH, TENCHINHANH, DIACHI, SODIENTHOAI, MACHUCVUQL, TRANGTHAI) VALUES ('" + machinhanh + "', N'" + tenchinhanh + "', '" + diachi + "','" + sodienthoai + "', '" + machucvuql + "', '"+trangthai+"')";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm chi nhánh!");
            }
            else if (isEditing)
            {
                string MaChiNhanh = txtMaChiNhanh.Text;
                string sql = "UPDATE CHINHANH SET" +
                             "TENCHINHANH = N'" + tenchinhanh + "', " +
                             "DIACHI = N'" + diachi + "', " +
                             "SODIENTHOAI = '" + sodienthoai + "', " +
                             "MACHUCVUQL = '" + machucvuql + "', " +
                             "TRANGTHAI = N'" + trangthai + "'" +
                             "WHERE MACHINHANH = N'" + machinhanh + "'";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật thông tin chi nhánh");
            }
            LoadChiNhanhPhanTrang();
            HamXuLy.Disconnect();
            ResetButtonState();
            EnableForm(false);
            ClearForm();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoichinhanh.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một chi nhánh để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (luoichinhanh.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn chi nhánh cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string MaChiNhanh = luoichinhanh.SelectedRows[0].Cells["MACHINHANH"].Value.ToString();

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa chi nhánh này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    HamXuLy.Connect();
                    string sql = "DELETE FROM CHINHANH WHERE MACHINHANH = '" + MaChiNhanh + "'";
                    HamXuLy.RunSQL(sql);
                    HamXuLy.Disconnect();

                    MessageBox.Show("Đã xóa chi nhánh thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChiNhanhPhanTrang();
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
        private Branch GetCurrentChiNhanhFromForm()
        {
            return new Branch
            {
                MaChiNhanh = txtMaChiNhanh.Text,
                TenChiNhanh = txtTenChiNhanh.Text,
                SoDienThoai = txtSoDienThoai.Text,
                DiaChi = txtDiaChi.Text,
                MaChucVuQL = cbMaChucVuQL.Text,
                TrangThai = cbTrangThai.Text

            };
        }
        private void btnRedo_Click(object sender, EventArgs e)
        {

        }
        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                Branch CN = redoStack.Pop();
                undoStack.Push(GetCurrentChiNhanhFromForm());

                txtMaChiNhanh.Text = CN.MaChiNhanh;
                txtTenChiNhanh.Text = CN.TenChiNhanh;
                txtSoDienThoai.Text = CN.SoDienThoai;
                txtDiaChi.Text = CN.DiaChi;
                cbMaChucVuQL.Text = CN.MaChucVuQL;
                cbTrangThai.Text = CN.TrangThai;


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
            string keyword = txtTenChiNhanh.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadChiNhanhPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = "SELECT * FROM CHINHANH WHERE TENCHINHANH LIKE N'%" + keyword + "%'";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoichinhanh.DataSource = dt;
            HamXuLy.Disconnect();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadChiNhanhPhanTrang();
        }
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadChiNhanhPhanTrang();
            }
        }
    }
}
