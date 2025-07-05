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
    public partial class frmNguyenVatLieu : Form
    {
        private Stack<NguyenVatLieu> undoStack = new Stack<NguyenVatLieu>();
        private Stack<NguyenVatLieu> redoStack = new Stack<NguyenVatLieu>();

        private int currentPage = 1;
        private int pageSize = 10;

        private bool isAdding = false;
        private bool isEditing = false;

        public frmNguyenVatLieu()
        {
            InitializeComponent();
        }

        private void LuoiNVL_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoiNVL.Rows[e.RowIndex];
                txtMaNVL.Text = row.Cells["MANGUYENLIEU"].Value.ToString();
                txtTenNVL.Text = row.Cells["TENNGUYENLIEU"].Value.ToString();
                txtDonViTinh.Text = row.Cells["DONVITINH"].Value.ToString();
                txtDonGiaNhap.Text = row.Cells["DONGIANHAP"].Value.ToString();
                txtSLTon.Text = row.Cells["SOLUONGTON"].Value.ToString();
                txtSLToiThieu.Text = row.Cells["SOLUONGTOITHIEU"].Value.ToString();
                txtGhiChu.Text = row.Cells["GHICHU"].Value.ToString();
                cbMaNCC.SelectedValue = row.Cells["MANCC"].Value.ToString();
            }
        }
        private void cbMaChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaChiNhanh.SelectedValue != null)
                LoadNguyenLieuPhanTrang();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadNguyenLieuPhanTrang();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadNguyenLieuPhanTrang();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isAdding = true; isEditing = false;
            ClearForm(); EnableForm(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoiNVL.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn nguyên liệu để sửa.");
                return;
            }

            isAdding = false; isEditing = true;
            EnableForm(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (luoiNVL.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn nguyên liệu để xóa.");
                return;
            }

            string maNL = luoiNVL.SelectedRows[0].Cells["MANGUYENLIEU"].Value.ToString();
            string sql = string.Format("DELETE FROM KHO_NGUYENLIEU WHERE MANGUYENLIEU = '{0}' AND MACHINHANH = '{1}'; DELETE FROM NGUYENLIEU WHERE MANGUYENLIEU = '{0}'", maNL, cbMaChiNhanh.SelectedValue);

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HamXuLy.RunSQL(sql);
                LoadNguyenLieuPhanTrang();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NguyenVatLieu nvl = GetCurrentNguyenLieuForm();
            undoStack.Push(nvl);
            redoStack.Clear();

            string sql = "";
            if (isAdding)
            {
                sql = string.Format(@"INSERT INTO NGUYENLIEU (TENNGUYENLIEU, DONVITINH, DONGIANHAP, GHICHU, MANCC) 
                                      VALUES (N'{0}', N'{1}', {2}, N'{3}', '{4}');
                                      INSERT INTO KHO_NGUYENLIEU (MANGUYENLIEU, MACHINHANH, SOLUONGTON, SOLUONGTOITHIEU, NGAYNHAPGANNHAT) 
                                      SELECT MAX(MANGUYENLIEU), '{5}', {6}, {7}, GETDATE();",
                                      nvl.TenNguyenLieu, nvl.DonViTinh, nvl.DonGiaNhap, nvl.GhiChu, nvl.MaNCC,
                                      cbMaChiNhanh.SelectedValue, nvl.SoLuongTon, nvl.SoLuongToiThieu);
                MessageBox.Show("Đã thêm nguyên liệu.");
            }
            else if (isEditing)
            {
                sql = string.Format(@"UPDATE NGUYENLIEU SET TENNGUYENLIEU = N'{0}', DONVITINH = N'{1}', DONGIANHAP = {2}, GHICHU = N'{3}', MANCC = '{4}' 
                                      WHERE MANGUYENLIEU = '{5}';
                                      UPDATE KHO_NGUYENLIEU SET SOLUONGTON = {6}, SOLUONGTOITHIEU = {7} 
                                      WHERE MANGUYENLIEU = '{5}' AND MACHINHANH = '{8}';",
                                      nvl.TenNguyenLieu, nvl.DonViTinh, nvl.DonGiaNhap, nvl.GhiChu, nvl.MaNCC, nvl.MaNguyenLieu,
                                      nvl.SoLuongTon, nvl.SoLuongToiThieu, cbMaChiNhanh.SelectedValue);
                MessageBox.Show("Đã cập nhật nguyên liệu.");
            }

            HamXuLy.RunSQL(sql);
            LoadNguyenLieuPhanTrang();
            EnableForm(false);
            ClearForm();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                NguyenVatLieu nvl = redoStack.Pop();
                undoStack.Push(GetCurrentNguyenLieuForm());

                txtMaNVL.Text = nvl.MaNguyenLieu;
                txtTenNVL.Text = nvl.TenNguyenLieu;
                txtDonViTinh.Text = nvl.DonViTinh;
                txtDonGiaNhap.Text = nvl.DonGiaNhap.ToString();
                txtSLTon.Text = nvl.SoLuongTon.ToString();
                txtSLToiThieu.Text = nvl.SoLuongToiThieu.ToString();
                txtGhiChu.Text = nvl.GhiChu;
                cbMaNCC.SelectedValue = nvl.MaNCC;

                EnableForm(true);
                btnSave.Enabled = true;
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                NguyenVatLieu nvl = undoStack.Pop();
                redoStack.Push(GetCurrentNguyenLieuForm());

                txtMaNVL.Text = nvl.MaNguyenLieu;
                txtTenNVL.Text = nvl.TenNguyenLieu;
                txtDonViTinh.Text = nvl.DonViTinh;
                txtDonGiaNhap.Text = nvl.DonGiaNhap.ToString();
                txtSLTon.Text = nvl.SoLuongTon.ToString();
                txtSLToiThieu.Text = nvl.SoLuongToiThieu.ToString();
                txtGhiChu.Text = nvl.GhiChu;
                cbMaNCC.SelectedValue = nvl.MaNCC;

                EnableForm(true);
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(false);
            isAdding = false;
            isEditing = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTenNVL.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadNguyenLieuPhanTrang();
                return;
            }

            string sql = "SELECT * FROM NGUYENLIEU WHERE TENNGUYENLIEU LIKE N'%{keyword}%'";
            luoiNVL.DataSource = HamXuLy.GetDataToTable(sql);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmNguyenVatLieu_Load(object sender, EventArgs e)
        {
            HamXuLy.FillCombo("SELECT MACHINHANH, TENCHINHANH FROM CHINHANH", cbMaChiNhanh, "TENCHINHANH", "MACHINHANH");
            HamXuLy.FillCombo("SELECT MANCC, TENNCC FROM NHACUNGCAP", cbMaNCC, "TENNCC", "MANCC");

            if (cbMaChiNhanh.Items.Count > 0) cbMaChiNhanh.SelectedIndex = 0;
            LoadNguyenLieuPhanTrang();
            EnableForm(false);
        }
        private DataTable ShowNguyenLieuPhanTrang(int page, int pageSize, string maChiNhanh)
        {
            HamXuLy.Connect();
            int startRow = (page - 1) * pageSize + 1;
            int endRow = page * pageSize;

            string sql = string.Format(@"
            SELECT * FROM (
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY NL.MANGUYENLIEU ASC) AS RowNum,
                    NL.MANGUYENLIEU, NL.TENNGUYENLIEU, NL.DONVITINH, NL.DONGIANHAP,
                    KNL.SOLUONGTON, KNL.SOLUONGTOITHIEU, KNL.NGAYNHAPGANNHAT,
                    NL.GHICHU, NL.MANCC, KNL.MACHINHANH
                FROM KHO_NGUYENLIEU KNL
                JOIN NGUYENLIEU NL ON KNL.MANGUYENLIEU = NL.MANGUYENLIEU
                WHERE KNL.MACHINHANH = '{0}'
            ) AS Temp
            WHERE RowNum BETWEEN {1} AND {2}",
                maChiNhanh, startRow, endRow);

            return HamXuLy.GetDataToTable(sql);
        }
        private void LoadNguyenLieuPhanTrang()
        {
            string maCN = "";
            if (cbMaChiNhanh.SelectedValue != null)
            {
                maCN = cbMaChiNhanh.SelectedValue.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn chi nhánh!");
                return;
            }

            int start = (currentPage - 1) * pageSize + 1;
            int end = currentPage * pageSize;

            string sql = string.Format(@"
                SELECT * FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY NL.MANGUYENLIEU) AS RowNum,
                           NL.MANGUYENLIEU, NL.TENNGUYENLIEU, NL.DONVITINH, NL.DONGIANHAP,
                           KNL.SOLUONGTON, KNL.SOLUONGTOITHIEU, KNL.NGAYNHAPGANNHAT,
                           NL.GHICHU, NL.MANCC
                    FROM NGUYENLIEU NL
                    JOIN KHO_NGUYENLIEU KNL ON NL.MANGUYENLIEU = KNL.MANGUYENLIEU
                    WHERE KNL.MACHINHANH = '{0}'
                ) AS Temp
                WHERE RowNum BETWEEN {1} AND {2}", maCN, start, end);

            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoiNVL.DataSource = dt;

            if (dt.Columns.Contains("RowNum")) luoiNVL.Columns["RowNum"].Visible = false;

            luoiNVL.Columns["MANGUYENLIEU"].HeaderText = "Mã NL";
            luoiNVL.Columns["TENNGUYENLIEU"].HeaderText = "Tên nguyên liệu";
            luoiNVL.Columns["DONVITINH"].HeaderText = "Đơn vị";
            luoiNVL.Columns["DONGIANHAP"].HeaderText = "Giá nhập";
            luoiNVL.Columns["SOLUONGTON"].HeaderText = "SL tồn";
            luoiNVL.Columns["SOLUONGTOITHIEU"].HeaderText = "SL tối thiểu";
            luoiNVL.Columns["NGAYNHAPGANNHAT"].HeaderText = "Ngày nhập";
            luoiNVL.Columns["GHICHU"].HeaderText = "Ghi chú";
            luoiNVL.Columns["MANCC"].HeaderText = "Mã NCC";

            luoiNVL.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiNVL.ReadOnly = true;
            luoiNVL.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblPage.Text = "Trang " + currentPage;
        }

        private void EnableForm(bool enable)
        {
            txtMaNVL.ReadOnly = true;
            txtTenNVL.Enabled = enable;
            txtDonViTinh.Enabled = enable;
            txtDonGiaNhap.Enabled = enable;
            txtSLTon.Enabled = enable;
            txtSLToiThieu.Enabled = enable;
            txtGhiChu.Enabled = enable;
            cbMaChiNhanh.Enabled = enable;
            cbMaNCC.Enabled = enable;
            btnSave.Enabled = enable;
        }

        private void ClearForm()
        {
            txtTenNVL.Text = "";
            txtDonViTinh.Text = "";
            txtDonGiaNhap.Text = "";
            txtSLTon.Text = "";
            txtSLToiThieu.Text = "";
            txtGhiChu.Text = "";
            cbMaChiNhanh.SelectedIndex = 0;
            cbMaNCC.SelectedIndex = 0;
        }

        private NguyenVatLieu GetCurrentNguyenLieuForm()
        {
            decimal gia = 0;
            float ton = 0, toiThieu = 0;

            Decimal.TryParse(txtDonGiaNhap.Text, out gia);
            float.TryParse(txtSLTon.Text, out ton);
            float.TryParse(txtSLToiThieu.Text, out toiThieu);

            return new NguyenVatLieu
            {
                MaNguyenLieu = txtMaNVL.Text,
                TenNguyenLieu = txtTenNVL.Text,
                DonViTinh = txtDonViTinh.Text,
                DonGiaNhap = gia,
                SoLuongTon = ton,
                SoLuongToiThieu = toiThieu,
                GhiChu = txtGhiChu.Text,
                MaNCC = cbMaNCC.SelectedValue != null ? cbMaNCC.SelectedValue.ToString() : ""
            };
        }

    }
}
