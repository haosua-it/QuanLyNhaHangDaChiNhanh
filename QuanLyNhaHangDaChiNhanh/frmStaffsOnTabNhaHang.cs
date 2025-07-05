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
    public partial class frmStaffsOnTabNhaHang : Form
    {
        private Stack<NhanVien> undoStack = new Stack<NhanVien>();
        private Stack<NhanVien> redoStack = new Stack<NhanVien>();
        private bool isAdding = false;
        private bool isEditing = false;

        public frmStaffsOnTabNhaHang()
        {
            InitializeComponent();
        }

        private void luoiNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
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
                cbChucVu.SelectedValue = row.Cells["MACHUCVU"].Value.ToString();
                dtpNgayVaoLam.Value = Convert.ToDateTime(row.Cells["NGAYVAOLAM"].Value);
                cbTrangThai.Text = row.Cells["TRANGTHAI"].Value.ToString();
            }
        }


        private void LoadNhanVienTheoChiNhanh()
        {
            try
            {
                if (string.IsNullOrEmpty(Session.MaChiNhanh))
                {
                    MessageBox.Show("Không xác định được mã chi nhánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maChiNhanh = Session.MaChiNhanh;
                DataTable dt = HamXuLy.ShowNhanVienTheoChiNhanh(maChiNhanh);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có nhân viên nào thuộc chi nhánh này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                luoiNhanVien.DataSource = dt;

                // Format ngày
                if (luoiNhanVien.Columns.Contains("NGAYSINH"))
                    luoiNhanVien.Columns["NGAYSINH"].DefaultCellStyle.Format = "dd/MM/yyyy";

                if (luoiNhanVien.Columns.Contains("NGAYVAOLAM"))
                    luoiNhanVien.Columns["NGAYVAOLAM"].DefaultCellStyle.Format = "dd/MM/yyyy";

                // Cấu hình tiêu đề (Header Text)
                luoiNhanVien.Columns["MANHANVIEN"].HeaderText = "Mã NV";
                luoiNhanVien.Columns["MALOAI"].HeaderText = "Mã loại";
                luoiNhanVien.Columns["HOTEN"].HeaderText = "Họ tên";
                luoiNhanVien.Columns["NGAYSINH"].HeaderText = "Ngày sinh";
                luoiNhanVien.Columns["CCCD"].HeaderText = "CCCD";
                luoiNhanVien.Columns["SODIENTHOAI"].HeaderText = "SĐT";
                luoiNhanVien.Columns["DIACHI"].HeaderText = "Địa chỉ";
                luoiNhanVien.Columns["MACHINHANH"].HeaderText = "Mã chi nhánh";
                luoiNhanVien.Columns["NGAYVAOLAM"].HeaderText = "Ngày vào làm";
                luoiNhanVien.Columns["TRANGTHAI"].HeaderText = "Trạng thái";
                luoiNhanVien.Columns["MACHUCVU"].HeaderText = "Chức vụ";

                luoiNhanVien.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                luoiNhanVien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                luoiNhanVien.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                luoiNhanVien.EnableHeadersVisualStyles = false;

                luoiNhanVien.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                luoiNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Cột vừa khung
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải nhân viên theo chi nhánh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frmStaffsOnTabNhaHang_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            HamXuLy.FillCombo("SELECT * FROM LOAINHANVIEN", cbLoaiNhanVien, "TENLOAI", "MALOAI");
            HamXuLy.FillCombo("SELECT * FROM CHINHANH", cbChiNhanh, "TENCHINHANH", "MACHINHANH");
            HamXuLy.FillCombo("SELECT * FROM CHUCVU", cbChucVu, "TENCHUCVU", "MACHUCVU");

            cbTrangThai.Items.Add("Đang làm");
            cbTrangThai.Items.Add("Đã nghỉ");

            LoadNhanVienTheoChiNhanh();
            EnableForm(false);

        }

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
            cbTrangThai.Enabled = enable;
            cbChucVu.Enabled = enable;
            btnSave.Enabled = enable;
        }
        private void ClearFields()
        {
            txtMaNhanVien.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtCCCD.Clear();
            cbLoaiNhanVien.SelectedIndex = -1;
            cbChiNhanh.SelectedIndex = -1;
            cbChucVu.SelectedIndex = -1;
            cbTrangThai.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Today;
            dtpNgayVaoLam.Value = DateTime.Today;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(true);
            //txtMaNhanVien.Text = HamXuLy.MaTuDong("NHANVIEN");

            isAdding = true;
            isEditing = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNhanVien.Text)) return;

            EnableForm(true);
            isEditing = true;
            isAdding = false;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            var nv = GetCurrentNhanVienFromForm();
        string sql;

        if (isAdding)
        {
            sql = string.Format(@"INSERT INTO NHANVIEN (
                MALOAI, HOTEN, NGAYSINH, CCCD, SODIENTHOAI,
                DIACHI, MACHINHANH, NGAYVAOLAM, TRANGTHAI, MACHUCVU
                ) VALUES (
                '{0}', N'{1}', '{2}', '{3}', '{4}',
                N'{5}', '{6}', '{7}', N'{8}', '{9}')",
                nv.MaLoai, nv.HoTen, nv.NgaySinh.ToString("yyyy-MM-dd"), nv.CCCD, nv.SDT,
                nv.DiaChi, nv.MaChiNhanh, nv.NgayVaoLam.ToString("yyyy-MM-dd"), nv.TrangThai, nv.MaChucVu);

            MessageBox.Show("Thêm thành công!");
        }
        else if (isEditing)
        {
            sql = string.Format(@"UPDATE NHANVIEN SET MALOAI = '{1}', HOTEN = N'{2}', NGAYSINH = '{3:yyyy-MM-dd}', 
                CCCD = '{4}', SODIENTHOAI = '{5}', DIACHI = N'{6}', 
                MACHINHANH = '{7}', NGAYVAOLAM = '{8:yyyy-MM-dd}', 
                TRANGTHAI = N'{9}', MACHUCVU = '{10}'
                WHERE MANHANVIEN = '{0}'",
                nv.MaNV, nv.MaLoai, nv.HoTen, nv.NgaySinh, nv.CCCD,
                nv.SDT, nv.DiaChi, nv.MaChiNhanh, nv.NgayVaoLam, nv.TrangThai, nv.MaChucVu);

            MessageBox.Show("Sửa thành công!");
        }
        else return;

        HamXuLy.RunSQL(sql);
        LoadNhanVienTheoChiNhanh();
        EnableForm(false);
        ClearForm();
        isAdding = isEditing = false;

        }

        private void ClearForm()
        {
            txtMaNhanVien.Clear(); txtHoTen.Clear(); txtCCCD.Clear(); txtSDT.Clear(); txtDiaChi.Clear();
            dtpNgaySinh.Value = dtpNgayVaoLam.Value = DateTime.Now;
            cbLoaiNhanVien.SelectedIndex = cbChiNhanh.SelectedIndex = cbTrangThai.SelectedIndex = cbChucVu.SelectedIndex = 0;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNhanVien.Text)) return;

            string maNV = txtMaNhanVien.Text;
            var confirm = MessageBox.Show("Xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                var oldNV = GetCurrentNhanVienFromForm();
                undoStack.Push(oldNV);
                redoStack.Clear();

                string sql = "DELETE FROM NHANVIEN WHERE MANHANVIEN = '" + maNV + "'";

                HamXuLy.RunSQL(sql);
                MessageBox.Show("Xóa thành công!");

                LoadNhanVienTheoChiNhanh();
                ClearForm();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Chức năng in chưa hoàn thiện.");
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count == 0)
            {
                MessageBox.Show("Không có thao tác nào để làm lại!");
                return;
            }

            var nv = redoStack.Pop();
            undoStack.Push(GetCurrentNhanVienFromForm());

            string sql = "DELETE FROM NHANVIEN WHERE MANHANVIEN = '{nv.MaNV}'";
            HamXuLy.RunSQL(sql);
            LoadNhanVienTheoChiNhanh();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count == 0)
            {
                MessageBox.Show("Không có thao tác nào để hoàn tác!");
                return;
            }

            var nv = undoStack.Pop();
            redoStack.Push(GetCurrentNhanVienFromForm());
            string sql = string.Format(@"
            INSERT INTO NHANVIEN 
            (MALOAI, HOTEN, NGAYSINH, CCCD, SODIENTHOAI, DIACHI, MACHINHANH, NGAYVAOLAM, TRANGTHAI, MACHUCVU)
            VALUES ('{0}', N'{1}', '{2:yyyy-MM-dd}', '{3}', '{4}', N'{5}', '{6}', '{7:yyyy-MM-dd}', N'{8}', '{9}')",
            nv.MaLoai, nv.HoTen, nv.NgaySinh, nv.CCCD, nv.SDT, nv.DiaChi,
            nv.MaChiNhanh, nv.NgayVaoLam, nv.TrangThai, nv.MaChucVu);


            HamXuLy.RunSQL(sql);
            LoadNhanVienTheoChiNhanh();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableForm(false);
            ClearForm();
            isAdding = isEditing = false;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtHoTen.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadNhanVienTheoChiNhanh();
                return;
            }

            string sql = "SELECT * FROM NHANVIEN WHERE HOTEN LIKE N'%" + keyword + "%' AND MACHINHANH = '" + Session.MaChiNhanh + "'";

            luoiNhanVien.DataSource = HamXuLy.GetDataToTable(sql);
        }


    }
}
