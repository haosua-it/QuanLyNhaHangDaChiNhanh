using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyNhaHangDaChiNhanh
{
    public partial class frmLuongPart : Form
    {

        public string maLoaiNhanVien;

        public frmLuongPart(string maloai)
        {
            InitializeComponent();
            maLoaiNhanVien = maloai;
        }
        private int currentPage = 1;
        private int pageSize = 10;
        private void frmUsers_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();

            txtSoGioCong.Enabled = false;
            txtMaLuong.Enabled = false;
            txtTongLuong.Enabled = false; // Tổng lương là chỉ đọc
            cboMaNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;

            string sqlNhanVien = "SELECT * FROM NHANVIEN";
            HamXuLy.FillCombo(sqlNhanVien, cboMaNhanVien, "HOTEN", "MANHANVIEN");

            LoadLuongPhanTrang();
            txtLuongTheoGio.TextChanged += new EventHandler(CapNhatTongLuong);
            txtThuong.TextChanged += new EventHandler(CapNhatTongLuong);
            txtKhauTru.TextChanged += new EventHandler(CapNhatTongLuong);
            cboMaNhanVien.SelectedIndexChanged += (s, ev) => TinhSoGioCong();
            txtThangLam.TextChanged += (s, ev) => TinhSoGioCong();
            txtNamLam.TextChanged += (s, ev) => TinhSoGioCong();

        }
        private void TinhSoGioCong()
        {
            if (string.IsNullOrWhiteSpace(txtThangLam.Text) || string.IsNullOrWhiteSpace(txtNamLam.Text)) return;
            if (cboMaNhanVien.SelectedIndex == -1) return;

            int manv = Convert.ToInt32(cboMaNhanVien.SelectedValue);
            int thang = int.Parse(txtThangLam.Text);
            int nam = int.Parse(txtNamLam.Text);

            string sql = string.Format(@"
        SELECT ISNULL(SUM(DATEDIFF(MINUTE, GIOVAO, GIORA)), 0) / 60
        FROM CHAMCONG
        WHERE MANHANVIEN = {0} AND MONTH(NGAY) = {1} AND YEAR(NGAY) = {2}
    ", manv, thang, nam);

            string gioCong = HamXuLy.GetFieldValue(sql);
            txtSoGioCong.Text = gioCong;
        }


        private void reset()
        {
            pnlLuongPart.Enabled = false;
            txtMaLuong.Enabled = false;
            txtTongLuong.Enabled = false;
            txtMaLuong.Text = "";
            txtThangLam.Text = "";
            txtNamLam.Text = "";
            txtSoGioCong.Text = "";
            txtLuongTheoGio.Text = "";
            txtThuong.Text = "";
            txtKhauTru.Text = "";
            txtTongLuong.Text = "";
            btnAdd.Enabled = true;
            btnBack.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnPrint.Enabled = true;
            btnRedo.Enabled = true;
            btnSave.Enabled = true;
            btnSearch.Enabled = true;
            btnUndo.Enabled = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CapNhatTongLuong(object sender, EventArgs e)
        {
            decimal luongTheoGio = 0;
            int soGioCong = 0;
            decimal thuong = 0;
            decimal khauTru = 0;

            // Nếu để trống thì gán "0" vào cho an toàn
            if (string.IsNullOrWhiteSpace(txtLuongTheoGio.Text)) txtLuongTheoGio.Text = "0";
            if (string.IsNullOrWhiteSpace(txtSoGioCong.Text)) txtSoGioCong.Text = "0";
            if (string.IsNullOrWhiteSpace(txtThuong.Text)) txtThuong.Text = "0";
            if (string.IsNullOrWhiteSpace(txtKhauTru.Text)) txtKhauTru.Text = "0";

            // Thử parse từng giá trị
            bool isValid =
                decimal.TryParse(txtLuongTheoGio.Text, out luongTheoGio) &&
                int.TryParse(txtSoGioCong.Text, out soGioCong) &&
                decimal.TryParse(txtThuong.Text, out thuong) &&
                decimal.TryParse(txtKhauTru.Text, out khauTru);

            if (isValid)
            {

                decimal tongLuong = soGioCong * luongTheoGio + thuong - khauTru;
                txtTongLuong.Text = tongLuong.ToString("N0");
            }
            else
            {
                txtTongLuong.Text = "Lỗi nhập liệu";
            }
        }

        private void LoadLuongPhanTrang()
        {
            DataTable dt = ShowLuongPhanTrang(currentPage, pageSize);
            luoiLuongPart.DataSource = dt;

            luoiLuongPart.Columns["MALUONG"].HeaderText = "Mã lương";
            luoiLuongPart.Columns["MANHANVIEN"].HeaderText = "Mã nhân viên";
            luoiLuongPart.Columns["THANG"].HeaderText = "Tháng";
            luoiLuongPart.Columns["NAM"].HeaderText = "Năm";
            luoiLuongPart.Columns["SOGIOCONG"].HeaderText = "Số giờ công";
            luoiLuongPart.Columns["LUONGTHEOGIO"].HeaderText = "Lương theo giờ";
            luoiLuongPart.Columns["THUONG"].HeaderText = "Thưởng";
            luoiLuongPart.Columns["KHAUTRU"].HeaderText = "Khấu trừ";
            luoiLuongPart.Columns["TONGLUONG"].HeaderText = "Tổng lương";

            luoiLuongPart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiLuongPart.ReadOnly = true;
            luoiLuongPart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lblPage.Text = string.Format("Trang {0}", currentPage);
        }
        public DataTable ShowLuongPhanTrang(int pageNumber, int pageSize)
        {
            HamXuLy.Connect();
            SqlConnection conn = HamXuLy.conn;
            DataTable dt = new DataTable();
            int offset = (pageNumber - 1) * pageSize;

            string sql = string.Format(@"
    SELECT lf.* FROM LUONG_PARTTIME lf
    JOIN NHANVIEN nv ON nv.MANHANVIEN = lf.MANHANVIEN
    WHERE nv.MALOAI = '{0}'
    ORDER BY MALUONG OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", maLoaiNhanVien, offset, pageSize);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phân trang: " + ex.Message);
            }

            return dt;
        }

        private void btnPrevPage_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadLuongPhanTrang();
            }
        }

        private void btnNextPage_Click_1(object sender, EventArgs e)
        {
            currentPage++;
            LoadLuongPhanTrang();
        }

        private void luoiLuongPart_Click(object sender, EventArgs e)
        {
            if (luoiLuongPart.CurrentRow == null) return;

            DataGridViewRow row = luoiLuongPart.CurrentRow;

            txtMaLuong.Text = row.Cells["MALUONG"].Value.ToString();
            txtThangLam.Text = row.Cells["THANG"].Value.ToString();
            txtNamLam.Text = row.Cells["NAM"].Value.ToString();
            txtSoGioCong.Text = row.Cells["SOGIOCONG"].Value.ToString();
            txtLuongTheoGio.Text = row.Cells["LUONGTHEOGIO"].Value.ToString();
            txtThuong.Text = row.Cells["THUONG"].Value.ToString();
            txtKhauTru.Text = row.Cells["KHAUTRU"].Value.ToString();
            txtTongLuong.Text = row.Cells["TONGLUONG"].Value.ToString();

            cboMaNhanVien.SelectedValue = row.Cells["MANHANVIEN"].Value.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            reset();
            pnlLuongPart.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            txtThangLam.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            reset();
            pnlLuongPart.Enabled = true;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            txtThangLam.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            if (txtMaLuong.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mã lương", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                HamXuLy.Connect();
                string sqlDelete = "DELETE FROM LUONG_PARTTIME WHERE MALUONG = " + txtMaLuong.Text;
                if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi lương này không?", "Xác nhận xóa!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        HamXuLy.RunSQL(sqlDelete);
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo");
                    }
                }
                LoadLuongPhanTrang();
                reset();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnEdit.Enabled == true) // sửa
            {
                if (txtMaLuong.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn mã lương", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    HamXuLy.Connect();
                    string sqlUpdate = "UPDATE LUONG_PARTTIME SET " +
                                       "MANHANVIEN = " + cboMaNhanVien.SelectedValue.ToString() + ", " +
                                       "THANG = " + txtThangLam.Text + ", " +
                                       "NAM = " + txtNamLam.Text + ", " +
                                       "SOGIOCONG = " + txtSoGioCong.Text + ", " +
                                       "LUONGTHEOGIO = " + txtLuongTheoGio.Text + ", " +
                                       "THUONG = " + txtThuong.Text + ", " +
                                       "KHAUTRU = " + txtKhauTru.Text + " " +
                                       "WHERE MALUONG = " + txtMaLuong.Text;

                    try
                    {
                        HamXuLy.RunSQL(sqlUpdate);
                        MessageBox.Show("Chỉnh sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo");
                    }

                    LoadLuongPhanTrang();
                    reset();
                }
            }
            else // thêm
            {
                if (txtThangLam.Text == "" || txtNamLam.Text == "" || txtSoGioCong.Text == "" || txtLuongTheoGio.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                HamXuLy.Connect();

                string sqlInsert = "INSERT INTO LUONG_PARTTIME(" +
                                   "MANHANVIEN, THANG, NAM, SOGIOCONG, LUONGTHEOGIO, THUONG, KHAUTRU" +
                                   ") VALUES (" +
                                   cboMaNhanVien.SelectedValue.ToString() + ", " +
                                   txtThangLam.Text + ", " +
                                   txtNamLam.Text + ", " +
                                   txtSoGioCong.Text + ", " +
                                   txtLuongTheoGio.Text + ", " +
                                   txtThuong.Text + ", " +
                                   txtKhauTru.Text + ")";

                try
                {
                    HamXuLy.RunSQL(sqlInsert);
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo");
                }

                LoadLuongPhanTrang();
                reset();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadLuongPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = "SELECT * FROM LUONG_PARTTIME WHERE MANHANVIEN IN (SELECT MANHANVIEN FROM NHANVIEN WHERE HOTEN LIKE N'%" + keyword + "%')";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoiLuongPart.DataSource = dt;
        }

        private Stack<object[]> undoStack = new Stack<object[]>();
        private Stack<object[]> redoStack = new Stack<object[]>();

        private void SaveCurrentState()
        {
            object[] state = new object[]
            {
                txtMaLuong.Text,
                txtThangLam.Text,
                txtNamLam.Text,
                txtSoGioCong.Text,
                txtLuongTheoGio.Text,
                txtThuong.Text,
                txtKhauTru.Text,
                txtTongLuong.Text,
                cboMaNhanVien.SelectedValue
            };
            undoStack.Push(state);
            redoStack.Clear(); // xóa redo khi có thay đổi mới
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                object[] currentState = new object[]
                {
                    txtMaLuong.Text,
                    txtThangLam.Text,
                    txtNamLam.Text,
                    txtSoGioCong.Text,
                    txtLuongTheoGio.Text,
                    txtThuong.Text,
                    txtKhauTru.Text,
                    txtTongLuong.Text,
                    cboMaNhanVien.SelectedValue
                };
                redoStack.Push(currentState);

                object[] previousState = undoStack.Pop();
                txtMaLuong.Text = previousState[0].ToString();
                txtThangLam.Text = previousState[1].ToString();
                txtNamLam.Text = previousState[2].ToString();
                txtSoGioCong.Text = previousState[3].ToString();
                txtLuongTheoGio.Text = previousState[4].ToString();
                txtThuong.Text = previousState[5].ToString();
                txtKhauTru.Text = previousState[6].ToString();
                txtTongLuong.Text = previousState[7].ToString();
                cboMaNhanVien.SelectedValue = previousState[8];
            }
            else
            {
                MessageBox.Show("Không có thao tác nào để hoàn tác.");
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                SaveCurrentState(); // lưu lại trước khi redo
                object[] redoState = redoStack.Pop();
                txtMaLuong.Text = redoState[0].ToString();
                txtThangLam.Text = redoState[1].ToString();
                txtNamLam.Text = redoState[2].ToString();
                txtSoGioCong.Text = redoState[3].ToString();
                txtLuongTheoGio.Text = redoState[4].ToString();
                txtThuong.Text = redoState[5].ToString();
                txtKhauTru.Text = redoState[6].ToString();
                txtTongLuong.Text = redoState[7].ToString();
                cboMaNhanVien.SelectedValue = redoState[8];
            }
            else
            {
                MessageBox.Show

("Không có thao tác nào để làm lại.");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Chưa triển khai chức năng in
        }

        private void txtSoGioCong_TextChanged(object sender, EventArgs e)
        {
        }
    }
}