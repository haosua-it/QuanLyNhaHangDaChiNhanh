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
    public partial class frmStaffs : Form
    {
        public frmStaffs()
        {
            InitializeComponent();
        }
        // thao tác hoàn tác
        private Stack<NhanVien> undoStack = new Stack<NhanVien>();
        private Stack<NhanVien> redoStack = new Stack<NhanVien>();

        // phân trang
        private int currentPage = 1;
        private int pageSize = 10;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmStaffs_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();

            // Load ComboBox loại nhân viên
            HamXuLy.FillCombo("SELECT * FROM LOAINHANVIEN", cbLoaiNhanVien, "TENLOAI", "MALOAI");

            // Load ComboBox chi nhánh
            HamXuLy.FillCombo("SELECT * FROM CHINHANH", cbChiNhanh, "TENCHINHANH", "MACHINHANH");

            // Load ComboBox chức vụ
            HamXuLy.FillCombo("SELECT * FROM CHUCVU", cbChucVu, "TENCHUCVU", "MACHUCVU");

            // Trạng thái
            cbTrangThai.Items.Add("Đang làm");
            cbTrangThai.Items.Add("Đã nghỉ");

            // Load danh sách nhân viên mặc định
            LoadNhanVienPhanTrang();
            EnableForm(false);
        }
        private void LoadNhanVienPhanTrang()
        {
            DataTable dt = HamXuLy.ShowNhanVienPhanTrang(currentPage, pageSize);
            luoiNhanVien.DataSource = dt;

            luoiNhanVien.Columns["MANHANVIEN"].HeaderText = "Mã nhân viên";
            luoiNhanVien.Columns["HOTEN"].HeaderText = "Họ tên";
            luoiNhanVien.Columns["NGAYSINH"].HeaderText = "Ngày sinh";
            luoiNhanVien.Columns["CCCD"].HeaderText = "CCCD";
            luoiNhanVien.Columns["SODIENTHOAI"].HeaderText = "SĐT";
            luoiNhanVien.Columns["DIACHI"].HeaderText = "Địa chỉ";
            luoiNhanVien.Columns["MALOAI"].HeaderText = "Loại NV";
            luoiNhanVien.Columns["MACHINHANH"].HeaderText = "Chi nhánh";
            luoiNhanVien.Columns["NGAYVAOLAM"].HeaderText = "Ngày vào làm";
            luoiNhanVien.Columns["TRANGTHAI"].HeaderText = "Trạng thái";
            luoiNhanVien.Columns["MACHUCVU"].HeaderText = "Chức vụ";

            luoiNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiNhanVien.ReadOnly = true;
            luoiNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lblPage.Text = string.Format("Trang {0}", currentPage);

        }



        private void luoiNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableForm(true); 
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoiNhanVien.Rows[e.RowIndex];

                txtMaNhanVien.Text = row.Cells["MANHANVIEN"].Value.ToString();
                txtHoTen.Text = row.Cells["HOTEN"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NGAYSINH"].Value);
                txtCCCD.Text = row.Cells["CCCD"].Value.ToString();
                txtSDT.Text = row.Cells["SODIENTHOAI"].Value.ToString();
                txtDiaChi.Text = row.Cells["DIACHI"].Value.ToString();
                cbLoaiNhanVien.SelectedValue = row.Cells["MALOAI"].Value.ToString();
                cbChiNhanh.SelectedValue = row.Cells["MACHINHANH"].Value.ToString();
                dtpNgayVaoLam.Value = Convert.ToDateTime(row.Cells["NGAYVAOLAM"].Value);
                cbTrangThai.Text = row.Cells["TRANGTHAI"].Value.ToString();
                cbChucVu.SelectedValue = row.Cells["MACHUCVU"].Value.ToString();
            }
        }
        //=================================== HÀM DÙNG ĐỂ KHÓA/ MỞ CÁC Ô NHẬP LIỆU
        private void EnableForm(bool enable) 
        {
            txtMaNhanVien.ReadOnly = true;
            txtHoTen.Enabled = enable;
            txtCCCD.Enabled = enable;
            txtSDT.Enabled = enable;
            txtDiaChi.Enabled = enable;
            dtpNgaySinh.Enabled = enable;
            dtpNgayVaoLam.Enabled = enable;
            cbLoaiNhanVien.Enabled = enable;
            cbChiNhanh.Enabled = enable;
            cbTrangThai.Enabled = enable;
            cbChucVu.Enabled = enable;
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


        private void ClearForm() // HÀM XÓA CÁC KÍ TỰ TRÊN PANEL THÔNG TIN ĐỂ THÊM NHÂN VIÊN MỚI
        {
            txtHoTen.Text = "";
            txtCCCD.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            dtpNgayVaoLam.Value = DateTime.Now;
            cbLoaiNhanVien.SelectedIndex = 0;
            cbChiNhanh.SelectedIndex = 0;
            cbTrangThai.SelectedIndex = 0;
            cbChucVu.SelectedIndex = 0;
        }

        private bool isAdding = false;
        private bool isEditing = false;

        //============================ CHỨC NĂNG THÊM NHÂN VIÊN Ở ĐÂY

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

        //=========================== CHỨC NĂNG LƯU Ở ĐÂY, LƯU CỦA THÊM VÀ CỦA SỬA
        private void btnSave_Click(object sender, EventArgs e) 
        {
            // Trước khi sửa hoặc xóa
            NhanVien currentNV = new NhanVien
            {
                MaNV = txtMaNhanVien.Text,
                HoTen = txtHoTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                CCCD = txtCCCD.Text,
                SDT = txtSDT.Text,
                DiaChi = txtDiaChi.Text,
                MaLoai = cbLoaiNhanVien.SelectedValue.ToString(),
                MaChiNhanh = cbChiNhanh.SelectedValue.ToString(),
                NgayVaoLam = dtpNgayVaoLam.Value,
                TrangThai = cbTrangThai.Text,
                MaChucVu = cbChucVu.SelectedValue.ToString()
            };
            undoStack.Push(currentNV);
            redoStack.Clear(); // Khi có thao tác mới, clear redoStack
            string maLoai = cbLoaiNhanVien.SelectedValue.ToString();
            string hoTen = txtHoTen.Text.Trim();
            string ngaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
            string cccd = txtCCCD.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string maCN = cbChiNhanh.SelectedValue.ToString();
            string ngayVaoLam = dtpNgayVaoLam.Value.ToString("yyyy-MM-dd");
            string trangThai = cbTrangThai.Text;
            string maChucVu = cbChucVu.SelectedValue.ToString();

            HamXuLy.Connect();

            if (isAdding)
            {
                string sql = "INSERT INTO NHANVIEN (MALOAI, HOTEN, NGAYSINH, CCCD, SODIENTHOAI, DIACHI, MACHINHANH, NGAYVAOLAM, TRANGTHAI, MACHUCVU) " +
                             "VALUES (N'" + maLoai + "', N'" + hoTen + "', '" + ngaySinh + "', '" + cccd + "', '" + sdt + "', N'" + diaChi + "', '" + maCN + "', '" + ngayVaoLam + "', N'" + trangThai + "', '" + maChucVu + "')";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm nhân viên!");
            }
            else if (isEditing)
            {
                string maNV = txtMaNhanVien.Text;
                string sql = "UPDATE NHANVIEN SET " +
                             "MALOAI = N'" + maLoai + "', " +
                             "HOTEN = N'" + hoTen + "', " +
                             "NGAYSINH = '" + ngaySinh + "', " +
                             "CCCD = '" + cccd + "', " +
                             "SODIENTHOAI = '" + sdt + "', " +
                             "DIACHI = N'" + diaChi + "', " +
                             "MACHINHANH = '" + maCN + "', " +
                             "NGAYVAOLAM = '" + ngayVaoLam + "', " +
                             "TRANGTHAI = N'" + trangThai + "', " +
                             "MACHUCVU = '" + maChucVu + "' " +
                             "WHERE MANHANVIEN = '" + maNV + "'";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật thông tin nhân viên!");
            }
            LoadNhanVienPhanTrang();
            HamXuLy.Disconnect();
            ResetButtonState();
            EnableForm(false);
            ClearForm();

        }
        //============================ CHÚC NĂNG NÚT SỬA

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoiNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (luoiNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = luoiNhanVien.SelectedRows[0].Cells["MANHANVIEN"].Value.ToString();

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    HamXuLy.Connect();
                    string sql = "DELETE FROM NHANVIEN WHERE MANHANVIEN = '" + maNV + "'";
                    HamXuLy.RunSQL(sql);
                    HamXuLy.Disconnect();

                    MessageBox.Show("Đã xóa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNhanVienPhanTrang();
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

        private NhanVien GetCurrentNhanVienFromForm()
        {
            return new NhanVien
            {
                MaNV = txtMaNhanVien.Text,
                HoTen = txtHoTen.Text,
                NgaySinh = dtpNgaySinh.Value,
                CCCD = txtCCCD.Text,
                SDT = txtSDT.Text,
                DiaChi = txtDiaChi.Text,
                MaLoai = cbLoaiNhanVien.SelectedValue == null ? "" : cbLoaiNhanVien.SelectedValue.ToString(),
                MaChiNhanh = cbChiNhanh.SelectedValue == null ? "" : cbChiNhanh.SelectedValue.ToString(),
                NgayVaoLam = dtpNgayVaoLam.Value,
                TrangThai = cbTrangThai.Text,
                MaChucVu = cbChucVu.SelectedValue == null ? "" : cbChucVu.SelectedValue.ToString()
            };
        }


        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                NhanVien nv = redoStack.Pop();
                undoStack.Push(GetCurrentNhanVienFromForm());

                // Áp dụng lại dữ liệu
                txtMaNhanVien.Text = nv.MaNV;
                txtHoTen.Text = nv.HoTen;
                dtpNgaySinh.Value = nv.NgaySinh;
                txtCCCD.Text = nv.CCCD;
                txtSDT.Text = nv.SDT;
                txtDiaChi.Text = nv.DiaChi;
                cbLoaiNhanVien.SelectedValue = nv.MaLoai;
                cbChiNhanh.SelectedValue = nv.MaChiNhanh;
                dtpNgayVaoLam.Value = nv.NgayVaoLam;
                cbTrangThai.Text = nv.TrangThai;
                cbChucVu.SelectedValue = nv.MaChucVu;

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
                NhanVien nv = undoStack.Pop();
                redoStack.Push(GetCurrentNhanVienFromForm());

                // Áp dụng dữ liệu vào form
                txtMaNhanVien.Text = nv.MaNV;
                txtHoTen.Text = nv.HoTen;
                dtpNgaySinh.Value = nv.NgaySinh;
                txtCCCD.Text = nv.CCCD;
                txtSDT.Text = nv.SDT;
                txtDiaChi.Text = nv.DiaChi;
                cbLoaiNhanVien.SelectedValue = nv.MaLoai;
                cbChiNhanh.SelectedValue = nv.MaChiNhanh;
                dtpNgayVaoLam.Value = nv.NgayVaoLam;
                cbTrangThai.Text = nv.TrangThai;
                cbChucVu.SelectedValue = nv.MaChucVu;

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
            string keyword = txtHoTen.Text.Trim(); 
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadNhanVienPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = "SELECT * FROM NHANVIEN WHERE HOTEN LIKE N'%" + keyword + "%'";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoiNhanVien.DataSource = dt;
            HamXuLy.Disconnect();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDashBoard f = new frmDashBoard(); 
            f.Show(); 
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadNhanVienPhanTrang();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadNhanVienPhanTrang();
            }
        }




    }
}
