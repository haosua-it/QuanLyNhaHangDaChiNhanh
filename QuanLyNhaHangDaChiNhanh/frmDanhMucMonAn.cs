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
    public partial class frmDanhMucMonAn : Form
    {
        public frmDanhMucMonAn()
        {
            InitializeComponent();
        }
        // thao tác hoàn tác
        private Stack<Danhmucmonan> undoStack = new Stack<Danhmucmonan>();
        private Stack<Danhmucmonan> redoStack = new Stack<Danhmucmonan>();

        private int currentPage = 1;
        private int pageSize = 10;

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void frmDanhMucMonAn_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            LoadDanhMucMonAnPhanTrang();
            EnableForm(false);

            
        }
        private void LoadDanhMucMonAnPhanTrang()
        {
            DataTable dt = HamXuLy.ShowDanhMucMonAnPhanTrang(currentPage, pageSize);
            luoidanhmucmonan.DataSource = dt;

            luoidanhmucmonan.Columns["MADANHMUC"].HeaderText = "Mã danh mục";
            luoidanhmucmonan.Columns["TENDANHMUC"].HeaderText = "Tên danh mục";

            luoidanhmucmonan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoidanhmucmonan.ReadOnly = true;
            luoidanhmucmonan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblPage.Text = string.Format("Trang {0}", currentPage);
        }
        private void luoidanhmucmonan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableForm(true);
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoidanhmucmonan.Rows[e.RowIndex];

                txtMaDanhMuc.Text = row.Cells["MADANHMUC"].Value.ToString();
                txtTenDanhMuc.Text = row.Cells["TENDANHMUC"].Value.ToString();
            }
        }
        private void EnableForm(bool enable)
        {
            txtMaDanhMuc.Enabled = enable;
            txtTenDanhMuc.Enabled = enable;

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
            btnBack.Enabled = false;
        }
        private void ClearForm() // HÀM XÓA CÁC KÍ TỰ TRÊN PANEL THÔNG TIN DANH MỤC MÓN ĂN
        {
            txtMaDanhMuc.Text = "";
            txtTenDanhMuc.Text = "";
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
            Danhmucmonan currentDMMA = new Danhmucmonan
            {
                MaDanhMuc = txtMaDanhMuc.Text,
                TenDanhMuc = txtTenDanhMuc.Text
            };
            undoStack.Push(currentDMMA);
            redoStack.Clear();
            string madanhmuc = txtMaDanhMuc.Text.Trim();
            string tendanhmuc = txtTenDanhMuc.Text.Trim();

            HamXuLy.Connect();
            if (isAdding)
            {
                string sql = "INSERT INTO DANHMUCMON (TENDANHMUC) VALUES (N'"+tendanhmuc+"')";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm danh mục món ăn mới!");
            }
            else if (isEditing)
            {
                string danhmucmon = txtMaDanhMuc.Text; ;
                string sql = "UPDATE DANHMUCMONAN SET " +
                             "TENDANHMUC = N'" + tendanhmuc + "' " +
                             "WHERE MADANHMUC = '" + madanhmuc + "'";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật thông tin danh mục món ăn!");
            }
            LoadDanhMucMonAnPhanTrang();
            HamXuLy.Disconnect();
            ResetButtonState();
            EnableForm(false);
            ClearForm();

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoidanhmucmonan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableForm(true);

            isEditing = true;
            isAdding = false;

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
            if (luoidanhmucmonan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string madanhmuc = luoidanhmucmonan.SelectedRows[0].Cells["MADANHMUC"].Value.ToString();

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    HamXuLy.Connect();
                    string sql = "DELETE FROM DANHMUCMON WHERE MADANHMUC = '" +madanhmuc+ "'";
                    HamXuLy.RunSQL(sql);
                    HamXuLy.Disconnect();

                    MessageBox.Show("Đã xóa danh mục thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhMucMonAnPhanTrang();
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
        private Danhmucmonan GetCurrentDanhMucFromForm()
        {
            return new Danhmucmonan
            {
                MaDanhMuc = txtMaDanhMuc.Text,
                TenDanhMuc = txtTenDanhMuc.Text,
            };
        }

                private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                Danhmucmonan dmma = redoStack.Pop();
                undoStack.Push(GetCurrentDanhMucFromForm());

                txtMaDanhMuc.Text = dmma.MaDanhMuc;
                txtTenDanhMuc.Text =dmma.TenDanhMuc;

                EnableForm(true);
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Không có thao tác nào để làm lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                Danhmucmonan dmma = undoStack.Pop();
                redoStack.Push(GetCurrentDanhMucFromForm());

                txtMaDanhMuc.Text = dmma.MaDanhMuc;
                txtTenDanhMuc.Text =dmma.TenDanhMuc;

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
            string keyword = txtTenDanhMuc.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadDanhMucMonAnPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = "SELECT * FROM DANHMUCMON WHERE TENDANHMUC LIKE N'%" + keyword + "%'";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoidanhmucmonan.DataSource = dt;
            HamXuLy.Disconnect();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadDanhMucMonAnPhanTrang();
        }
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhMucMonAnPhanTrang();
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
        }
    }

