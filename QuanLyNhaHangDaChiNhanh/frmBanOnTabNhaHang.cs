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
    public partial class frmBanOnTabNhaHang : Form
    {
        //private int maHoaDon;

        private string selectedMaBan = null;
        private string selectedTenBan = null;
        private string selectedTrangThai = null;
        public frmBanOnTabNhaHang()
        {
            InitializeComponent();
        }
        private void LoadBanTheoChiNhanh()
        {
            HamXuLy.Connect();

            if (string.IsNullOrEmpty(Session.MaChiNhanh))
            {
                MessageBox.Show("Không xác định được mã chi nhánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sqlKhu = "SELECT MAKHU, TENKHU FROM KHU WHERE MACHINHANH = '" + Session.MaChiNhanh + "'";
            HamXuLy.FillCombo(sqlKhu, cbKhu, "TENKHU", "MAKHU");

            // Gỡ bỏ rồi gán lại sự kiện để tránh gán nhiều lần
            cbKhu.SelectedIndexChanged -= cbKhu_SelectedIndexChanged;
            cbKhu.SelectedIndexChanged += cbKhu_SelectedIndexChanged;

            if (cbKhu.Items.Count > 0)
            {
                cbKhu.SelectedIndex = 0; // Chọn mục đầu tiên
                LoadBanTheoKhu();
            }
            else
            {
                MessageBox.Show("Chi nhánh chưa có khu nào.");
            }
        }

        

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void cbKhu_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBanTheoKhu();
        }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        //===================================
        private void LoadBanTheoKhu()
        {
            if (cbKhu.SelectedValue == null || string.IsNullOrEmpty(Session.MaChiNhanh))
                return;

            string maKhu = cbKhu.SelectedValue.ToString();
            string maChiNhanh = Session.MaChiNhanh;

            string sql = "SELECT MABAN, TENBAN, TRANGTHAI FROM BANAN WHERE MAKHU = '" + maKhu + "' AND MACHINHANH = '" + maChiNhanh + "'";
            DataTable dt = HamXuLy.GetDataToTable(sql);

            flpBan.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string maBan = row["MABAN"].ToString();
                string tenBan = row["TENBAN"].ToString();
                string trangThai = row["TRANGTHAI"].ToString().Trim();

                Button btn = new Button
                {
                    Name = "btn" + maBan,
                    Size = new Size(110, 110),
                    TextAlign = ContentAlignment.BottomCenter,
                    ImageAlign = ContentAlignment.TopCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Text = tenBan + "\n(" + trangThai + ")",
                    Tag = maBan + "|" + trangThai + "|" + tenBan 
                };


                try
                {
                    btn.Image = Image.FromFile(@"D:\LTQL\QLBH\Project\QuanLyNhaHangDaChiNhanhVer2\QuanLyNhaHangDaChiNhanh\img\icons8-dining-table-48.png");
                }
                catch
                {
                    // Bỏ MessageBox để tránh gián đoạn, có thể log nếu cần
                }

                // Gán màu theo trạng thái bàn
                string tt = trangThai.ToLower();
                if (tt == "trống")
                    btn.BackColor = Color.LightGreen;
                else if (tt == "có khách")
                    btn.BackColor = Color.LightSalmon;
                else if (tt == "đã in biểu kiểm món")
                    btn.BackColor = Color.Gold;
                else if (tt == "đã in hóa đơn")
                    btn.BackColor = Color.MediumPurple;
                else
                    btn.BackColor = Color.LightGray;

                btn.Click += Ban_Click;

                flpBan.Controls.Add(btn);
            }
        }

        private void Ban_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string[] parts = btn.Tag.ToString().Split('|');
            if (parts.Length < 3)
            {
                MessageBox.Show("Dữ liệu Tag không đúng định dạng!");
                return;
            }

            selectedMaBan = parts[0];
            selectedTrangThai = parts[1].Trim().ToLower();
            selectedTenBan = parts[2];

            // (Tuỳ bạn) Có thể tô đậm nút đã chọn hoặc đổi màu
            // hoặc hiển thị ra label: lblBanDangChon.Text = selectedTenBan;
        }

        private void frmBanOnTabNhaHang_Load(object sender, EventArgs e)
        {
            LoadBanTheoChiNhanh();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            /*if (string.IsNullOrEmpty(selectedMaBan))
            {
                MessageBox.Show("Vui lòng chọn bàn trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(lblTenKhachHang.Text))
            {
                DialogResult confirm = MessageBox.Show("Hóa đơn đã có khách hàng.\nBạn có muốn thay đổi không?",
                                                       "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.No)
                    return;
            }

            frmChonKhachHang formChon = new frmChonKhachHang();

            if (formChon.ShowDialog() == DialogResult.OK)
            {
                string tenKH = formChon.TenKhachHangDuocChon;

                if (string.IsNullOrEmpty(tenKH))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy lại MAHD từ DB đúng với trạng thái "Chưa thanh toán"
                string sqlGetHD = "SELECT TOP 1 MAHD FROM HOADON WHERE MABAN = @maban AND TRANGTHAI = N'Chưa thanh toán'";
                Dictionary<string, object> paramGetHD = new Dictionary<string, object>();
                paramGetHD.Add("@maban", selectedMaBan);

                object objMaHD = HamXuLy.GetFieldValue(sqlGetHD, paramGetHD);
                int maHD;
                if (objMaHD == null || !int.TryParse(objMaHD.ToString(), out maHD))
                {
                    MessageBox.Show("Không tìm thấy hóa đơn chưa thanh toán!", "Thông báo");
                    return;
                }

                // Chỉ cập nhật TENKHACHHANG
                string sqlUpdate = "UPDATE HOADON SET TENKHACHHANG = @tenkh WHERE MAHD = @mahd";
                Dictionary<string, object> param = new Dictionary<string, object>
        {
            { "@tenkh", tenKH },
            { "@mahd", maHD }
        };

                HamXuLy.RunSqlWithParams(sqlUpdate, param);
                lblTenKhachHang.Text = tenKH;

                MessageBox.Show("Đã thêm tên khách hàng vào hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }




        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedMaBan))
            {
                MessageBox.Show("Vui lòng chọn bàn để in hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tìm hóa đơn chưa thanh toán
            string sql = "SELECT TOP 1 MAHD FROM HOADON WHERE MABAN = @maban AND TRANGTHAI = N'Chưa thanh toán'";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@maban", selectedMaBan);

            object result = HamXuLy.GetFieldValue(sql, param);

            int maHD;
            if (result == null || !int.TryParse(result.ToString(), out maHD))
            {
                MessageBox.Show("Mã bàn: " + selectedMaBan + "\nTrạng thái: " + selectedTrangThai + "\nTên bàn: " + selectedTenBan);
                return;
            }

            // Gọi form in hóa đơn
            frmInHoaDon frm = new frmInHoaDon(maHD);
            frm.ShowDialog();
        }

        private void btnVao_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedMaBan))
            {
                MessageBox.Show("Vui lòng chọn bàn trước!", "Thông báo");
                return;
            }

            int maHD = -1;

            if (selectedTrangThai.ToLower() == "trống")
            {
                DialogResult result = MessageBox.Show("Bàn đang trống. Tạo hóa đơn mới?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.No) return;

                // Lấy mã nhân viên hiện tại
                string sqlGetID = "SELECT MANHANVIEN FROM NGUOIDUNG WHERE MANGUOIDUNG = @manguoidung";
                Dictionary<string, object> paramGetID = new Dictionary<string, object>();
                paramGetID.Add("@manguoidung", Session.MaNhanVien);

                string idNhanVien = HamXuLy.GetFieldValue(sqlGetID, paramGetID);
                int idNV = -1;

                if (string.IsNullOrEmpty(idNhanVien) || !int.TryParse(idNhanVien, out idNV))
                {
                    MessageBox.Show("Không thể lấy mã nhân viên!");
                    return;
                }

                // Tạo hóa đơn mới
                string sqlInsert = @"
            INSERT INTO HOADON (MACHINHANH, NGAYLAP, MANV, MABAN, TRANGTHAI)
            VALUES (@machinhanh, GETDATE(), @manv, @maban, N'Chưa thanh toán');
            SELECT SCOPE_IDENTITY();";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@machinhanh", Session.MaChiNhanh);
                parameters.Add("@manv", idNV);
                parameters.Add("@maban", selectedMaBan);

                object resultObj = HamXuLy.ExecuteScalar(sqlInsert, parameters);

                if (resultObj == null || !int.TryParse(resultObj.ToString(), out maHD))
                {
                    MessageBox.Show("Không thể tạo hóa đơn.");
                    return;
                }

                // Cập nhật trạng thái bàn
                string sqlUpdate = "UPDATE BANAN SET TRANGTHAI = N'Có khách' WHERE MABAN = @maban";
                Dictionary<string, object> paramUpdate = new Dictionary<string, object>();
                paramUpdate.Add("@maban", selectedMaBan);
                HamXuLy.RunSqlWithParams(sqlUpdate, paramUpdate);
            }
            else
            {
                // Nếu bàn không trống → kiểm tra hóa đơn đang nợ
                string sqlCheck = "SELECT TOP 1 MAHD FROM HOADON WHERE MABAN = @maban AND TRANGTHAI = N'Chưa thanh toán'";
                Dictionary<string, object> paramCheck = new Dictionary<string, object>();
                paramCheck.Add("@maban", selectedMaBan);

                object resultCheck = HamXuLy.GetFieldValue(sqlCheck, paramCheck);
                if (resultCheck != null && int.TryParse(resultCheck.ToString(), out maHD))
                {
                    // OK, tìm được hóa đơn
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn chưa thanh toán.");
                    return;
                }
            }

            // Chuyển sang form gọi món
            this.Hide();
            frmGoiMon goiMon = new frmGoiMon(maHD, selectedTenBan);
            goiMon.ShowDialog();
            this.Show();
            LoadBanTheoKhu();
        }



        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedMaBan))
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Thông báo");
                return;
            }

            string sql = "UPDATE BANAN SET TRANGTHAI = N'Đã in hóa đơn' WHERE MABAN = @maban";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@maban", selectedMaBan);

            HamXuLy.RunSqlWithParams(sql, param);
            MessageBox.Show("Bàn đã được đánh dấu là đã in hóa đơn!", "Thông báo");
            LoadBanTheoKhu();
        }

        private void btnKetThucBan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedMaBan))
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Thông báo");
                return;
            }

            // Cập nhật trạng thái bàn về 'Trống'
            string sql = "UPDATE BANAN SET TRANGTHAI = N'Trống' WHERE MABAN = @maban";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@maban", selectedMaBan);

            HamXuLy.RunSqlWithParams(sql, param);
            MessageBox.Show("Bàn đã kết thúc và trở về trạng thái trống.", "Thông báo");
            LoadBanTheoKhu();
        }

        private void btnThe_Click(object sender, EventArgs e)
        {
            CapNhatHinhThucThanhToan("Thẻ");
        }

        private void btnChuyenKhoan_Click(object sender, EventArgs e)
        {
            CapNhatHinhThucThanhToan("Chuyển khoản");
        }

        private void btnTienMat_Click(object sender, EventArgs e)
        {
            CapNhatHinhThucThanhToan("Tiền mặt");
        }
        private void CapNhatHinhThucThanhToan(string hinhThuc)
        {
            if (string.IsNullOrEmpty(selectedMaBan))
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Thông báo");
                return;
            }

            // Lấy mã hóa đơn chưa thanh toán của bàn
            string sqlGetHD = "SELECT TOP 1 MAHD FROM HOADON WHERE MABAN = @maban AND TRANGTHAI = N'Nợ'";
            Dictionary<string, object> paramGetHD = new Dictionary<string, object>();
            paramGetHD.Add("@maban", selectedMaBan);

            object objMaHD = HamXuLy.GetFieldValue(sqlGetHD, paramGetHD);
            int maHD;
            if (objMaHD == null || !int.TryParse(objMaHD.ToString(), out maHD))
            {
                MessageBox.Show("Không tìm thấy hóa đơn chưa thanh toán!", "Thông báo");
                return;
            }

            // Cập nhật hình thức thanh toán + trạng thái hóa đơn
            string sql = @"
        UPDATE HOADON 
        SET HINHTHUCTHANHTOAN = @httt, TRANGTHAI = N'Đã thanh toán'
        WHERE MAHD = @mahd";

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@httt", hinhThuc);
            param.Add("@mahd", maHD);

            HamXuLy.RunSqlWithParams(sql, param);

            MessageBox.Show("Đã cập nhật hình thức thanh toán: " + hinhThuc + "\nvà đánh dấu hóa đơn là đã thanh toán.", "Thông báo");
        }


    }
}
