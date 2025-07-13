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
    public partial class frmXepCa : Form
    {
        public frmXepCa()
        {
            InitializeComponent();
        }
        private int currentPage = 1;
        private int pageSize = 10;
        private void frmUsers_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            pnlXepCa.Enabled = false;
            txtMaChamCong.Visible = false;
            cboCaLam.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;

            string sqlCa = "SELECT * FROM CALAMVIEC";
            string sqlNhanVien = "SELECT * FROM NHANVIEN";
            HamXuLy.FillCombo(sqlCa, cboCaLam, "TENCA", "MACA");
            HamXuLy.FillCombo(sqlNhanVien, cboNhanVien, "HOTEN", "MANHANVIEN");

            LoadChamCongPhanTrang();
        }

        private void LoadChamCongPhanTrang()
        {
            DataTable dt = ShowChamCongPhanTrang(currentPage, pageSize);
            luoiXepCa.DataSource = dt;

            luoiXepCa.Columns["MACHAMCONG"].HeaderText = "Mã chấm công";
            luoiXepCa.Columns["MANHANVIEN"].HeaderText = "Mã NV";
            luoiXepCa.Columns["MACA"].HeaderText = "Ca làm";
            luoiXepCa.Columns["NGAY"].HeaderText = "Ngày";
            luoiXepCa.Columns["GIOVAO"].HeaderText = "Giờ vào";
            luoiXepCa.Columns["GIORA"].HeaderText = "Giờ ra";
            luoiXepCa.Columns["TRANGTHAI"].HeaderText = "Trạng thái";

            luoiXepCa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiXepCa.ReadOnly = true;
            luoiXepCa.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lblPage.Text = string.Format("Trang {0}", currentPage);
        }

        public static DataTable ShowChamCongPhanTrang(int pageNumber, int pageSize)
        {
            HamXuLy.Connect();
            SqlConnection conn = HamXuLy.conn;
            DataTable dt = new DataTable();
            int offset = (pageNumber - 1) * pageSize;

            string sql = string.Format("SELECT * FROM CHAMCONG ORDER BY MACHAMCONG OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            return dt;
        }

        private void reset()
        {
            pnlXepCa.Enabled = false;
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
            txtMaChamCong.Text = "";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnPrevPage_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadChamCongPhanTrang();
            }
        }

        private void btnNextPage_Click_1(object sender, EventArgs e)
        {
            currentPage++;
            LoadChamCongPhanTrang();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            reset();
            pnlXepCa.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            txtMaChamCong.Text = "";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            pnlXepCa.Enabled = true;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            txtMaChamCong.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;

            if (txtMaChamCong.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mã chấm công nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int machamcong = Convert.ToInt32(luoiXepCa.CurrentRow.Cells["MACHAMCONG"].Value);
            string sql = "DELETE FROM CHAMCONG WHERE MACHAMCONG = " + machamcong;

            if (MessageBox.Show("Bạn có chắc muốn xóa ca làm này không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    HamXuLy.RunSQL(sql);
                    MessageBox.Show("Xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChamCongPhanTrang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            HamXuLy.Connect();

            string manv = cboNhanVien.SelectedValue.ToString();
            string maca = cboCaLam.SelectedValue.ToString();
            string ngay = dtpNgayXepCa.Value.ToString("yyyy-MM-dd");

            // Lấy giờ vào / giờ ra từ CALAMVIEC
            string sqlCa = string.Format("SELECT GIOBATDAU, GIOKETTHUC FROM CALAMVIEC WHERE MACA = N'{0}'", maca);
            DataTable dtCa = HamXuLy.GetDataToTable(sqlCa);

            if (dtCa.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy thông tin ca làm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string giovao = dtCa.Rows[0]["GIOBATDAU"].ToString();
            string giora = dtCa.Rows[0]["GIOKETTHUC"].ToString();

            // =================== THÊM ===================
            if (btnAdd.Enabled == true && btnEdit.Enabled == false)
            {
                // Kiểm tra trùng
                string sqlCheck = string.Format("SELECT COUNT(*) FROM CHAMCONG WHERE MANHANVIEN = {0} AND MACA = N'{1}' AND NGAY = '{2}'", manv, maca, ngay);
                int count = int.Parse(HamXuLy.GetFieldValue(sqlCheck));

                if (count > 0)
                {
                    MessageBox.Show("Ca làm này đã được xếp cho nhân viên trong ngày.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert ca mới
                string sqlInsert = string.Format(
                    "INSERT INTO CHAMCONG (MANHANVIEN, MACA, NGAY, GIOVAO, GIORA, TRANGTHAI) " +
                    "VALUES ({0}, N'{1}', '{2}', '{3}', '{4}', N'{5}')",
                    manv, maca, ngay, giovao, giora, "Chưa chấm công"
                );

                try
                {
                    HamXuLy.RunSQL(sqlInsert);
                    MessageBox.Show("Xếp ca thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm: " + ex.Message);
                }
            }

            // =================== SỬA ===================
            else if (btnEdit.Enabled == true && btnAdd.Enabled == false)
            {
                if (string.IsNullOrWhiteSpace(txtMaChamCong.Text))
                {
                    MessageBox.Show("Không tìm thấy mã chấm công để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int machamcong = Convert.ToInt32(txtMaChamCong.Text);

                string sqlUpdate = string.Format(
                    "UPDATE CHAMCONG SET MACA = N'{0}', NGAY = '{1}', GIOVAO = '{2}', GIORA = '{3}' WHERE MACHAMCONG = {4}",
                    maca, ngay, giovao, giora, machamcong
                );

                try
                {
                    HamXuLy.RunSQL(sqlUpdate);
                    MessageBox.Show("Cập nhật ca thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                }
            }

            LoadChamCongPhanTrang();
            reset();
        }





        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            string sql = "SELECT * FROM CHAMCONG WHERE MANHANVIEN IN (SELECT MANHANVIEN FROM NHANVIEN WHERE HOTEN LIKE N'%" + keyword + "%')";
            luoiXepCa.DataSource = HamXuLy.GetDataToTable(sql);
        }


        private Stack<object[]> undoStack = new Stack<object[]>();
        private Stack<object[]> redoStack = new Stack<object[]>();

        private void SaveCurrentState()
        {

        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
           
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void luoiXepCa_Click(object sender, EventArgs e)
        {
            if (luoiXepCa.CurrentRow == null) return;

            DataGridViewRow row = luoiXepCa.CurrentRow;

            txtMaChamCong.Text = row.Cells["MACHAMCONG"].Value.ToString();
            cboNhanVien.SelectedValue = row.Cells["MANHANVIEN"].Value.ToString();
            cboCaLam.SelectedValue = row.Cells["MACA"].Value.ToString();
            dtpNgayXepCa.Value = Convert.ToDateTime(row.Cells["NGAY"].Value);
        }

        private void btnThemcalam_Click(object sender, EventArgs e)
        {
            frmShift frm = new frmShift();
            frm.ShowDialog();
            this.Close();
        }


    }
}
