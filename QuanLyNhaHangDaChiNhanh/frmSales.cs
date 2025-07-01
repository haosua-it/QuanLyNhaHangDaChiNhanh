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
    public partial class frmSales : Form
    {
        public frmSales()
        {
            InitializeComponent();

        }
        // phân trang
        //private Stack<KhuyenMai> undoStack = new Stack<KhuyenMai>();
        //private Stack<KhuyenMai> redoStack = new Stack<KhuyenMai>();
        //phân trang
        private int currentPage = 1;
        private int pageSize = 10;
        //==================
        private bool isAdding = false;
        private bool isEditing = false;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // NÚT BACK Ở ĐÂY
        {
            this.Hide();
            frmDashBoard f = new frmDashBoard();
            f.FormClosed += (s, args) => this.Close();
            f.Show(); 
        }   

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmSales_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            LoadKhuyenMaiPhanTrang();

            HamXuLy.FillCombo("SELECT * FROM LOAIKHUYENMAI", cbLoaiKhuyenMai, "TENLOAI", "MALOAI");
            cbTrangThai.Items.Add("Áp dụng");
            cbTrangThai.Items.Add("Không áp dụng");
            EnableForm(false);
        }
        
        //=========================HÀM LOAD KHUYẾN MÃI======================
        private void LoadKhuyenMaiPhanTrang()
        {
            DataTable dt = HamXuLy.ShowKhuyenMaiPhanTrang(currentPage, pageSize);
            luoiKhuyenMai.DataSource = dt;

            // Cấu hình hiển thị
            luoiKhuyenMai.Columns["MAKM"].HeaderText = "Mã KM";
            luoiKhuyenMai.Columns["TENKM"].HeaderText = "Tên KM";
            luoiKhuyenMai.Columns["GIATRI"].HeaderText = "Giá trị";
            luoiKhuyenMai.Columns["DIEUKIEN"].HeaderText = "Điều kiện";
            luoiKhuyenMai.Columns["NGAYBATDAU"].HeaderText = "Bắt đầu";
            luoiKhuyenMai.Columns["NGAYKETTHUC"].HeaderText = "Kết thúc";
            luoiKhuyenMai.Columns["TRANGTHAI"].HeaderText = "Trạng thái";
            luoiKhuyenMai.Columns["MALOAI"].HeaderText = "Loại";

            luoiKhuyenMai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiKhuyenMai.ReadOnly = true;
            luoiKhuyenMai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            luoiKhuyenMai.Columns["NGAYBATDAU"].DefaultCellStyle.Format = "dd/MM/yyyy";
            luoiKhuyenMai.Columns["NGAYKETTHUC"].DefaultCellStyle.Format = "dd/MM/yyyy";
            luoiKhuyenMai.Columns["GIATRI"].DefaultCellStyle.Format = "N0";

            lblPage.Text = "Trang " + currentPage.ToString();

        }
        private void ClearForm() // HÀM XÓA CÁC KÍ TỰ TRÊN PANEL THÔNG TIN ĐỂ THÊM NHÂN VIÊN MỚI
        {
            txtTenKhuyenMai.Text = "";
            txtDieuKien.Text = "";
            txtGiaTri.Text = "";

            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now;
            cbLoaiKhuyenMai.SelectedIndex = 0;
            cbTrangThai.SelectedIndex = 0;

        }

        private void EnableForm(bool enable)
        {
            txtMaKhuyenMai.ReadOnly = true;
            txtTenKhuyenMai.Enabled = enable;
            txtGiaTri.Enabled = enable;
            txtDieuKien.Enabled = enable;
            dtpNgayBatDau.Enabled = enable;
            dtpNgayKetThuc.Enabled = enable;
            cbLoaiKhuyenMai.Enabled = enable;
            cbTrangThai.Enabled = enable;
            btnSave.Enabled = enable;
        }

        //================================RESET BTN

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

        private void luoiKhuyenMai_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableForm(true);
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoiKhuyenMai.Rows[e.RowIndex];
                txtMaKhuyenMai.Text = row.Cells["MAKM"].Value.ToString();
                txtTenKhuyenMai.Text = row.Cells["TENKM"].Value.ToString();
                txtGiaTri.Text = row.Cells["GIATRI"].Value.ToString();
                txtDieuKien.Text = row.Cells["DIEUKIEN"].Value.ToString();
                dtpNgayBatDau.Value = Convert.ToDateTime(row.Cells["NGAYBATDAU"].Value);
                dtpNgayKetThuc.Value = Convert.ToDateTime(row.Cells["NGAYKETTHUC"].Value);
                cbTrangThai.Text = row.Cells["TRANGTHAI"].Value.ToString();
                cbLoaiKhuyenMai.SelectedValue = row.Cells["MALOAI"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(true);
            isAdding = true;
            isEditing = false;
            btnAdd.Enabled = btnEdit.Enabled = btnDelete.Enabled = btnSearch.Enabled = false;
            btnSave.Enabled = btnCancel.Enabled = btnBack.Enabled = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoiKhuyenMai.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi để sửa!");
                return;
            }
            EnableForm(true);
            isAdding = false;
            isEditing = true;
            btnAdd.Enabled = btnEdit.Enabled = btnDelete.Enabled = btnSearch.Enabled = false;
            btnSave.Enabled = btnCancel.Enabled = btnBack.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            KhuyenMai km = GetCurrentKhuyenMaiFromForm();
            //undoStack.Push(km);
            //redoStack.Clear();

            string sql = "";
            if (isAdding)
            {
                sql = string.Format("INSERT INTO KHUYENMAI (TENKM, GIATRI, DIEUKIEN, NGAYBATDAU, NGAYKETTHUC, TRANGTHAI, MALOAI) " +
                    "VALUES (N'{0}', {1}, N'{2}', '{3}', '{4}', N'{5}', '{6}')",
                    km.TenKM, km.GiaTri, km.DieuKien,
                    km.NgayBatDau.ToString("yyyy-MM-dd"),
                    km.NgayKetThuc.ToString("yyyy-MM-dd"),
                    km.TrangThai, km.MaLoai);
                MessageBox.Show("Đã thêm khuyến mãi!");
            }
            else if (isEditing)
            {
                sql = string.Format("UPDATE KHUYENMAI SET TENKM = N'{0}', GIATRI = {1}, DIEUKIEN = N'{2}', " +
                    "NGAYBATDAU = '{3}', NGAYKETTHUC = '{4}', TRANGTHAI = N'{5}', MALOAI = '{6}' " +
                    "WHERE MAKM = '{7}'",
                    km.TenKM, km.GiaTri, km.DieuKien,
                    km.NgayBatDau.ToString("yyyy-MM-dd"),
                    km.NgayKetThuc.ToString("yyyy-MM-dd"),
                    km.TrangThai, km.MaLoai, km.MaKM);
                MessageBox.Show("Đã cập nhật khuyến mãi!");
            }

            HamXuLy.RunSQL(sql);
            LoadKhuyenMaiPhanTrang();
            ResetButtonState();
            EnableForm(false);
            ClearForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (luoiKhuyenMai.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn khuyến mãi để xóa!");
                return;
            }

            string maKM = luoiKhuyenMai.SelectedRows[0].Cells["MAKM"].Value.ToString();
            DialogResult r = MessageBox.Show("Xóa khuyến mãi này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                string sql = "DELETE FROM KHUYENMAI WHERE MAKM = '" + maKM + "'";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã xóa khuyến mãi!");
                LoadKhuyenMaiPhanTrang();
                ClearForm();
                EnableForm(false);
                ResetButtonState();
            }
        }
        private KhuyenMai GetCurrentKhuyenMaiFromForm()
        {
            int giaTri = 0;
            int.TryParse(txtGiaTri.Text.Trim(), out giaTri); // phòng ngừa lỗi nhập chữ

            string maLoai = "";
            if (cbLoaiKhuyenMai.SelectedValue != null)
                maLoai = cbLoaiKhuyenMai.SelectedValue.ToString();

            return new KhuyenMai
            {
                MaKM = txtMaKhuyenMai.Text.Trim(),
                TenKM = txtTenKhuyenMai.Text.Trim(),
                GiaTri = giaTri,
                DieuKien = txtDieuKien.Text.Trim(),
                NgayBatDau = dtpNgayBatDau.Value,
                NgayKetThuc = dtpNgayKetThuc.Value,
                TrangThai = cbTrangThai.Text,
                MaLoai = maLoai
            };
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(false);
            ResetButtonState();
            isAdding = isEditing = false;
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadKhuyenMaiPhanTrang();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadKhuyenMaiPhanTrang();
            }
        }

    }
}
