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
    public partial class frmSupplier : Form
    {
        private Stack<NhaCungCap> undoStack = new Stack<NhaCungCap>();
        private Stack<NhaCungCap> redoStack = new Stack<NhaCungCap>();

        private int currentPage = 1;
        private int pageSize = 10;
        private bool isAdding = false;
        private bool isEditing = false;

        public frmSupplier()
        {
            InitializeComponent();
        }

        private void frmSupplier_Load(object sender, EventArgs e)
        {
            LoadNhaCungCapPhanTrang();
            EnableForm(false);
            ResetButtonState();
        }
        private void LoadNhaCungCapPhanTrang()
        {
            try
            {
                HamXuLy.Connect();

                int startRow = (currentPage - 1) * pageSize + 1;
                int endRow = currentPage * pageSize;

                string sql = string.Format(@"
            SELECT * FROM (
                SELECT *, ROW_NUMBER() OVER (ORDER BY MANCC) AS RowNum
                FROM NHACUNGCAP
            ) AS Sub
            WHERE RowNum BETWEEN {0} AND {1}", startRow, endRow);

                DataTable dt = HamXuLy.GetDataToTable(sql);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị!");
                }

                luoiNCC.DataSource = dt;

                // Gán tên cột rõ ràng
                luoiNCC.Columns["MANCC"].HeaderText = "Mã NCC";
                luoiNCC.Columns["TENNCC"].HeaderText = "Tên nhà cung cấp";
                luoiNCC.Columns["DIACHI"].HeaderText = "Địa chỉ";
                luoiNCC.Columns["SODIENTHOAI"].HeaderText = "Số điện thoại";
                luoiNCC.Columns["EMAIL"].HeaderText = "Email";

                // Ẩn cột RowNum
                if (luoiNCC.Columns.Contains("RowNum"))
                {
                    luoiNCC.Columns["RowNum"].Visible = false;
                }

                luoiNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                luoiNCC.ReadOnly = true;
                luoiNCC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                lblPage.Text = "Trang " + currentPage.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                HamXuLy.Disconnect();
            }
        }




        private void luoiNCC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableForm(true);
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoiNCC.Rows[e.RowIndex];
                txtMaNCC.Text = row.Cells["MANCC"].Value.ToString();
                txtTenNCC.Text = row.Cells["TENNCC"].Value.ToString();
                txtDiaChi.Text = row.Cells["DIACHI"].Value.ToString();
                txtSDT.Text = row.Cells["SODIENTHOAI"].Value.ToString();
                txtEmail.Text = row.Cells["EMAIL"].Value.ToString();
            }
        }
        private void EnableForm(bool enable)
        {
            txtMaNCC.ReadOnly = true;
            txtTenNCC.Enabled = enable;
            txtDiaChi.Enabled = enable;
            txtSDT.Enabled = enable;
            txtEmail.Enabled = enable;
            btnSave.Enabled = enable;
        }

        private void ClearForm()
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtDiaChi.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
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
        }

        private NhaCungCap GetCurrentNCCFromForm()
        {
            return new NhaCungCap
            {
                MaNCC = txtMaNCC.Text,
                TenNCC = txtTenNCC.Text,
                DiaChi = txtDiaChi.Text,
                SoDienThoai = txtSDT.Text,
                Email = txtEmail.Text
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(true);
            isAdding = true;
            isEditing = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoiNCC.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để sửa.");
                return;
            }
            EnableForm(true);
            isAdding = false;
            isEditing = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NhaCungCap ncc = GetCurrentNCCFromForm();
            undoStack.Push(ncc);
            redoStack.Clear();
            HamXuLy.Connect();

            if (isAdding)
            {
                string sql = string.Format(
                    "INSERT INTO NHACUNGCAP (TENNCC, DIACHI, SODIENTHOAI, EMAIL) " +
                    "VALUES (N'{0}', N'{1}', '{2}', '{3}')",
                    ncc.TenNCC, ncc.DiaChi, ncc.SoDienThoai, ncc.Email
                );
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm nhà cung cấp.");
            }
            else if (isEditing)
            {
                string sql = string.Format(
                    "UPDATE NHACUNGCAP SET TENNCC = N'{0}', DIACHI = N'{1}', SODIENTHOAI = '{2}', EMAIL = '{3}' " +
                    "WHERE MANCC = '{4}'",
                    ncc.TenNCC, ncc.DiaChi, ncc.SoDienThoai, ncc.Email, ncc.MaNCC
                );
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật thông tin nhà cung cấp.");

            }

            LoadNhaCungCapPhanTrang();
            ResetButtonState();
            EnableForm(false);
            ClearForm();
            HamXuLy.Disconnect();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (luoiNCC.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa.");
                return;
            }

            string maNCC = luoiNCC.SelectedRows[0].Cells["MANCC"].Value.ToString();
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                HamXuLy.Connect();
                string sql = string.Format("DELETE FROM NHACUNGCAP WHERE MANCC = '{0}'", maNCC);
                HamXuLy.RunSQL(sql);
                LoadNhaCungCapPhanTrang();
                ClearForm();
                EnableForm(false);
                ResetButtonState();
                HamXuLy.Disconnect();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                NhaCungCap ncc = redoStack.Pop();
                undoStack.Push(GetCurrentNCCFromForm());
                txtMaNCC.Text = ncc.MaNCC;
                txtTenNCC.Text = ncc.TenNCC;
                txtDiaChi.Text = ncc.DiaChi;
                txtSDT.Text = ncc.SoDienThoai;
                txtEmail.Text = ncc.Email;
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                NhaCungCap ncc = undoStack.Pop();
                redoStack.Push(GetCurrentNCCFromForm());
                txtMaNCC.Text = ncc.MaNCC;
                txtTenNCC.Text = ncc.TenNCC;
                txtDiaChi.Text = ncc.DiaChi;
                txtSDT.Text = ncc.SoDienThoai;
                txtEmail.Text = ncc.Email;
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
             string keyword = txtTenNCC.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadNhaCungCapPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = string.Format("SELECT * FROM NHACUNGCAP WHERE TENNCC LIKE N'%{0}%'", keyword);
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoiNCC.DataSource = dt;
            HamXuLy.Disconnect();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDashBoard f = new frmDashBoard();
            f.FormClosed += (s, args) => this.Close();
            f.Show(); 
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadNhaCungCapPhanTrang();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadNhaCungCapPhanTrang();
        }

    }

}
