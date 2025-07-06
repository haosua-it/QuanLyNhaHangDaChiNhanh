using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QuanLyNhaHangDaChiNhanh
{
    public partial class frmGoiMon: Form
    {
        private string tenBan = "";
        private int maHoaDon;
        public frmGoiMon(int maHoaDon, string tenBan)
        {
            InitializeComponent();
            this.maHoaDon = maHoaDon;
            this.tenBan = tenBan;

            // Có thể gọi hàm hiển thị dữ liệu theo bàn ở đây
            LoadThongTinBan();
        }

        private void frmGoiMon_Load(object sender, EventArgs e)
        {
            lblThoiGian.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            timer = new Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;
            timer.Start();

            lblTenBan.Text = tenBan;
            InitChiTietHoaDon();
            LoadDanhMucMonAn();
            LoadChiTietHoaDonTuDatabase();
        }
        private Timer timer;

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblThoiGian.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        private void lblThoiGian_Click(object sender, EventArgs e)
        {

        }
        private void LoadThongTinBan()
        {
            lblTenBan.Text =  tenBan;
        }
        private void LoadDanhMucMonAn()
        {
            string sql = "SELECT MADANHMUC, TENDANHMUC FROM DANHMUCMON";
            DataTable dt = HamXuLy.GetDataToTable(sql);

            cbDanhMuc.DataSource = dt;
            cbDanhMuc.DisplayMember = "TENDANHMUC";
            cbDanhMuc.ValueMember = "MADANHMUC";
        }


        private void cbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDanhMuc.SelectedValue != null)
            {
                string maLoai = cbDanhMuc.SelectedValue.ToString();
                LoadMonAnTheoDanhMuc(maLoai); // đúng tên hàm
            }
        }


        private void LoadMonAnTheoDanhMuc(string maLoai)
        {
            string sql = "SELECT MAMON, TENMON, HINHANH FROM MONAN WHERE MADANHMUC = @maloai";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@maloai", maLoai);
            DataTable dt = HamXuLy.GetDataToTable(sql, param);

            flpMonAn.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string tenMon = row["TENMON"].ToString();
                string hinhAnhPath = row["HINHANH"].ToString();
                string maMon = row["MAMON"].ToString();

                Button btn = new Button();
                btn.Text = tenMon;
                btn.Tag = maMon;
                btn.Width = 100;
                btn.Height = 100;
                btn.TextAlign = ContentAlignment.BottomCenter;
                btn.Font = new Font("Segoe UI", 9);
                btn.ImageAlign = ContentAlignment.TopCenter;

                try
                {
                    if (!string.IsNullOrEmpty(hinhAnhPath) && File.Exists(hinhAnhPath))
                        btn.Image = Image.FromFile(hinhAnhPath);
                }
                catch { }

                btn.Click += BtnMonAn_Click;

                flpMonAn.Controls.Add(btn);
            }
        }

        private DataTable chiTietHoaDon = new DataTable();

        private void InitChiTietHoaDon()
        {
            chiTietHoaDon = new DataTable(); // Đảm bảo khởi tạo mới mỗi lần

            chiTietHoaDon = new DataTable();
            chiTietHoaDon.Columns.Add("MAMON");
            chiTietHoaDon.Columns.Add("TENMON");
            chiTietHoaDon.Columns.Add("SOLUONG", typeof(int));
            chiTietHoaDon.Columns.Add("GIA", typeof(decimal));
            chiTietHoaDon.Columns.Add("GIAMGIA", typeof(decimal));
            chiTietHoaDon.Columns.Add("THANHTIEN", typeof(decimal), "(SOLUONG * GIA) - GIAMGIA");

            dgvChiTietHoaDon.DataSource = chiTietHoaDon;
        }


        private void BtnMonAn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string maMon = btn.Tag.ToString();

            foreach (DataGridViewRow row in dgvChiTietHoaDon.Rows)
            {
                if (row.Cells["MAMON"] != null && row.Cells["MAMON"].Value != null && row.Cells["MAMON"].Value.ToString() == maMon)
                {
                    if (row.Cells["SOLUONG"] != null)
                    {
                        int soLuong = Convert.ToInt32(row.Cells["SOLUONG"].Value);
                        row.Cells["SOLUONG"].Value = soLuong + 1;
                    }
                    return;
                }
            }

            string sql = "SELECT TENMON, GIA FROM MONAN WHERE MAMON = @mamon";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@mamon", maMon);
            DataTable dt = HamXuLy.GetDataToTable(sql, param);

            if (dt.Rows.Count > 0)
            {
                string tenMon = dt.Rows[0]["TENMON"].ToString();
                decimal donGia = Convert.ToDecimal(dt.Rows[0]["GIA"]);

                chiTietHoaDon.Rows.Add(maMon, tenMon, 1, donGia, 0, donGia);
            }
        }

        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
            if (chiTietHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có món ăn nào được chọn.");
                return;
            }

            foreach (DataRow row in chiTietHoaDon.Rows)
            {
                string sqlInsert = @"INSERT INTO CHITIETHOADON (MAHD, MACHINHANH, MAMON, SOLUONG, DONGIA)
                         VALUES (@mahd, @machinhanh, @mamon, @soluong, @dongia)";


                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@mahd", maHoaDon);
                param.Add("@machinhanh", Session.MaChiNhanh); // bạn cần đảm bảo `Session.MaChiNhanh` tồn tại
                param.Add("@mamon", row["MAMON"]);
                param.Add("@soluong", row["SOLUONG"]);
                param.Add("@dongia", row["GIA"]);
                //param.Add("@thanhtien", row["THANHTIEN"]);

                HamXuLy.RunSqlWithParams(sqlInsert, param); // phương thức bạn đã dùng
            }

            MessageBox.Show("Lưu hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (chiTietHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Không có món ăn nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataRow row in chiTietHoaDon.Rows)
            {
                string maMon = row["MAMON"].ToString();

                // Kiểm tra món này đã có trong CHITIETHOADON chưa để tránh thêm trùng
                string sqlCheck = "SELECT COUNT(*) FROM CHITIETHOADON WHERE MAHD = @mahd AND MAMON = @mamon";
                Dictionary<string, object> paramCheck = new Dictionary<string, object>();
                paramCheck.Add("@mahd", maHoaDon);
                paramCheck.Add("@mamon", maMon);

                int count = Convert.ToInt32(HamXuLy.ExecuteScalar(sqlCheck, paramCheck));
                if (count > 0)
                {
                    // Nếu đã tồn tại, có thể cập nhật số lượng mới (tuỳ yêu cầu):
                    string sqlUpdate = @"UPDATE CHITIETHOADON
                     SET SOLUONG = SOLUONG + @soluong,
                         DONGIA = @dongia
                     WHERE MAHD = @mahd AND MAMON = @mamon";


                    Dictionary<string, object> paramUpdate = new Dictionary<string, object>();
                    paramUpdate.Add("@mahd", maHoaDon);
                    paramUpdate.Add("@mamon", maMon);
                    paramUpdate.Add("@soluong", row["SOLUONG"]);
                    paramUpdate.Add("@dongia", row["GIA"]);

                    HamXuLy.RunSqlWithParams(sqlUpdate, paramUpdate);
                }
                else
                {
                    // Nếu chưa có thì insert mới
                    string sqlInsert = @"INSERT INTO CHITIETHOADON (MACHINHANH, MAHD, MAMON, SOLUONG, DONGIA)
                             VALUES (@machinhanh, @mahd, @mamon, @soluong, @dongia)";


                    Dictionary<string, object> paramInsert = new Dictionary<string, object>();
                    paramInsert.Add("@machinhanh", Session.MaChiNhanh);
                    paramInsert.Add("@mahd", maHoaDon);
                    paramInsert.Add("@mamon", row["MAMON"]);
                    paramInsert.Add("@soluong", row["SOLUONG"]);
                    paramInsert.Add("@dongia", row["GIA"]);
                    //paramInsert.Add("@thanhtien", row["THANHTIEN"]);

                    HamXuLy.RunSqlWithParams(sqlInsert, paramInsert);
                }
            }

            MessageBox.Show("Đã lưu tạm danh sách món ăn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void LoadChiTietHoaDonTuDatabase()
        {
            string sql = @"
        SELECT 
            ct.MAMON, 
            m.TENMON, 
            ct.SOLUONG, 
            ct.DONGIA AS GIA,
            (ct.SOLUONG * ct.DONGIA) AS THANHTIEN
        FROM CHITIETHOADON ct
        JOIN MONAN m ON ct.MAMON = m.MAMON
        WHERE ct.MAHD = @mahd";

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@mahd", maHoaDon);

            DataTable dt = HamXuLy.GetDataToTable(sql, param);

            chiTietHoaDon.Rows.Clear(); // nên clear trước để tránh trùng

            foreach (DataRow row in dt.Rows)
            {
                chiTietHoaDon.Rows.Add(
                    row["MAMON"],
                    row["TENMON"],
                    row["SOLUONG"],
                    row["GIA"],
                    0, // GIAMGIA không có trong CSDL, tạm thời set 0
                    row["THANHTIEN"]
                );
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            /*frmChonKhachHang formChon = new frmChonKhachHang();
            if (formChon.ShowDialog() == DialogResult.OK)
            {
                string maKH = formChon.MaKhachHangDuocChon;

                string sqlUpdate = "UPDATE HOADON SET MAKH = @makh WHERE MAHD = @mahd";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@makh", maKH);
                param.Add("@mahd", maHoaDon);

                HamXuLy.RunSqlWithParams(sqlUpdate, param);

                MessageBox.Show("Đã thêm khách hàng vào hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void btnGiamGia_Click(object sender, EventArgs e)
        {

        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }


    }
}
