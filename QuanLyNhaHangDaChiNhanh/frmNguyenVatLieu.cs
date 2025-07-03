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
        public frmNguyenVatLieu()
        {
            InitializeComponent();
        }
        private Stack<NguyenVatLieu> undoStack = new Stack<NguyenVatLieu>();
        private Stack<NguyenVatLieu> redoStack = new Stack<NguyenVatLieu>();

        private int currentPage = 1;
        private int pageSize = 10;
        private bool isAdding = false;
        private bool isEditing = false;


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
            isAdding = true;
            isEditing = false;
            EnableForm(true);
            ClearForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoiNVL.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu để sửa!");
                return;
            }

            isAdding = false;
            isEditing = true;
            EnableForm(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (luoiNVL.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn nguyên liệu cần xóa!");
                return;
            }

            string maNL = luoiNVL.SelectedRows[0].Cells["MANGUYENLIEU"].Value.ToString();
            if (MessageBox.Show("Xác nhận xóa?", "Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM NGUYENLIEU WHERE MANGUYENLIEU = '{maNL}'";
                HamXuLy.RunSQL(sql);
                LoadNguyenLieuPhanTrang();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NguyenVatLieu nvl = GetCurrentNguyenLieuFromForm();
            undoStack.Push(nvl);
            redoStack.Clear();

            string sql = "";

            if (isAdding)
            {
                sql = string.Format("INSERT INTO NGUYENLIEU (TENNGUYENLIEU, DONVITINH, DONGIANHAP, SOLUONGTON, SOLUONGTOITHIEU, NGAYNHAPGANNHAT, GHICHU, MANCC) " +
                                    "VALUES (N'{0}', N'{1}', {2}, {3}, {4}, GETDATE(), N'{5}', {6})",
                                    nvl.TenNguyenLieu, nvl.DonViTinh, nvl.DonGiaNhap, nvl.SoLuongTon, nvl.SoLuongToiThieu, nvl.GhiChu, nvl.MaNCC);
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm nguyên liệu!");
            }
            else if (isEditing)
            {
                sql = string.Format("UPDATE NGUYENLIEU SET TENNGUYENLIEU = N'{0}', DONVITINH = N'{1}', DONGIANHAP = {2}, " +
                                    "SOLUONGTON = {3}, SOLUONGTOITHIEU = {4}, GHICHU = N'{5}', MANCC = {6} " +
                                    "WHERE MANGUYENLIEU = '{7}'",
                                    nvl.TenNguyenLieu, nvl.DonViTinh, nvl.DonGiaNhap, nvl.SoLuongTon, nvl.SoLuongToiThieu, nvl.GhiChu, nvl.MaNCC, nvl.MaNguyenLieu);
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật nguyên liệu!");
            }
            else return;

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
                undoStack.Push(GetCurrentNguyenLieuFromForm());

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
                redoStack.Push(GetCurrentNguyenLieuFromForm());

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
            this.Hide();
            frmDashBoard f = new frmDashBoard();
            f.FormClosed += (s, args) => this.Close();
            f.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmNguyenVatLieu_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();

            HamXuLy.FillCombo("SELECT MACHINHANH, TENCHINHANH FROM CHINHANH", cbMaChiNhanh, "TENCHINHANH", "MACHINHANH");

            if (cbMaChiNhanh.Items.Count > 0)
            {
                cbMaChiNhanh.SelectedIndex = 0;
            }

            HamXuLy.FillCombo("SELECT MANCC, TENNCC FROM NHACUNGCAP", cbMaNCC, "TENNCC", "MANCC");

            // 🟢 Bây giờ cbMaChiNhanh đã có giá trị, mới được gọi
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
            if (cbMaChiNhanh.SelectedValue == null)
                return;

            string maChiNhanh = cbMaChiNhanh.SelectedValue.ToString();
            DataTable dt = ShowNguyenLieuPhanTrang(currentPage, pageSize, maChiNhanh);

            luoiNVL.DataSource = dt;

            luoiNVL.Columns["MANGUYENLIEU"].HeaderText = "Mã NL";
            luoiNVL.Columns["TENNGUYENLIEU"].HeaderText = "Tên NL";
            luoiNVL.Columns["DONVITINH"].HeaderText = "ĐVT";
            luoiNVL.Columns["DONGIANHAP"].HeaderText = "Đơn giá";
            luoiNVL.Columns["SOLUONGTON"].HeaderText = "Tồn kho";
            luoiNVL.Columns["SOLUONGTOITHIEU"].HeaderText = "Tối thiểu";
            luoiNVL.Columns["NGAYNHAPGANNHAT"].HeaderText = "Ngày nhập gần nhất";
            luoiNVL.Columns["GHICHU"].HeaderText = "Ghi chú";
            luoiNVL.Columns["MANCC"].HeaderText = "Nhà cung cấp";

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

        private NguyenVatLieu GetCurrentNguyenLieuFromForm()
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
