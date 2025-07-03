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
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
            txtMatKhau.UseSystemPasswordChar = true;
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát không?", "Xác nhận thoát!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private Boolean kiemtrathongtin()
        {

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Bạn chưa nhập tên tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // Kiểm tra độ dài mật khẩu (ít nhất 6 ký tự)
            if (txtMatKhau.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }


            return true;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            bool kq = kiemtrathongtin();
            if (kq == true)
            {
                HamXuLy.Connect();
                string taikhoan = txtTenDangNhap.Text.Trim();
                string matkhau = txtMatKhau.Text.Trim();
                string sql1 = "SELECT * FROM NGUOIDUNG WHERE TENNGUOIDUNG = '" + taikhoan + "' AND MATKHAU = '" + matkhau + "'";
                string sql2 = "SELECT * FROM NGUOIDUNG WHERE TENNGUOIDUNG = '" + taikhoan + "' AND MATKHAU != '" + matkhau + "'";
                DataTable dtlg = new DataTable();
                frmDashBoard frm = new frmDashBoard();
                frm.SetAllButtonsEnabled(false);
                if (HamXuLy.TruyVan(sql1, dtlg))
                {

                    string nhomquyen = dtlg.Rows[0]["MACHUCVU"].ToString().Trim();
                    string iduser = dtlg.Rows[0]["MANGUOIDUNG"].ToString().Trim();
                    if (nhomquyen == "CV001")
                    {
                        frm.Show();
                        frm.SetAllButtonsEnabled(true);
                    }
                    else
                    {
                        string sql3 = "SELECT * FROM PHANQUYEN WHERE MANGUOIDUNG = '" + iduser + "'";
                        DataTable dtPQ = new DataTable();
                        if (HamXuLy.TruyVan(sql3, dtPQ))
                        {
                            frm.Show();
                            foreach (DataRow row in dtPQ.Rows)
                            {
                                string chucNang = row["MACHUCNANG"].ToString().Trim();

                                if (chucNang == "CN001") // Quản lý người dùng
                                {
                                    frm.NguoiDung.Enabled = true;
                                }
                                if (chucNang == "CN002") // Quản lý nhân viên
                                {
                                    frm.NhanVien.Enabled = true;
                                    frm.NhanVienNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN003") // Quản lý chức vụ
                                {
                                    frm.NhanVien.Enabled = true;  // Gộp chung vào nhân viên
                                }
                                if (chucNang == "CN004") // Quản lý chi nhánh
                                {
                                    frm.ChiNhanh.Enabled = true;
                                }
                                if (chucNang == "CN005" || chucNang == "CN006") // Quản lý khu vực bàn + Bàn ăn
                                {
                                    frm.BanAn.Enabled = true;
                                    frm.BanAnNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN007") // Quản lý khách hàng
                                {
                                    frm.KhachHang.Enabled = true;
                                    frm.KhachHangNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN008") // Đặt bàn
                                {
                                    frm.BanAnNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN009") // Đánh giá món ăn
                                {
                                    frm.KhachHangNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN010") // Quản lý thực đơn
                                {
                                    frm.ThucDon.Enabled = true;
                                    frm.ThucDonNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN011") // Quản lý danh mục món ăn
                                {
                                    frm.DanhMucMonAn.Enabled = true;
                                }
                                if (chucNang == "CN012") // Quản lý nguyên liệu
                                {
                                    frm.NguyenLieu.Enabled = true;
                                    frm.NguyenLieuNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN013") // Quản lý nhập kho
                                {
                                    frm.NguyenLieu.Enabled = true;
                                }
                                if (chucNang == "CN014") // Quản lý nhà cung cấp
                                {
                                    frm.NhaCungCap.Enabled = true;
                                }
                                if (chucNang == "CN015" || chucNang == "CN016" || chucNang == "CN017") // Hóa đơn / Thanh toán / Gộp-Tách-Đổi bàn
                                {
                                    frm.HoaDon.Enabled = true;
                                    frm.HoaDonNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN018") // Quản lý khuyến mãi
                                {
                                    frm.GiamGia.Enabled = true;
                                }
                                if (chucNang == "CN019") // Quản lý voucher
                                {
                                    frm.GiamGia.Enabled = true;
                                }
                                if (chucNang == "CN020") // Quản lý lương
                                {
                                    frm.Luong.Enabled = true;
                                }
                                if (chucNang == "CN021") // Quản lý ca làm
                                {
                                    frm.CaLam.Enabled = true;
                                }
                                if (chucNang == "CN022") // Chấm công
                                {
                                    frm.CaLam.Enabled = true;
                                }
                                if (chucNang == "CN023") // Báo cáo doanh thu
                                {
                                    frm.BaoCao.Enabled = true;
                                    frm.BaoCaoNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN024") // Báo cáo lương
                                {
                                    frm.BaoCao.Enabled = true;
                                }
                                if (chucNang == "CN025") // Tìm kiếm
                                {
                                    frm.TimKiem.Enabled = true;
                                    frm.TimKiemNhaHang.Enabled = true;
                                }
                                if (chucNang == "CN026") // Nhật ký hệ thống
                                {
                                    frm.NguoiDung.Enabled = true;  // Không có nút riêng thì gộp User
                                }
                                if (chucNang == "CN027") // Lịch sử đăng nhập
                                {
                                    frm.NguoiDung.Enabled = true;
                                }
                                if (chucNang == "CN028") // Quản lý định mức món ăn
                                {
                                    frm.ThucDon.Enabled = true;
                                }
                            }

                        }
                    }
                }
                else if (HamXuLy.TruyVan(sql2, dtlg))
                {
                    MessageBox.Show("Sai mật khẩu!");
                }
                else
                {
                    MessageBox.Show("Tài khoản không tồn tại!");
                }
            }
            else
            {

            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }
    }
}
