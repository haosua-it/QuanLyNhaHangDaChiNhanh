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
    public partial class frmShift : Form
    {
        private int currentPage = 1;
        private int pageSize = 10;

        public frmShift()
        {
            InitializeComponent();
        }

        private void frmShift_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            pnlNguoiDung.Enabled = false;
            txtMaCaLam.Enabled = false;
            LoadCaLamViecPhanTrang();
        }

        private void reset()
        {
            pnlNguoiDung.Enabled = false;
            txtMaCaLam.Text = "";
            txtTenCaLam.Text = "";
            dtpBatDau.Value = DateTime.Now;
            dtpKetThuc.Value = DateTime.Now;
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

        private void LoadCaLamViecPhanTrang()
        {
            DataTable dt = ShowCaLamViecPhanTrang(currentPage, pageSize);
            luoiCaLamViec.DataSource = dt;

            luoiCaLamViec.Columns["MACA"].HeaderText = "Mã ca làm";
            luoiCaLamViec.Columns["TENCA"].HeaderText = "Tên ca";
            luoiCaLamViec.Columns["GIOBATDAU"].HeaderText = "Giờ bắt đầu";
            luoiCaLamViec.Columns["GIOKETTHUC"].HeaderText = "Giờ kết thúc";

            luoiCaLamViec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiCaLamViec.ReadOnly = true;
            luoiCaLamViec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lblPage.Text = string.Format("Trang {0}", currentPage);
        }

        public static DataTable ShowCaLamViecPhanTrang(int pageNumber, int pageSize)
        {
            HamXuLy.Connect();
            SqlConnection conn = HamXuLy.conn;
            DataTable dt = new DataTable();
            int offset = (pageNumber - 1) * pageSize;
            string sql = string.Format("SELECT * FROM CALAMVIEC ORDER BY MACA OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            return dt;
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadCaLamViecPhanTrang();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadCaLamViecPhanTrang();
        }

        private void luoiCaLamViec_Click(object sender, EventArgs e)
        {
            if (luoiCaLamViec.CurrentRow == null) return;
            var row = luoiCaLamViec.CurrentRow;
            txtMaCaLam.Text = row.Cells["MACA"].Value.ToString();
            txtTenCaLam.Text = row.Cells["TENCA"].Value.ToString();
            dtpBatDau.Value = DateTime.Today.Add((TimeSpan)row.Cells["GIOBATDAU"].Value);
            dtpKetThuc.Value = DateTime.Today.Add((TimeSpan)row.Cells["GIOKETTHUC"].Value);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            reset();
            txtMaCaLam.Text = HamXuLy.MaTuDong("CALAMVIEC");
            pnlNguoiDung.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            txtTenCaLam.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            pnlNguoiDung.Enabled = true;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            txtTenCaLam.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMaCaLam.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mã ca làm");
                return;
            }

            string sql = "DELETE FROM CALAMVIEC WHERE MACA = '" + txtMaCaLam.Text + "'";
            if (MessageBox.Show("Bạn có chắc muốn xóa ca làm này không?", "Xác nhận xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Xóa thành công");
                LoadCaLamViecPhanTrang();
                reset();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TimeSpan batDau = dtpBatDau.Value.TimeOfDay;
            TimeSpan ketThuc = dtpKetThuc.Value.TimeOfDay;

            if (btnEdit.Enabled == true)
            {
                string sqlUpdate = string.Format(@"
    UPDATE CALAMVIEC SET
        TENCA = N'{0}',
        GIOBATDAU = '{1}',
        GIOKETTHUC = '{2}'
    WHERE MACA = N'{3}'",
    txtTenCaLam.Text,
    dtpBatDau.Value.ToString("HH:mm:ss"),
    dtpKetThuc.Value.ToString("HH:mm:ss"),
    txtMaCaLam.Text
);
                HamXuLy.RunSQL(sqlUpdate);

            }
            else
            {
                string sqlInsert = string.Format(@"
    INSERT INTO CALAMVIEC (MACA, TENCA, GIOBATDAU, GIOKETTHUC)
    VALUES (N'{0}', N'{1}', '{2}', '{3}')",
    txtMaCaLam.Text,
    txtTenCaLam.Text,
    dtpBatDau.Value.ToString("HH:mm:ss"),
    dtpKetThuc.Value.ToString("HH:mm:ss")
);
                HamXuLy.RunSQL(sqlInsert);

            }

            LoadCaLamViecPhanTrang();
            reset();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadCaLamViecPhanTrang();
                return;
            }

            string sql = "SELECT * FROM CALAMVIEC WHERE TENCA LIKE N'%" + keyword + "%'";
            luoiCaLamViec.DataSource = HamXuLy.GetDataToTable(sql);
        }

        private void btnUndo_Click(object sender, EventArgs e) { }
        private void btnRedo_Click(object sender, EventArgs e) { }
        private void btnPrint_Click(object sender, EventArgs e) { }
    }
}
