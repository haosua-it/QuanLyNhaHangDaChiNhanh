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
    public partial class FrmBan : Form
    {
        public FrmBan()
        {
            InitializeComponent();
        }
        
        private Stack<BanAn> undoStack = new Stack<BanAn>();
        private Stack<BanAn> redoStack = new Stack<BanAn>();

        
        private int currentPage = 1;
        private int pageSize = 10;

        private void FrmBan_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();

            
            HamXuLy.FillCombo("SELECT * FROM CHINHANH", cbChiNhanh, "TENCHINHANH", "MACHINHANH");

            
            HamXuLy.FillCombo("SELECT * FROM KHU", cbMaKhu, "MAKHU", "MAKHU");

            cbTrangThai.Items.Add("Bàn Trống");
            cbTrangThai.Items.Add("Bàn Có Khách");

            
            LoadBanAnPhanTrang();
            EnableForm(false);
        }
        private void LoadBanAnPhanTrang()
        {
            DataTable dt = HamXuLy.ShowBanAnPhanTrang(currentPage, pageSize);
            luoiBanAn.DataSource = dt;

            luoiBanAn.Columns["MABAN"].HeaderText = "Mã Bàn";
            luoiBanAn.Columns["TENBAN"].HeaderText = "Tên Bàn";
            luoiBanAn.Columns["TRANGTHAI"].HeaderText = "Trạng Thái";
            luoiBanAn.Columns["MAKHU"].HeaderText = "Khu";
            luoiBanAn.Columns["MACHINHANH"].HeaderText = "Chi nhánh";

            luoiBanAn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiBanAn.ReadOnly = true;
            luoiBanAn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblPage.Text = string.Format("Trang {0}", currentPage);
        }

        private void luoiBanAn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableForm(true);
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoiBanAn.Rows[e.RowIndex];

                txtMaBan.Text = row.Cells["MABAN"].Value.ToString();
                txtTenBan.Text = row.Cells["TENBAN"].Value.ToString();
                cbChiNhanh.SelectedValue = row.Cells["MACHINHANH"].Value.ToString();
                cbMaKhu.SelectedValue = row.Cells["MAKHU"].Value.ToString();
                cbTrangThai.Text = row.Cells["TRANGTHAI"].Value.ToString();

            }

        }
        //======================HÀM DÙNG ĐỂ KHÓA/MỞ Ô NHẬP DỮ LIỆU
        private void EnableForm(bool enable)
        {
            txtMaBan.ReadOnly = true;
            txtTenBan.Enabled = enable;
            cbChiNhanh.Enabled = enable;
            cbMaKhu.Enabled = enable;
            cbTrangThai.Enabled = enable;
            btnSave.Enabled = enable;
        }
        //===========resetBTN//
        private void ResetButtonState()
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnUndo.Enabled = true;
            btnRedo.Enabled = true;
            btnBack.Enabled = true;
        }
        private void ClearForm() 
        {
            txtTenBan.Text = "";
            cbMaKhu.SelectedIndex = 0;
            cbChiNhanh.SelectedIndex = 0;
            cbTrangThai.SelectedIndex = 0;
        }
        private bool isAdding = false;
        private bool isEditing = false;
        //============================ CHỨC NĂNG THÊM BÀN Ở ĐÂY
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(true);

            isAdding = true;
            isEditing = false;

            // Trạng thái các nút khi Thêm
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnUndo.Enabled = false;
            btnRedo.Enabled = false;
            btnBack.Enabled = true;

        }
        //=========================== CHỨC NĂNG LƯU Ở ĐÂY, LƯU CỦA THÊM VÀ CỦA SỬA
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Trước khi sửa hoặc xóa
            BanAn currentBA = new BanAn
            {
                MaBan = txtMaBan.Text,
                TenBan = txtTenBan.Text,
                MaChiNhanh = cbChiNhanh.SelectedValue.ToString(),
                MaKhu = cbMaKhu.SelectedValue.ToString(),
                TrangThai = cbTrangThai.Text
            };
            undoStack.Push(currentBA);
            redoStack.Clear(); // Khi có thao tác mới, clear redoStack
            string maban = txtMaBan.Text.Trim();
            string tenban = txtTenBan.Text.Trim();
            string trangthai = cbTrangThai.Text;
            string machinhanh = cbChiNhanh.SelectedValue.ToString();
            string makhu = cbMaKhu.SelectedValue.ToString();

            HamXuLy.Connect();

            if (isAdding)
            {
                string sql = "INSERT INTO BANAN (MABAN, TENBAN, MACHINHANH, TRANGTHAI, MAKHU) VALUES ('" + maban + "', N'" + tenban + "', '" + machinhanh + "','" + trangthai + "', '" + makhu + "')";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm bàn!");
            }
            else if (isEditing)
            {
                string MaBan = txtMaBan.Text;
                string sql = "UPDATE BANAN SET" +
                             "TENBAN = N'" + tenban + "', " +
                             "MACHINHANH = '" + machinhanh + "', " +
                             "TRANGTHAI = N'" + trangthai + "', " +
                             "MAKHU = '" + makhu + "' " +
                             "WHERE MABAN = '" + MaBan + "'";
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật thông tin bàn");
            }
            LoadBanAnPhanTrang();
            HamXuLy.Disconnect();
            ResetButtonState();
            EnableForm(false);
            ClearForm();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (luoiBanAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một bàn ăn để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableForm(true); // Cho phép nhập liệu

            isEditing = true;
            isAdding = false;

            // Trạng thái các nút khi sửa
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnUndo.Enabled = false;
            btnRedo.Enabled = false;
            btnBack.Enabled = true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (luoiBanAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn ăn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string MaBan = luoiBanAn.SelectedRows[0].Cells["MABAN"].Value.ToString();

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    HamXuLy.Connect();
                    string sql = "DELETE FROM BANAN WHERE MABAN = '" + MaBan + "'";
                    HamXuLy.RunSQL(sql);
                    HamXuLy.Disconnect();

                    MessageBox.Show("Đã xóa bàn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBanAnPhanTrang();
                    ClearForm();
                    EnableForm(false);
                    ResetButtonState();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // CHỨC NĂNG NÚT IN
        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
        private BanAn GetCurrentBanAnFromForm()
        {
            return new BanAn
            {
                MaBan = txtMaBan.Text,
                TenBan = txtTenBan.Text,
                MaChiNhanh = cbChiNhanh.SelectedValue == null ? "" : cbChiNhanh.SelectedValue.ToString(),
                TrangThai = cbTrangThai.Text,
                MaKhu = cbMaKhu.SelectedValue == null ? "" : cbMaKhu.SelectedValue.ToString()

            };
        }
        private void btnRedo_Click(object sender, EventArgs e)
        {

        }
        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                BanAn ba = redoStack.Pop();
                undoStack.Push(GetCurrentBanAnFromForm());

                txtMaBan.Text = ba.MaBan;
                txtTenBan.Text = ba.TenBan;
                cbChiNhanh.SelectedValue = ba.MaChiNhanh;
                cbMaKhu.SelectedValue = ba.MaKhu;
                cbTrangThai.Text = ba.TrangThai;

                EnableForm(true);
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Không có thao tác nào để hoàn tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(false);
            ResetButtonState();
            isAdding = false;
            isEditing = false;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTenBan.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadBanAnPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = "SELECT * FROM BANAN WHERE TENBAN LIKE N'%" + keyword + "%'";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoiBanAn.DataSource = dt;
            HamXuLy.Disconnect();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadBanAnPhanTrang();
        }
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadBanAnPhanTrang();
            }
        }

        private void cbMaKhu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}