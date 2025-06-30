using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace QuanLyNhaHangDaChiNhanh
{
    public partial class frmDangKy : Form
    {
        public frmDangKy()
        {
            InitializeComponent();
            txtMatKhau.UseSystemPasswordChar = true;
            txtCFPass.UseSystemPasswordChar = true;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            bool kt = kiemtrathongtin();
            if(kt == true)
            {
                int trangthai = chkTrangThai.Checked ? 1 : 0;
                string sqlInsert = "INSERT INTO NGUOIDUNG(MANGUOIDUNG,TENNGUOIDUNG,MATKHAU,HOVATEN,EMAIL,SODIENTHOAI,MACHINHANH,MACHUCVU,TRANGTHAI) VALUES(N'" + txtMaNguoiDung.Text + "', N'" + txtTenDangNhap.Text + "', N'" + txtMatKhau.Text + "', N'" + txtHoTen.Text + "', N'" + txtEmail.Text + "', N'" + txtSDT.Text + "', N'" + cboMaChiNhanh.SelectedValue + "', N'" + cboMaChucVu.SelectedValue + "', " + trangthai + ")";

                try
                {
                    HamXuLy.RunSQL(sqlInsert);
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo");
                }
            }
            else
            {

            }
        }
        private Boolean kiemtrathongtin()
        {
            HamXuLy.Connect();

            // Kiểm tra trùng tên đăng nhập
            string sql = "SELECT COUNT(*) FROM NGUOIDUNG WHERE TENNGUOIDUNG = '" + txtTenDangNhap.Text.Trim() + "'";
            int count = HamXuLy.GetCount(sql);  // Bạn cần viết thêm hàm GetCount trả về int

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Bạn chưa nhập tên tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }
            if (count > 0)
            {
                MessageBox.Show("Tên tài khoản đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            // Kiểm tra mật khẩu
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            // Kiểm tra độ dài mật khẩu (ít nhất 8 ký tự)
            if (txtMatKhau.Text.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            // Kiểm tra xác nhận mật khẩu
            if (txtCFPass.Text != txtMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCFPass.Focus();
                return false;
            }

            // Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Bạn chưa nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // Kiểm tra Email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Bạn chưa nhập email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }
            else
            {
                // Regex kiểm tra định dạng Email
                string patternEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, patternEmail))
                {
                    MessageBox.Show("Email không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // Kiểm tra Số điện thoại
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Bạn chưa nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }
            else
            {
                string patternSDT = @"^\d{10}$";  // Đúng 10 số
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text, patternSDT))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! Phải là 10 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return false;
                }
            }

            return true;
        }
       


        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát không?", "Xác nhận thoát!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            txtCFPass.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lbDangNhap_Click(object sender, EventArgs e)
        {

        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            txtMaNguoiDung.Visible = false;
            txtMaNguoiDung.Text = HamXuLy.MaTuDong("NGUOIDUNG");
            cboMaChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaChucVu.DropDownStyle = ComboBoxStyle.DropDownList;
            string sqlChiNhanh = "SELECT * FROM CHINHANH";
            string sqlChucVu = "SELECT * FROM CHUCVU";
            HamXuLy.FillCombo(sqlChiNhanh, cboMaChiNhanh, "TENCHINHANH", "MACHINHANH");
            HamXuLy.FillCombo(sqlChucVu, cboMaChucVu, "TENCHUCVU", "MACHUCVU");
        }

    }
}
