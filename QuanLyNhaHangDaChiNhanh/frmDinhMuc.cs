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
    public partial class frmDinhMuc : Form
    {
        public frmDinhMuc()
        {
            InitializeComponent();
        }
        private int currentPage = 1;
        private int pageSize = 10;
        private void frmDinhMuc_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            pnlDinhMuc.Enabled = false;
            cboNguyenLieu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMonAn.DropDownStyle = ComboBoxStyle.DropDownList;
            txtSoLuong.Enabled = true;

            string sqlNguyenLieu = "SELECT MANGUYENLIEU, TENNGUYENLIEU FROM NGUYENLIEU";
            string sqlMonAn = "SELECT MAMON, TENMON FROM MONAN";

            HamXuLy.FillCombo(sqlNguyenLieu, cboNguyenLieu, "TENNGUYENLIEU", "MANGUYENLIEU");
            HamXuLy.FillCombo(sqlMonAn, cboMonAn, "TENMON", "MAMON");

            LoadDinhMuc();
        }

        private void LoadDinhMuc()
{
    int offset = (currentPage - 1) * pageSize;

    string sql = string.Format("   SELECT D.MAMON, M.TENMON, D.MANGUYENLIEU, N.TENNGUYENLIEU, D.SOLUONG FROM DINHMUC D  JOIN MONAN M ON M.MAMON = D.MAMONJOIN NGUYENLIEU N ON N.MANGUYENLIEU = D.MANGUYENLIEU ORDER BY M.TENMON OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);

    DataTable dt = HamXuLy.GetDataToTable(sql);
    luoiNguoiDung.DataSource = dt;

    luoiNguoiDung.Columns["MAMON"].HeaderText = "Mã món";
    luoiNguoiDung.Columns["TENMON"].HeaderText = "Tên món";
    luoiNguoiDung.Columns["MANGUYENLIEU"].HeaderText = "Mã nguyên liệu";
    luoiNguoiDung.Columns["TENNGUYENLIEU"].HeaderText = "Nguyên liệu";
    luoiNguoiDung.Columns["SOLUONG"].HeaderText = "Số lượng";

    luoiNguoiDung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    luoiNguoiDung.ReadOnly = true;
    luoiNguoiDung.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                lblPage.Text = string.Format("Trang {0}", currentPage);
}


        private void luoiNguoiDung_Click(object sender, EventArgs e)
        {
            if (luoiNguoiDung.CurrentRow == null) return;

            DataGridViewRow row = luoiNguoiDung.CurrentRow;
            cboMonAn.SelectedValue = row.Cells["MAMON"].Value;
            cboNguyenLieu.SelectedValue = row.Cells["MANGUYENLIEU"].Value;
            txtSoLuong.Text = row.Cells["SOLUONG"].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            pnlDinhMuc.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            txtSoLuong.Text = "";
            cboMonAn.SelectedIndex = -1;
            cboNguyenLieu.SelectedIndex = -1;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            pnlDinhMuc.Enabled = true;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string mamon = cboMonAn.SelectedValue.ToString();
            string manguyenlieu = cboNguyenLieu.SelectedValue.ToString();

            string sql = string.Format("DELETE FROM DINHMUC WHERE MAMON = {0} AND MANGUYENLIEU = {1}", mamon, manguyenlieu);

            if (MessageBox.Show("Bạn có chắc muốn xóa định mức này không?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HamXuLy.RunSQL(sql);
                LoadDinhMuc();
                pnlDinhMuc.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string mamon = cboMonAn.SelectedValue.ToString();
            string manguyenlieu = cboNguyenLieu.SelectedValue.ToString();
            string soluong = txtSoLuong.Text;

            if (btnAdd.Enabled == true) // thêm
            {
                string sqlCheck = string.Format("SELECT COUNT(*) FROM DINHMUC WHERE MAMON = {0} AND MANGUYENLIEU = {1}", mamon, manguyenlieu);
                int count = int.Parse(HamXuLy.GetFieldValue(sqlCheck));
                if (count > 0)
                {
                    MessageBox.Show("Định mức này đã tồn tại.");
                    return;
                }

                string sqlInsert = string.Format("INSERT INTO DINHMUC(MAMON, MANGUYENLIEU, SOLUONG) VALUES ({0}, {1}, {2})", mamon, manguyenlieu, soluong);
                HamXuLy.RunSQL(sqlInsert);
            }
            else // sửa
            {
                string sqlUpdate = string.Format("UPDATE DINHMUC SET SOLUONG = {2} WHERE MAMON = {0} AND MANGUYENLIEU = {1}", mamon, manguyenlieu, soluong);
                HamXuLy.RunSQL(sqlUpdate);
            }

            MessageBox.Show("Lưu thành công.", "Thông báo");
            LoadDinhMuc();
            pnlDinhMuc.Enabled = false;
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadDinhMuc();
        }
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDinhMuc();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDinhMuc.Enabled = false;
            txtSoLuong.Text = "";
            cboMonAn.SelectedIndex = -1;
            cboNguyenLieu.SelectedIndex = -1;
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            string sql = @"SELECT D.MAMON, M.TENMON, D.MANGUYENLIEU, N.TENNGUYENLIEU, D.SOLUONG
                   FROM DINHMUC D
                   JOIN MONAN M ON M.MAMON = D.MAMON
                   JOIN NGUYENLIEU N ON N.MANGUYENLIEU = D.MANGUYENLIEU
                   WHERE M.TENMON LIKE N'%" + keyword + @"%' OR N.TENNGUYENLIEU LIKE N'%" + keyword + @"%'";
            luoiNguoiDung.DataSource = HamXuLy.GetDataToTable(sql);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng in chưa được triển khai.", "Thông báo");
        }

        private Stack<object[]> undoStack = new Stack<object[]>();
        private Stack<object[]> redoStack = new Stack<object[]>();

        private void SaveCurrentState()
        {
            object[] state = new object[]
    {
        cboMonAn.SelectedValue,
        cboNguyenLieu.SelectedValue,
        txtSoLuong.Text
    };
            undoStack.Push(state);
            redoStack.Clear();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                object[] state = undoStack.Pop();
                redoStack.Push(new object[] { cboMonAn.SelectedValue, cboNguyenLieu.SelectedValue, txtSoLuong.Text });
                cboMonAn.SelectedValue = state[0];
                cboNguyenLieu.SelectedValue = state[1];
                txtSoLuong.Text = state[2].ToString();
            }
            else
            {
                MessageBox.Show("Không có thao tác để hoàn tác.", "Thông báo");
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                SaveCurrentState();
                object[] state = redoStack.Pop();
                cboMonAn.SelectedValue = state[0];
                cboNguyenLieu.SelectedValue = state[1];
                txtSoLuong.Text = state[2].ToString();
            }
            else
            {
                MessageBox.Show("Không có thao tác để làm lại.", "Thông báo");
            }
        }



    }
}
