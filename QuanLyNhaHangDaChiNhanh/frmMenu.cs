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
    public partial class frmMenu : Form
    {
        private Stack<MonAn> undoStack = new Stack<MonAn>();
        private Stack<MonAn> redoStack = new Stack<MonAn>();

        private int currentPage = 1;
        private int pageSize = 10;
        private bool isAdding = false;
        private bool isEditing = false;
        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            HamXuLy.FillCombo("SELECT * FROM DANHMUCMON", cbDanhMuc, "TENDANHMUC", "MADANHMUC");
            HamXuLy.FillCombo("SELECT * FROM CHINHANH", cbChiNhanh, "TENCHINHANH", "MACHINHANH");
            LoadMonAnPhanTrang();
            EnableForm(false);
        }
         private void LoadMonAnPhanTrang()
        {

            DataTable dt = HamXuLy.ShowMonAnPhanTrang(currentPage, pageSize);
            luoiMonAn.DataSource = dt;

            luoiMonAn.Columns["MAMON"].HeaderText = "Mã món";
            luoiMonAn.Columns["TENMON"].HeaderText = "Tên món";
            luoiMonAn.Columns["GIA"].HeaderText = "Giá";
            luoiMonAn.Columns["HINHANH"].HeaderText = "Hình ảnh";
            luoiMonAn.Columns["GHICHU"].HeaderText = "Ghi chú";
            luoiMonAn.Columns["MADANHMUC"].HeaderText = "Danh mục";

            luoiMonAn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiMonAn.ReadOnly = true;
            luoiMonAn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            lblPage.Text = string.Format("Trang {0}", currentPage);

        }

        private void EnableForm(bool enable)
        {
            txtMaMon.ReadOnly = true;
            //txtTenMon.Enabled = enable;
            txtGia.Enabled = enable;
            txtHinhAnh.Enabled = enable;
            txtGhiChu.Enabled = enable;
            cbDanhMuc.Enabled = enable;
            btnSave.Enabled = enable;
        }

        private void ClearForm()
        {
            txtTenMon.Text = "";
            txtGia.Text = "";
            txtHinhAnh.Text = "";
            txtGhiChu.Text = "";
            cbDanhMuc.SelectedIndex = 0;
        }

        private MonAn GetCurrentMonAnFromForm()
        {
            decimal gia = 0;
            Decimal.TryParse(txtGia.Text, out gia);

            return new MonAn
            {
                MaMon = txtMaMon.Text,
                TenMon = txtTenMon.Text,
                Gia = gia,
                HinhAnh = txtHinhAnh.Text,
                GhiChu = txtGhiChu.Text,
                MaDanhMuc = cbDanhMuc.SelectedValue != null ? cbDanhMuc.SelectedValue.ToString() : "",
                MaChiNhanh = cbChiNhanh.SelectedValue != null ? cbChiNhanh.SelectedValue.ToString() : ""
            };
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
            if (luoiMonAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món ăn để sửa!");
                return;
            }

            isAdding = false;
            isEditing = true;
            EnableForm(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MonAn mon = GetCurrentMonAnFromForm();
            undoStack.Push(mon);
            redoStack.Clear();

            string sql = "";

            if (isAdding)
            {
                sql = string.Format("INSERT INTO MONAN (TENMON, GIA, HINHANH, GHICHU, MADANHMUC, MACHINHANH) " +
                                    "VALUES (N'{0}', {1}, N'{2}', N'{3}', '{4}', '{5}')",
                                    mon.TenMon, mon.Gia, mon.HinhAnh, mon.GhiChu, mon.MaDanhMuc, mon.MaChiNhanh);
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã thêm món ăn!");
            }
            else if (isEditing)
            {
                sql = string.Format("UPDATE MONAN SET TENMON = N'{0}', GIA = {1}, HINHANH = N'{2}', GHICHU = N'{3}', " +
                                    "MADANHMUC = '{4}', MACHINHANH = '{5}' WHERE MAMON = '{6}'",
                                    mon.TenMon, mon.Gia, mon.HinhAnh, mon.GhiChu, mon.MaDanhMuc, mon.MaChiNhanh, mon.MaMon);
                HamXuLy.RunSQL(sql);
                MessageBox.Show("Đã cập nhật món ăn!");
            }
            else return;

            LoadMonAnPhanTrang();
            EnableForm(false);
            ClearForm();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (luoiMonAn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn món cần xóa!");
                return;
            }

            string maMon = luoiMonAn.SelectedRows[0].Cells["MAMON"].Value.ToString();
            if (MessageBox.Show("Xác nhận xóa?", "Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = string.Format("DELETE FROM MONAN WHERE MAMON = '{0}'", maMon);
                HamXuLy.RunSQL(sql);
                LoadMonAnPhanTrang();
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                MonAn mon = redoStack.Pop();
                undoStack.Push(GetCurrentMonAnFromForm());

                txtMaMon.Text = mon.MaMon;
                txtTenMon.Text = mon.TenMon;
                txtGia.Text = mon.Gia.ToString();
                txtHinhAnh.Text = mon.HinhAnh;
                txtGhiChu.Text = mon.GhiChu;
                cbDanhMuc.SelectedValue = mon.MaDanhMuc;

                EnableForm(true);
                btnSave.Enabled = true;
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                MonAn mon = undoStack.Pop();
                redoStack.Push(GetCurrentMonAnFromForm());

                txtMaMon.Text = mon.MaMon;
                txtTenMon.Text = mon.TenMon;
                txtGia.Text = mon.Gia.ToString();
                txtHinhAnh.Text = mon.HinhAnh;
                txtGhiChu.Text = mon.GhiChu;
                cbDanhMuc.SelectedValue = mon.MaDanhMuc;

                EnableForm(true);
                btnSave.Enabled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
             string keyword = txtTenMon.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadMonAnPhanTrang();
                return;
            }

            string sql = string.Format("SELECT * FROM MONAN WHERE TENMON LIKE N'%{0}%'", keyword);
            luoiMonAn.DataSource = HamXuLy.GetDataToTable(sql);
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadMonAnPhanTrang();
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadMonAnPhanTrang();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(false);
            isAdding = false;
            isEditing = false;
        }

        private void luoiMonAn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = luoiMonAn.Rows[e.RowIndex];
                txtMaMon.Text = row.Cells["MAMON"].Value.ToString();
                txtTenMon.Text = row.Cells["TENMON"].Value.ToString();
                txtGia.Text = row.Cells["GIA"].Value.ToString();
                txtHinhAnh.Text = row.Cells["HINHANH"].Value.ToString();
                txtGhiChu.Text = row.Cells["GHICHU"].Value.ToString();
                cbDanhMuc.SelectedValue = row.Cells["MADANHMUC"].Value.ToString();

                if (row.Cells["MACHINHANH"] != null)
                    cbChiNhanh.SelectedValue = row.Cells["MACHINHANH"].Value.ToString();

                // Load hình ảnh từ thư mục (tránh lỗi file lock)
                string imagePath = Application.StartupPath + @"\HinhAnhMonAn\" + txtHinhAnh.Text;
                if (File.Exists(imagePath))
                {
                    using (Image img = Image.FromFile(imagePath))
                    {
                        picHinhAnh.Image = new Bitmap(img);
                    }
                    picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    picHinhAnh.Image = null;
                }
            }
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDashBoard f = new frmDashBoard();
            f.FormClosed += (s, args) => this.Close();
            f.Show(); 
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn hình ảnh món ăn";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    using (Image img = Image.FromFile(filePath))
                    {
                        picHinhAnh.Image = new Bitmap(img);
                    }
                    picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Không thể mở hình ảnh. File không đúng định dạng hoặc bị hỏng!", "Lỗi ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi mở ảnh: " + ex.Message);
                }


                // Lưu tên file ảnh vào textbox
                txtHinhAnh.Text = Path.GetFileName(filePath);

                // Tạo thư mục ảnh nếu chưa có
                string destinationFolder = Application.StartupPath + @"\HinhAnhMonAn\";
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                // Copy ảnh vào thư mục dự án nếu chưa tồn tại
                string destFile = Path.Combine(destinationFolder, txtHinhAnh.Text);
                if (!File.Exists(destFile))
                {
                    File.Copy(filePath, destFile);
                }
            }
        }




    }
}
