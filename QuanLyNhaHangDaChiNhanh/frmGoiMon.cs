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
        private PictureBox pictureBoxMonAn;

        private void InitializePictureBox()
        {
            pictureBoxMonAn = new PictureBox();
            pictureBoxMonAn.Size = new Size(120, 120);
            pictureBoxMonAn.Location = new Point(600, 100); // chỉnh tuỳ ý
            pictureBoxMonAn.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxMonAn.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pictureBoxMonAn);
        }

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
            InitializePictureBox();

            if (cbDanhMuc.SelectedValue != null && !(cbDanhMuc.SelectedValue is DataRowView))
            {
                LoadMonAnTheoDanhMuc(cbDanhMuc.SelectedValue.ToString());
            }

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
        private bool isLoadingDanhMuc = false;
        private bool isDanhMucLoaded = false;
        private void LoadDanhMucMonAn()
        {
            isLoadingDanhMuc = true;

            cbDanhMuc.SelectedIndexChanged -= cbDanhMuc_SelectedIndexChanged; // Gỡ sự kiện tạm thời

            string sql = "SELECT MADANHMUC, TENDANHMUC FROM DANHMUCMON";
            DataTable dt = HamXuLy.GetDataToTable(sql);

            cbDanhMuc.DataSource = dt;
            cbDanhMuc.DisplayMember = "TENDANHMUC";
            cbDanhMuc.ValueMember = "MADANHMUC";

            cbDanhMuc.SelectedIndexChanged += cbDanhMuc_SelectedIndexChanged; // Gắn lại sự kiện

            isLoadingDanhMuc = false;
            isDanhMucLoaded = true;

            if (cbDanhMuc.SelectedValue != null && !(cbDanhMuc.SelectedValue is DataRowView))
            {
                string maDanhMuc = cbDanhMuc.SelectedValue.ToString();
                LoadMonAnTheoDanhMuc(maDanhMuc);
            }
        }

        private void cbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isDanhMucLoaded || isLoadingDanhMuc || cbDanhMuc.SelectedValue == null || cbDanhMuc.SelectedValue is DataRowView)
                return;

            string maDanhMuc = cbDanhMuc.SelectedValue.ToString();
            LoadMonAnTheoDanhMuc(maDanhMuc);
        }



        private void LoadMonAnTheoDanhMuc(string maDanhMuc)
        {
            string sql = "SELECT DISTINCT MAMON, TENMON, HINHANH FROM MONAN WHERE MADANHMUC = @madm";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@madm", maDanhMuc);
            DataTable dt = HamXuLy.GetDataToTable(sql, param);

            flpMonAn.Controls.Clear();

            HashSet<string> addedMonAn = new HashSet<string>();

            foreach (DataRow row in dt.Rows)
            {
                string maMon = row["MAMON"].ToString();
                if (addedMonAn.Contains(maMon)) continue; // tránh trùng món

                addedMonAn.Add(maMon);

                string tenMon = row["TENMON"].ToString();
                string hinhAnh = row["HINHANH"].ToString();

                if (!string.IsNullOrEmpty(hinhAnh) && !Path.IsPathRooted(hinhAnh))
                {
                    hinhAnh = Path.Combine(@"D:\\LTQL\\QLBH\\Project\\QuanLyNhaHangDaChiNhanhVer2\\QuanLyNhaHangDaChiNhanh\\img", hinhAnh);
                }

                Button btn = new Button();
                btn.Size = new Size(100, 100);
                btn.TextAlign = ContentAlignment.BottomCenter;
                btn.ImageAlign = ContentAlignment.TopCenter;
                btn.Tag = maMon;
                btn.Text = tenMon;

                if (!string.IsNullOrEmpty(hinhAnh) && File.Exists(hinhAnh))
                {
                    try
                    {
                        Image original = Image.FromFile(hinhAnh);
                        Image resized = new Bitmap(original, new Size(48, 48));
                        btn.Image = resized;
                    }
                    catch { }
                }

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

            // Tăng số lượng nếu món đã có
            foreach (DataGridViewRow row in dgvChiTietHoaDon.Rows)
            {
                if (row.Cells["MAMON"].Value != null && row.Cells["MAMON"].Value.ToString() == maMon)
                {
                    int soLuong = Convert.ToInt32(row.Cells["SOLUONG"].Value);
                    row.Cells["SOLUONG"].Value = soLuong + 1;
                    return;
                }

            }

            string sql = "SELECT TENMON, GIA, HINHANH FROM MONAN WHERE MAMON = @mamon";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@mamon", maMon);
            DataTable dt = HamXuLy.GetDataToTable(sql, param);

            if (dt.Rows.Count > 0)
            {
                string tenMon = dt.Rows[0]["TENMON"].ToString();
                decimal donGia = Convert.ToDecimal(dt.Rows[0]["GIA"]);
                string hinhAnh = dt.Rows[0]["HINHANH"].ToString();

                if (!string.IsNullOrEmpty(hinhAnh))
                {
                    string fullPath = Path.Combine(@"D:\\LTQL\\QLBH\\Project\\QuanLyNhaHangDaChiNhanhVer2\\QuanLyNhaHangDaChiNhanh\\img", hinhAnh);
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            pictureBoxMonAn.Image = Image.FromFile(fullPath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể tải ảnh: " + ex.Message);
                        }
                    }
                    else
                    {
                        pictureBoxMonAn.Image = null; // clear nếu ảnh không tồn tại
                    }
                }
                else
                {
                    pictureBoxMonAn.Image = null; // clear nếu không có hình
                }

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

        private void btnThanhToan_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void dgvChiTietHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvChiTietHoaDon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa trong hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa món đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            foreach (DataGridViewRow selectedRow in dgvChiTietHoaDon.SelectedRows)
            {
                if (selectedRow.Cells["MAMON"].Value == null)
                    continue;

                string maMon = selectedRow.Cells["MAMON"].Value.ToString();

                // Xóa khỏi chiTietHoaDon trên giao diện
                foreach (DataRow row in chiTietHoaDon.Rows)
                {
                    if (row["MAMON"].ToString() == maMon)
                    {
                        chiTietHoaDon.Rows.Remove(row);
                        break;
                    }
                }

                // Xóa khỏi CSDL nếu đã lưu
                string sqlDelete = "DELETE FROM CHITIETHOADON WHERE MAHD = @mahd AND MAMON = @mamon";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@mahd", maHoaDon);
                param.Add("@mamon", maMon);

                HamXuLy.RunSqlWithParams(sqlDelete, param);
            }

            MessageBox.Show("Đã xóa món khỏi hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGiamGia_Click(object sender, EventArgs e)
        {
            if (chiTietHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa có món ăn nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tính tổng tiền hiện tại của hóa đơn
            decimal tongTien = 0;
            foreach (DataRow row in chiTietHoaDon.Rows)
            {
                decimal soLuong, gia;
                decimal.TryParse(row["SOLUONG"].ToString(), out soLuong);
                decimal.TryParse(row["GIA"].ToString(), out gia);
                tongTien += soLuong * gia;
            }

            // Truy vấn khuyến mãi hợp lệ
            string sql = @"
        SELECT TOP 1 * 
        FROM KHUYENMAI 
        WHERE TRANGTHAI = 1 
            AND @now BETWEEN NGAYBATDAU AND NGAYKETTHUC
        ORDER BY GIATRI DESC";

            Dictionary<string, object> param = new Dictionary<string, object>();
            param["@now"] = DateTime.Now;

            DataTable dt = HamXuLy.GetDataToTable(sql, param);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có khuyến mãi nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy thông tin khuyến mãi tốt nhất
            DataRow km = dt.Rows[0];
            string tenKM = km["TENKM"].ToString();
            int maKM = Convert.ToInt32(km["MAKM"]);
            decimal giaTri = 0;
            int maLoai = 0;

            decimal.TryParse(km["GIATRI"].ToString(), out giaTri);
            int.TryParse(km["MALOAI"].ToString(), out maLoai);

            decimal tongGiamGia = 0;

            // Áp dụng khuyến mãi
            if (maLoai == 1) // Giảm theo phần trăm
            {
                foreach (DataRow row in chiTietHoaDon.Rows)
                {
                    decimal soLuong, gia;
                    decimal.TryParse(row["SOLUONG"].ToString(), out soLuong);
                    decimal.TryParse(row["GIA"].ToString(), out gia);

                    decimal giam = soLuong * gia * giaTri / 100;
                    row["GIAMGIA"] = Math.Round(giam, 0);
                    tongGiamGia += Math.Round(giam, 0);
                }
            }
            else if (maLoai == 2) // Giảm số tiền cố định (chia đều)
            {
                decimal giamTheoMon = giaTri / chiTietHoaDon.Rows.Count;
                giamTheoMon = Math.Round(giamTheoMon, 0);

                foreach (DataRow row in chiTietHoaDon.Rows)
                {
                    row["GIAMGIA"] = giamTheoMon;
                    tongGiamGia += giamTheoMon;
                }
            }

            // Tính tổng tiền sau giảm
            decimal tongTienSauGiam = tongTien - tongGiamGia;

            // Cập nhật vào bảng HOADON
            string sqlUpdateHD = @"
        UPDATE HOADON 
        SET MAKM = @makm,
            GIATRIKHUYENMAI = @giamgia,
            TONGTIENHANG = @tongtien,
            TONGTIENPHAITRA = @phaitra
        WHERE MAHD = @mahd";

            Dictionary<string, object> paramUpdate = new Dictionary<string, object>();
            paramUpdate["@makm"] = maKM;
            paramUpdate["@giamgia"] = tongGiamGia;
            paramUpdate["@tongtien"] = tongTien;
            paramUpdate["@phaitra"] = tongTienSauGiam;
            paramUpdate["@mahd"] = maHoaDon;

            HamXuLy.RunSqlWithParams(sqlUpdateHD, paramUpdate);

            dgvChiTietHoaDon.Refresh(); // Cập nhật lại lưới

            // ==== ĐỊNH DẠNG CỘT GIÁ & THÀNH TIỀN ====
            dgvChiTietHoaDon.Columns["GIA"].DefaultCellStyle.Format = "N0";
            dgvChiTietHoaDon.Columns["GIA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvChiTietHoaDon.Columns["THANHTIEN"].DefaultCellStyle.Format = "N0";
            dgvChiTietHoaDon.Columns["THANHTIEN"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            // ==========================================

            MessageBox.Show(
                string.Format("Đã áp dụng khuyến mãi: {0}\nTổng giảm: {1:N0} VNĐ\nTổng phải trả: {2:N0} VNĐ",
                tenKM, tongGiamGia, tongTienSauGiam),
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
