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
                    Tag = maBan + "|" + trangThai
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
            if (parts.Length < 2)
            {
                MessageBox.Show("Dữ liệu Tag không đúng định dạng!");
                return;
            }

            string selectedMaBan = parts[0];
            string trangThaiBan = parts[1];

            string tt = trangThaiBan.Trim().ToLower().Replace("\r", "").Replace("\n", "").Replace(" ", "");

            if (tt == "trống")
            {
                DialogResult result = MessageBox.Show("Bàn đang trống. Bạn có chắc chắn muốn vào và tạo hóa đơn mới không?",
                                                      "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;

                // Lấy MANHANVIEN từ NGUOIDUNG theo mã người dùng
                string sqlGetID = "SELECT MANHANVIEN FROM NGUOIDUNG WHERE MANGUOIDUNG = @manguoidung";
                Dictionary<string, object> paramGetID = new Dictionary<string, object>();
                paramGetID.Add("@manguoidung", Session.MaNhanVien); // ví dụ: "ND001"

                string idNhanVien = HamXuLy.GetFieldValue(sqlGetID, paramGetID);

                if (string.IsNullOrEmpty(idNhanVien))
                {
                    MessageBox.Show("Không tìm thấy mã nhân viên cho người dùng: " + Session.MaNhanVien);
                    return;
                }

                int idNV;
                if (!int.TryParse(idNhanVien, out idNV))
                {
                    MessageBox.Show("Mã nhân viên không hợp lệ! Chuỗi: " + idNhanVien);
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

                if (resultObj == null || resultObj == DBNull.Value)
                {
                    MessageBox.Show("Không thể tạo hóa đơn. Chuỗi trả về: null");
                    return;
                }

                int maHD;
                if (!int.TryParse(resultObj.ToString(), out maHD))
                {
                    MessageBox.Show("Không thể tạo hóa đơn. Chuỗi trả về: " + resultObj.ToString());
                    return;
                }

                // Cập nhật trạng thái bàn sang "Có khách"
                string sqlUpdateBan = "UPDATE BANAN SET TRANGTHAI = N'Có khách' WHERE MABAN = @maban";
                Dictionary<string, object> paramUpdateBan = new Dictionary<string, object>();
                paramUpdateBan.Add("@maban", selectedMaBan);
                HamXuLy.RunSqlWithParams(sqlUpdateBan, paramUpdateBan);

                frmGoiMon goiMonForm = new frmGoiMon(maHD);
                goiMonForm.ShowDialog();

                LoadBanTheoKhu();

            }
            else
            {
                // Bàn đã có khách
                string sqlCheck = "SELECT TOP 1 MAHD FROM HOADON WHERE MABAN = @maban AND TRANGTHAI = N'Chưa thanh toán'";
                Dictionary<string, object> paramCheck = new Dictionary<string, object>();
                paramCheck.Add("@maban", selectedMaBan);

                DataTable dtCheck = HamXuLy.GetDataToTable(sqlCheck, paramCheck);

                if (dtCheck.Rows.Count > 0)
                {
                    int maHD = Convert.ToInt32(dtCheck.Rows[0]["MAHD"]);
                    frmGoiMon goiMonForm = new frmGoiMon(maHD);
                    goiMonForm.ShowDialog();
                    LoadBanTheoKhu();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn chưa thanh toán cho bàn này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void frmBanOnTabNhaHang_Load(object sender, EventArgs e)
        {
            LoadBanTheoChiNhanh();
        }


    }
}
